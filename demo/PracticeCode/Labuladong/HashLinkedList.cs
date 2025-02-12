public class HashLinkedList<K, V>
{
    Dictionary<K, Node<K, V>> dic;
    Node<K, V> m_head, m_tail;

    public HashLinkedList()
    {
        dic = new Dictionary<K, Node<K, V>>();
        m_head = null;
    }

    public void Put(K key, V value)
    {
        if (dic.ContainsKey(key))
        {
            Remove(key);
            Put(key, value);
        }
        else
        {
            var node = new Node<K, V>(key, value);
            dic[key] = node;

            if (m_head == null) 
            { 
                m_head = node;
                m_tail = node;
            }
            else
            {
                m_tail.next = node;
                node.pre = m_tail;
                m_tail = node;
            }
        }
    }

    public void Remove(K key)
    {
        if (dic.ContainsKey(key))
        {
            var node = dic[key];
            if (m_head.Equals(node))
            {
                m_head = node.next;
            }
            if (m_tail.Equals(node))
            {
                m_tail = node.pre;
            }

            node.pre.next = node.next;
            node.next.pre = node.pre;

            dic.Remove(key);
        }
    }

    public V Get(K key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key].value;
        }
        return default(V);
    }

    public K[] Keys()
    {
        var res = new List<K>();
        var head = m_head;
        while (head != null)
        {
            res.Add(head.key);
            head = head.next;
        }
        return res.ToArray<K>();
    }

    class Node<K, V>
    {
        public K key;
        public V value;
        public Node<K, V> pre;
        public Node<K, V> next;
        public Node(K key, V value)
        {
            this.key = key;
            this.value = value;
            this.pre = null;
            this.next = null;
        }
    }
}

/*
    var map = new HashLinkedList<string, string>();

    map.Put("3", "three");
    map.Put("4", "four");
    map.Put("5", "five");
    map.Put("1", "one");
    map.Put("2", "two");

    Debug.Log(map.Get("5")); // 5
    foreach (var key in map.Keys())
    {
        Debug.Log($"> {key}");
    }
    // 3 4 5 1 2

    map.Put("5", "Go");
    foreach (var key in map.Keys())
    {
        Debug.Log($"> {key}");
    }
    // 3 4 1 2 5
*/