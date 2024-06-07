using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.QuickSearch;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SLPathFinding : MonoBehaviour
{
    public int widthCount = 150;
    public int heightCount = 150;

    public float blockPercent = 10;

    Mesh pMesh;
    public Vector3 m_CubeScale = new Vector3(0.5f, 0.5f, 1);

    GameObject[][] m_cubeMap;
    bool[][] m_wallMap;

    static Dictionary<int, Dictionary<int, int>> G = new Dictionary<int, Dictionary<int, int>>();
    static Dictionary<int, Dictionary<int, bool>> Added = new Dictionary<int, Dictionary<int, bool>>();

    PFHeap openSet = new PFHeap();
    static int[] m_end;

    private void Awake()
    {
        pMesh = transform.GetComponent<MeshFilter>().mesh;
        GenerateWallMap();
        GenerateBlocks();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

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

    void WalkPath_JPS(int[] start, int[] end)
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
            int[] coord = openSet.Pop();

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

    List<int[]> GetPrunedNeighbors(int[] coord)
    {
        List<int[]> res = new List<int[]>();
        foreach (var neighbor in GetNeighbors(coord))
        {
            // Code here...
        }

        return res;
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
        for (int i = 0; i < heightCount; ++i)
        {
            for (int j = 0; j < widthCount; ++j)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(transform);
                cube.transform.position = rootPos + new Vector3(j * pWidthPerUnit, i * pHeightPerUnit, 0);
                cube.transform.localScale = m_CubeScale;
                cube.GetComponent<Renderer>().material.color = CanWalk(i, j) ? Color.green : Color.black;

                m_cubeMap[i][j] = cube;
            }
        }
    }

    bool CanWalk(int x, int y)
    {
        return !m_wallMap[x][y];
    }

    bool CanWalk(int[] coord)
    {
        return CanWalk(coord[0], coord[1]);
    }

    #endregion 地图操作
}
