using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.QuickSearch;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SLPathFinding : MonoBehaviour
{
    public int widthCount = 150;
    public int heightCount = 150;

    public float blockPercent = 10;

    public bool use_JPS = true;
    public int random_seed = 0;

    Mesh pMesh;
    public Vector3 m_CubeScale = new Vector3(0.5f, 0.5f, 1);

    GameObject[][] m_cubeMap;
    bool[][] m_wallMap;

    static Dictionary<int, Dictionary<int, int>> G = new Dictionary<int, Dictionary<int, int>>();
    static Dictionary<int, Dictionary<int, bool>> Added = new Dictionary<int, Dictionary<int, bool>>();
    //static Dictionary<int, Dictionary<int, bool>> Added = new Dictionary<int, Dictionary<int, bool>>(); // ==> TODO: JPS shall add a new ADDed-variable with direction

    PFHeap openSet = new PFHeap();
    static int[] m_start;
    static int[] m_end;

    private void Awake()
    {
        pMesh = transform.GetComponent<MeshFilter>().mesh;
        GenerateWallMap();
        GenerateBlocks();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        if (use_JPS) 
            WalkPath_JPS(new int[] { 0, 0 }, new int[] { widthCount - 1, heightCount - 1 });
        else
            WalkPath_AStar(new int[] { 0, 0 }, new int[] { widthCount - 1, heightCount - 1 });

        stopwatch.Stop();
        Debug.Log($"Execution: {stopwatch.ElapsedMilliseconds}ms");
    }


    #region A*
    void WalkPath_AStar(int[] start, int[] end)
    {
        m_end = end;

        // Init Path recorder
        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();

        // Init G, H
        for (int i = 0; i < widthCount; ++i)
        {
            G[i] = new Dictionary<int, int>();
            for (int j = 0; j < heightCount; ++j)
            {
                G[i][j] = int.MaxValue;
            }
        }

        // Start point
        G[start[0]][start[1]] = 0;
        openSet.Add(start);
        cameFrom[start] = null;

        // Find path
        //int cnt = 0, limit = 1000;
        while (openSet.Count > 0)
        {
            ////Debug.Log($"Open Set Count 1: {openSet.Count}");
            //if (++cnt > limit)
            //{
            //    //Debug.Log("here 1");
            //    break;
            //}

            int[] coord = openSet.Pop();
            //Debug.Log($"Open Set Count 2: {openSet.Count}");

            // Path Found
            if (coord[0] == end[0] && coord[1] == end[1])
            {
                Debug.Log("Path found :)");

                int[] stepCoord = coord;
                while (stepCoord != null)
                {
                    //if (++cnt > limit)
                    //{
                    //    //Debug.Log("here 2");
                    //    break;
                    //}
                    m_cubeMap[stepCoord[0]][stepCoord[1]].GetComponent<Renderer>().material.color = Color.red;
                    stepCoord = cameFrom[stepCoord];
                }

                return;
            }

            // Access neighbors
            foreach (var neighbor in GetNeighbors(coord))
            {
                if (neighbor != null && CanWalk(neighbor) && !IsAdded(neighbor))
                {
                    // Update G
                    var tmp = G[coord[0]][coord[1]] + GetRealDistance(coord, neighbor);
                    if (tmp < G[neighbor[0]][neighbor[1]])
                    {
                        G[neighbor[0]][neighbor[1]] = tmp;
                        cameFrom[neighbor] = coord;
                    }

                    // Add to Open Set
                    AddOpenSet(neighbor);
                    SetAdded(neighbor[0], neighbor[1]);
                }
            }
        }
        Debug.Log("Done, no path found :(");
    }

    List<int[]> GetNeighbors(int[] coord)
    {
        List<int[]> res = new List<int[]>();

        int x = coord[0], y = coord[1];

        bool isLeftBorder = x == 0;
        bool isTopBorder = y == heightCount - 1;
        bool isRightBorder = x == widthCount - 1;
        bool isBottomBorder = y == 0;

        // Straight
        if (!isLeftBorder)
            res.Add(new int[] { x - 1, y });
        if (!isTopBorder)
            res.Add(new int[] { x, y + 1 });
        if (!isRightBorder)
            res.Add(new int[] { x + 1, y });
        if (!isBottomBorder)
            res.Add(new int[] { x, y - 1 });

        // Diagonal
        if (!isLeftBorder && !isTopBorder)
            res.Add(new int[] { x - 1, y + 1 });
        if (!isRightBorder && !isTopBorder)
            res.Add(new int[] { x + 1, y + 1 });
        if (!isLeftBorder && !isBottomBorder)
            res.Add(new int[] { x - 1, y - 1 });
        if (!isRightBorder && !isBottomBorder)
            res.Add(new int[] { x + 1, y - 1 });

        return res;
    }

    void SetAdded(int x, int y)
    {
        if (!Added.ContainsKey(x))
            Added[x] = new Dictionary<int, bool>();
        Added[x][y] = true;
    }

    bool IsAdded(int[] coord)
    {
        int x = coord[0], y = coord[1];
        if (!Added.ContainsKey(x))
            return false;
        if (!Added[x].ContainsKey(y))
            return false;
        return Added[x][y];
    }

    void AddOpenSet(int[] coord)
    {
        if (openSet.Contains(coord))
            return;
        openSet.Add(coord);
    }

    static int GetH(int[] coord)
    {
        return GetH(coord[0], coord[1]);
    }

    static int GetH(int x, int y)
    {
        //return H[x][y];
        return GetManhattonDistance(new int[] { x, y }, m_end);
    }

    static int GetG(int[] coord)
    {
        return GetG(coord[0], coord[1]);
    }

    static int GetG(int x, int y)
    {
        return G[x][y];
    }

    static int GetManhattonDistance(int[] coordA, int[] coordB)
    {
        int deltaX = Math.Abs(coordA[0] - coordB[0]);
        int deltaY = Math.Abs(coordA[1] - coordB[1]);
        return deltaX + deltaY;
    }

    static int GetRealDistance(int[] coordA, int[] coordB)
    {
        // Assume A, B are neighbors.
        if (coordA[0] == coordB[0])
        {
            // Vertical straight move
            return Math.Abs(coordA[1] - coordB[1]);
        }
        else if (coordA[1] == coordB[1])
        {
            // Horizontal straight move
            return Math.Abs(coordA[0] - coordB[0]);
        }
        else
        {
            // Diagonal move, assume such case consist of 2 straight move
            return GetManhattonDistance(coordA, coordB);
        }
    }

    #endregion A*

    #region JPS

    Dictionary<int[], int[]> m_jumpFrom = new Dictionary<int[], int[]>();

    enum Dir
    {
        top,
        bottom,
        left,
        right,
        topLeft,
        topRight,
        bottomLeft,
        bottomRight,
        self
    }

    void WalkPath_JPS(int[] start, int[] end)
    {
        m_start = start;
        m_end = end;

        // Init Path recorder
        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();

        // Init G
        for (int i = 0; i < widthCount; ++i)
        {
            G[i] = new Dictionary<int, int>();
            for (int j = 0; j < heightCount; ++j)
            {
                G[i][j] = int.MaxValue;
            }
        }

        // Start point
        G[start[0]][start[1]] = 0;
        openSet.Add(start);
        cameFrom[start] = null;

        // Find path
        while (openSet.Count > 0)
        {
            int[] coord = openSet.Pop();

            // Path Found
            if (IsGoal(coord))
            {
                Debug.Log("Path found :)");

                int[] stepCoord = coord;
                while (stepCoord != null)
                {
                    m_cubeMap[stepCoord[0]][stepCoord[1]].GetComponent<Renderer>().material.color = Color.red;

                    //if (cameFrom[stepCoord] != null && !IsNeighbor(stepCoord, cameFrom[stepCoord]))
                    //{
                    //    Dir dir = GetDir(stepCoord, cameFrom[stepCoord]);

                    //    int[] tmp = Step(stepCoord, dir);

                    //    while (!IsSameCoord(tmp, cameFrom[stepCoord]))
                    //    {
                    //        m_cubeMap[tmp[0]][tmp[1]].GetComponent<Renderer>().material.color = Color.red;
                    //        tmp = Step(tmp, dir);
                    //    }
                    //}

                    stepCoord = cameFrom[stepCoord];
                }

                return;
            }

            // Access successors
            //TracePoint(coord, "Start");
            foreach (var successor in GetSuccessors(coord, GetDir(cameFrom[coord], coord)))
            {
                //TracePoint(successor, $"Try ? successor != null{successor != null} / CanWalk{CanWalk(successor)} / !IsAdded(successor){!IsAdded(successor)}");
                if (successor != null && CanWalk(successor) && !IsAdded(successor))
                {
                    //TracePoint(successor, "Access");

                    // Update G
                    var tmp = G[coord[0]][coord[1]] + GetRealDistance(coord, successor);
                    if (tmp < G[successor[0]][successor[1]])
                    {
                        G[successor[0]][successor[1]] = tmp;
                        cameFrom[successor] = coord;
                    }

                    // Add to Open Set
                    AddOpenSet(successor);
                    SetAdded(successor[0], successor[1]);
                }
            }
        }
        Debug.Log("Done, no path found :(");
    }

    void TracePoint(int[] coord, string prefix = "")
    {
        //Debug.Log($"{prefix} / Accessing coord: {coord[0]}, {coord[1]}");
    }

    List<int[]> GetSuccessors(int[] coord, Dir dir)
    {
        List<int[]> openSet = new List<int[]>();

        if (dir == Dir.self)
        {
            foreach (int[] neighbor in GetNeighbors(coord))
            {
                if (CanWalk(coord))
                {
                    int[] next = Jump(coord, GetDir(coord, neighbor));
                    if (next != null)
                    {
                        openSet.Add(next);
                    }
                }
            }
        }
        else
        {
            List<int[]> neighbors = new List<int[]>();
            foreach (int[] nNeighbor in GetNaturalNeighbors(coord, dir))
                neighbors.Add(nNeighbor);
            foreach (int[] fNeighbor in GetForceNeighbors(coord, dir))
                neighbors.Add(fNeighbor);
            foreach (int[] neighbor in neighbors)
            {
                int[] next = Jump(coord, GetDir(coord, neighbor));
                //TracePoint(neighbor, $"Got neighbor of {coord[0]},{coord[1]}");
                if (next != null)
                {
                    //TracePoint(neighbor, $"is add to openset");
                    openSet.Add(next);
                }
            }
        }

        return openSet;
    }

    List<int[]> GetNaturalNeighbors(int[] coord, Dir dir)
    {
        List<int[]> res = new List<int[]>();
        int[] neighbor;

        switch (dir)
        {
            case Dir.topLeft:
                neighbor = new int[] { coord[0] - 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] - 1, coord[1] + 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0], coord[1] + 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.topRight:
                neighbor = new int[] { coord[0], coord[1] + 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] + 1, coord[1] + 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] + 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.bottomLeft:
                neighbor = new int[] { coord[0] - 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] - 1, coord[1] - 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0], coord[1] - 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.bottomRight:
                neighbor = new int[] { coord[0], coord[1] - 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] + 1, coord[1] - 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                neighbor = new int[] { coord[0] + 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.top:
                neighbor = new int[] { coord[0], coord[1] + 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.right:
                neighbor = new int[] { coord[0] + 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.bottom:
                neighbor = new int[] { coord[0], coord[1] - 1 };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
            case Dir.left:
                neighbor = new int[] { coord[0] - 1, coord[1] };
                if (CanWalk(neighbor)) { res.Add(neighbor); }
                break;
        }

        return res;
    }

    List<int[]> GetForceNeighbors(int[] coord, Dir dir)
    {
        List<int[]> res = new List<int[]>();

        switch(dir)
        {
            // Diagonal
            case Dir.bottomRight:
                if (IsWall(GetNeighbor(coord, Dir.top)) && CanWalk(GetNeighbor(coord, Dir.topRight)))
                    res.Add(GetNeighbor(coord, Dir.topRight));
                if (IsWall(GetNeighbor(coord, Dir.left)) && CanWalk(GetNeighbor(coord, Dir.bottomLeft)))
                    res.Add(GetNeighbor(coord, Dir.bottomLeft));
                break;
            case Dir.bottomLeft:
                if (IsWall(GetNeighbor(coord, Dir.top)) && CanWalk(GetNeighbor(coord, Dir.topLeft)))
                    res.Add(GetNeighbor(coord, Dir.topLeft));
                if (IsWall(GetNeighbor(coord, Dir.right)) && CanWalk(GetNeighbor(coord, Dir.bottomRight)))
                    res.Add(GetNeighbor(coord, Dir.bottomRight));
                break;
            case Dir.topRight:
                if (IsWall(GetNeighbor(coord, Dir.bottom)) && CanWalk(GetNeighbor(coord, Dir.bottomRight)))
                    res.Add(GetNeighbor(coord, Dir.bottomRight));
                if (IsWall(GetNeighbor(coord, Dir.left)) && CanWalk(GetNeighbor(coord, Dir.topLeft)))
                    res.Add(GetNeighbor(coord, Dir.topLeft));
                break;
            case Dir.topLeft:
                if (IsWall(GetNeighbor(coord, Dir.bottom)) && CanWalk(GetNeighbor(coord, Dir.bottomLeft)))
                    res.Add(GetNeighbor(coord, Dir.bottomLeft));
                if (IsWall(GetNeighbor(coord, Dir.right)) && CanWalk(GetNeighbor(coord, Dir.topRight)))
                    res.Add(GetNeighbor(coord, Dir.topRight));
                break;
            // Straight
            case Dir.bottom:
                if (IsWall(GetNeighbor(coord, Dir.left)) && CanWalk(GetNeighbor(coord, Dir.bottomLeft)))
                    res.Add(GetNeighbor(coord, Dir.bottomLeft));
                if (IsWall(GetNeighbor(coord, Dir.right)) && CanWalk(GetNeighbor(coord, Dir.bottomRight)))
                    res.Add(GetNeighbor(coord, Dir.bottomRight));
                break;
            case Dir.left:
                if (IsWall(GetNeighbor(coord, Dir.top)) && CanWalk(GetNeighbor(coord, Dir.topLeft)))
                    res.Add(GetNeighbor(coord, Dir.topLeft));
                if (IsWall(GetNeighbor(coord, Dir.bottom)) && CanWalk(GetNeighbor(coord, Dir.bottomLeft)))
                    res.Add(GetNeighbor(coord, Dir.bottomLeft));
                break;
            case Dir.top:
                if (IsWall(GetNeighbor(coord, Dir.left)) && CanWalk(GetNeighbor(coord, Dir.topLeft)))
                    res.Add(GetNeighbor(coord, Dir.topLeft));
                if (IsWall(GetNeighbor(coord, Dir.right)) && CanWalk(GetNeighbor(coord, Dir.topRight)))
                    res.Add(GetNeighbor(coord, Dir.topRight));
                break;
            case Dir.right:
                if (IsWall(GetNeighbor(coord, Dir.bottom)) && CanWalk(GetNeighbor(coord, Dir.bottomRight)))
                    res.Add(GetNeighbor(coord, Dir.bottomRight));
                if (IsWall(GetNeighbor(coord, Dir.top)) && CanWalk(GetNeighbor(coord, Dir.topRight)))
                    res.Add(GetNeighbor(coord, Dir.topRight));
                break;
        }

        return res;
    }

    string GetDirName(Dir dir)
    {
        switch(dir)
        {
            case Dir.topLeft:
                return "topLeft";
            case Dir.top:
                return "top";
            case Dir.topRight:
                return "topRight";
            case Dir.left:
                return "left";
            case Dir.right:
                return "right";
            case Dir.bottomLeft:
                return "bottomLeft";
            case Dir.bottom:
                return "bottom";
            case Dir.bottomRight:
                return "bottomRight";
            default:
                return "self?";
        }
    }

    int[] Jump(int[] coord, Dir dir)
    {
        int[] next = Step(coord, dir);

        if (!CanWalk(next))
        {
            return null;
        }

        if (IsGoal(next))
        {
            //m_cubeMap[next[0]][next[1]].GetComponent<Renderer>().material.color = Color.yellow;
            return next;
        }

        if (HasForceNeighbor(next, dir))
        {
            //m_cubeMap[next[0]][next[1]].GetComponent<Renderer>().material.color = Color.yellow;
            return next;
        }

        if (IsDiagonal(dir))
        {
            Dir dComp_1, dComp_2;
            dComp_1 = GetDirComponent(dir, out dComp_2);

            int[] res_1 = Jump(next, dComp_1);
            if (res_1 != null)
            {
                //m_cubeMap[next[0]][next[1]].GetComponent<Renderer>().material.color = Color.yellow;
                return next;
            }
                
            int[] res_2 = Jump(next, dComp_2);
            if (res_2 != null)
            {
                //m_cubeMap[next[0]][next[1]].GetComponent<Renderer>().material.color = Color.yellow;
                return next;
            }
        }

        return Jump(next, dir);
    }

    int[] Step(int[] coord, Dir dir)
    {
        return GetNeighbor(coord, dir);
    }

    bool IsNeighbor(int[] coord1, int[] coord2)
    {
        if (coord1 == null || coord2 == null)
        {
            Debug.Log($"NULL happend between: {coord1} / {coord2}");
        }
        return Math.Abs(coord1[0] - coord2[0]) <= 1 && Math.Abs(coord1[1] - coord2[1]) <= 1;
    }

    bool IsSameCoord(int[] coord1, int[] coord2)
    {
        return coord1[0] == coord2[0] && coord1[1] == coord2[1];
    }

    bool HasForceNeighbor(int[] coord, Dir dir)
    {
        return GetForceNeighbors(coord, dir).Count > 0;
    }

    Dir GetDir(int[] from, int[] to)
    {
        if (from == null || to == null)
            return Dir.self;

        int diffX = to[0] - from[0];
        int diffY = to[1] - from[1];

        if (diffX < 0 && diffY > 0)
            return Dir.topLeft;
        else if (diffX == 0 && diffY > 0)
            return Dir.top;
        else if (diffX > 0 && diffY > 0)
            return Dir.topRight;
        else if (diffX > 0 && diffY == 0)
            return Dir.right;
        else if (diffX < 0 && diffY == 0)
            return Dir.left;
        else if (diffX < 0 && diffY < 0)
            return Dir.bottomLeft;
        else if (diffX == 0 && diffY < 0)
            return Dir.bottom;
        else if (diffX > 0 && diffY < 0)
            return Dir.bottomRight;
        else
            return Dir.self;
    }

    int[] GetNeighbor(int[] coord, Dir dir)
    {
        switch(dir)
        {
            case Dir.topLeft:
                return new int[] { coord[0] - 1, coord[1] + 1 };
            case Dir.top:
                return new int[] { coord[0], coord[1] + 1 };
            case Dir.topRight:
                return new int[] { coord[0] + 1, coord[1] + 1 };
            case Dir.left:
                return new int[] { coord[0] - 1, coord[1] };
            case Dir.right:
                return new int[] { coord[0] + 1, coord[1] };
            case Dir.bottomLeft:
                return new int[] { coord[0] - 1, coord[1] - 1 };
            case Dir.bottom:
                return new int[] { coord[0], coord[1] - 1 };
            case Dir.bottomRight:
                return new int[] { coord[0] + 1, coord[1] - 1 };
            default:
                return null;
        }
    }

    bool IsStart(int[] coord)
    {
        return coord[0] == m_start[0] && coord[1] == m_start[1];
    }

    bool IsGoal(int[] coord)
    {
        return coord[0] == m_end[0] && coord[1] == m_end[1];
    }

    bool IsDiagonal(Dir dir)
    {
        switch(dir)
        {
            case Dir.topLeft:
            case Dir.topRight:
            case Dir.bottomLeft:
            case Dir.bottomRight:
                return true;
        }
        return false;
    }

    Dir GetDirComponent(Dir dir, out Dir comp)
    {
        if (IsDiagonal(dir))
        {
            switch (dir)
            {
                case Dir.topLeft:
                    comp = Dir.top;
                    return Dir.left;
                case Dir.topRight:
                    comp = Dir.top;
                    return Dir.right;
                case Dir.bottomLeft:
                    comp = Dir.bottom;
                    return Dir.left;
                case Dir.bottomRight:
                    comp = Dir.bottom;
                    return Dir.right;
            }
        }
        comp = dir;
        return comp;
    }

    #endregion JPS

    #region 堆

    private class PFHeap
    {
        List<int[]> coordArr;
        public int Count
        {
            get
            {
                return coordArr.Count;
            }
        }

        public PFHeap()
        {
            coordArr = new List<int[]>();
        }

        public void Add(int[] coord)
        {
            coordArr.Add(coord);
            SwimUp();
        }

        public int[] Pop()
        {
            if (coordArr.Count <= 0) { return null; }
            
            int[] res = coordArr[0];
            int tail = coordArr.Count - 1;
            Swap(0, tail);
            coordArr.RemoveAt(tail);

            Heapify(0);
            
            return res;
        }

        public bool Contains(int[] coord)
        {
            foreach (var _coord in coordArr)
            {
                if (_coord[0] == coord[0] && _coord[1] == coord[1])
                {
                    return true;
                }
            }
            return false;
        }

        public void Adjust()
        {
            SwimUp();
        }

        #region private
        void SwimUp()
        {
            for (int i = (coordArr.Count - 2) / 2; i >= 0; --i)
            {
                Heapify(i);
            }
        }

        void Heapify(int index)
        {
            if (coordArr.Count <= 0)
                return;

            int rootVal = GetHByIndex(index);
            int[] childIndex = new int[] { 2 * index + 1, 2 * index + 2 };

            int greaterVal = rootVal;
            int greaterIndex = index;
            for (int i = 0; i < 2; ++i)
            {
                if (childIndex[i] < coordArr.Count)
                {
                    int val = GetHByIndex(childIndex[i]);
                    if (val > greaterVal)
                    {
                        greaterVal = val;
                        greaterIndex = childIndex[i];
                    }
                }
            }
            if (greaterIndex != index)
            {
                Swap(index, greaterIndex);
                Heapify(greaterIndex);
            }
        }

        int GetHByIndex(int index)
        {
            int[] coord = coordArr[index];
            return GetH(coord);
        }

        void Swap(int a, int b)
        {
            var tmp = coordArr[a];
            coordArr[a] = coordArr[b];
            coordArr[b] = tmp;
        }
        #endregion private
    }

    #endregion 堆


    #region 地图操作
    void GenerateWallMap()
    {
        if (random_seed == 0)
            random_seed = DateTime.Now.Millisecond;
        UnityEngine.Random.InitState(random_seed);

        m_wallMap = new bool[widthCount][];
        for (int i = 0; i < widthCount; ++i)
        {
            m_wallMap[i] = new bool[heightCount];
            for (int j = 0; j < heightCount; ++j)
            {
                if (i == 0 && j == 0)
                    m_wallMap[i][j] = false; // start point
                else if (i == widthCount-1 && j == heightCount-1)
                    m_wallMap[i][j] = false; // end point
                else
                    m_wallMap[i][j] = UnityEngine.Random.Range(0.0f, 100.0f) < blockPercent;
            }
        }
        m_wallMap[0][1] = false;
    }

    void GenerateBlocks()
    {
        int verticesCount = pMesh.vertices.Length;
        int side = (int)Mathf.Sqrt(verticesCount);

        Vector3[] vertices = new Vector3[4];

        vertices[0] = transform.TransformPoint(pMesh.vertices[0]);
        vertices[1] = transform.TransformPoint(pMesh.vertices[verticesCount / side - 1]);
        vertices[2] = transform.TransformPoint(pMesh.vertices[verticesCount - verticesCount / side]);
        vertices[3] = transform.TransformPoint(pMesh.vertices[verticesCount - 1]);

        float pWidth = (vertices[1] - vertices[0]).magnitude;
        float pHeight = (vertices[2] - vertices[0]).magnitude;

        float pWidthPerUnit = pWidth / widthCount;
        float pHeightPerUnit = pHeight / heightCount;

        m_cubeMap = new GameObject[widthCount][];
        for (int i = 0; i < widthCount; ++i)
            m_cubeMap[i] = new GameObject[heightCount];

        Vector3 rootPos = vertices[3];
        for (int i = 0; i < widthCount; ++i)
        {
            for (int j = 0; j < heightCount; ++j)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(transform);
                cube.transform.position = rootPos + new Vector3(i * pWidthPerUnit, j * pHeightPerUnit, 0);
                cube.transform.localScale = m_CubeScale;
                cube.GetComponent<Renderer>().material.color = CanWalk(i, j) ? Color.green : Color.black;
                cube.name = $"{i}_{j}";

                m_cubeMap[i][j] = cube;
            }
        }
    }

    bool CanWalk(int x, int y)
    {
        if (x < 0 || x >= widthCount)
            return false;
        if (y < 0 || y >= heightCount)
            return false;

        return !IsWall(x, y);
    }

    bool IsWall(int x, int y)
    {
        if (x < 0 || x >= widthCount)
            return false;
        if (y < 0 || y >= heightCount)
            return false;

        return m_wallMap[x][y];
    }

    bool CanWalk(int[] coord)
    {
        return CanWalk(coord[0], coord[1]);
    }

    bool IsWall(int[] coord)
    {
        return IsWall(coord[0], coord[1]);
    }

    #endregion 地图操作
}
