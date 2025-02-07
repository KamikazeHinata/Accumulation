public class CycleArray<T>
{
    private T[] m_arr;

    private int m_start;
    private int m_end;
    private int m_count;
    private int m_size;

    public CycleArray(int size)
    {
        m_arr = new T[size];
        m_start = 0;
        m_end = 0;
        m_count = 0;
        m_size = size;
    }

    public void AddHead(T value)
    {
        if (IsFull())
        {
            Resize(m_size * 2);
        }
        m_start = (m_start - 1 + m_size) % m_size;
        m_arr[m_start] = value;
        m_count++;
    }

    public void AddTail(T value)
    {
        if (IsFull())
        {
            Resize(m_size * 2);
        }
        m_arr[m_end] = value;
        m_end = (m_end + 1) % m_size;
        m_count++;
    }

    public void RemoveHead()
    {
        if (!IsEmpty())
        {
            m_start = (m_start + 1) % m_size;
            m_count--;
        }
    }

    public void RemoveTail()
    {
        if (!IsEmpty())
        {
            m_end = (m_end - 1 + m_size) % m_size;
            m_count--;
        }
    }

    public void Dump()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < m_count; ++i)
        {
            sb.Append($"{m_arr[(m_start + i) % m_size]}");
            if (i < m_count - 1)
            {
                sb.Append(" ");
            }
        }
        sb.Append($"] with size {m_size}");
        Debug.Log(sb);
    }

    private bool IsEmpty()
    {
        return m_count == 0;
    }

    private bool IsFull()
    {
        return m_count == m_size;
    }

    private void Resize(int newSize)
    {
        T[] newArr = new T[newSize];
        for (int i = 0; i < m_count; ++i)
        {
            newArr[i] = m_arr[(m_start + i) % m_size];
        }

        m_arr = newArr;
        m_size = newSize;
        m_start = 0;
        m_end = m_count;
    }
}


/***
Test code:
{
    var arr = new CycleArray<int>(5);
    arr.AddTail(3);
    arr.AddTail(4);
    arr.AddTail(5);

    arr.Dump(); //  [3 4 5] with size 5

    arr.AddHead(2);
    arr.AddHead(1);

    arr.Dump(); // [1 2 3 4 5] with size 5

    arr.AddHead(0);

    arr.Dump(); // [0 1 2 3 4 5] with size 10
}
***/

