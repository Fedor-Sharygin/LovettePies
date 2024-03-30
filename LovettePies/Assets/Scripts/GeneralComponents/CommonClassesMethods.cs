using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalNamespace
{
    internal class MyMatrix<T>
    {
        private T[,] m_Elements;
        private int m_RowNum;
        private int m_ColNum;

        public int Rows
        {
            get
            {
                return m_RowNum;
            }
        }
        public int Columns
        {
            get
            {
                return m_ColNum;
            }
        }

        #region Asserts
        private void AssertRows(int p_Idx)
        {
            if (p_Idx < 0 || p_Idx >= m_RowNum)
            {
                throw new System.IndexOutOfRangeException($"Tried to access matrix Row Out of Index: {p_Idx}/{m_RowNum}");
            }
        }
        private void AssertCols(int p_Idx)
        {
            if (p_Idx < 0 || p_Idx >= m_ColNum)
            {
                throw new System.IndexOutOfRangeException($"Tried to access matrix Column Out of Index: {p_Idx}/{m_ColNum}");
            }
        }
        #endregion

        #region Indexers
        public T[] this[int p_Idx]
        {
            get
            {
                AssertRows(p_Idx);

                T[] RetArray = new T[m_ColNum];
                for (int i = 0; i < m_ColNum; ++i)
                {
                    RetArray[i] = m_Elements[p_Idx, i];
                }
                return RetArray;
            }

            set
            {
                AssertRows(p_Idx);

                if (value.Length != m_ColNum)
                {
                    throw new System.Exception($"Untyped Excpetion: Provided Array has different length than the Matrix Row: {value.Length}/{m_ColNum}");
                }

                for (int i = 0; i < m_ColNum; ++i)
                {
                    m_Elements[p_Idx, m_ColNum] = value[i];
                }
            }
        }

        public T this[int p_RowIdx, int p_ColIdx]
        {
            get
            {
                AssertRows(p_RowIdx);
                AssertCols(p_ColIdx);

                return m_Elements[p_RowIdx, p_ColIdx];
            }
            
            set
            {
                AssertRows(p_RowIdx);
                AssertCols(p_ColIdx);

                m_Elements[p_RowIdx, p_ColIdx] = value;
            }
        }
        #endregion

        public MyMatrix(int p_RowNum, int p_ColNum, T p_DefaultVal = default(T))
        {
            m_RowNum = p_RowNum;
            m_ColNum = p_ColNum;

            m_Elements = new T[m_RowNum, m_ColNum];

            for (int i = 0; i < m_RowNum; ++i)
            {
                for (int j = 0; j < m_ColNum; ++j)
                {
                    m_Elements[i, j] = p_DefaultVal;
                }
            }
        }

        ~MyMatrix()
        {
            m_Elements = null;
        }
    }

    public static class GeneralFunctions
    {
        public static T Clone<T>(T p_Object)
        {
            return JsonUtility.FromJson<T>(JsonUtility.ToJson(p_Object));
        }

        public static int GetDirection(Vector2 p_From, Vector2 p_To)
        {
            if (p_To.x == p_From.x)
            {
                return p_To.y - p_From.y > 0 ? 2 : 3;
            }
            else
            {
                return p_To.x - p_From.x > 0 ? 1 : 0;
            }
        }
        public static int GetDirection(Vector2 p_Direction)
        {
            if (p_Direction.x == 0)
            {
                return p_Direction.y > 0 ? 2 : 3;
            }
            else
            {
                return p_Direction.x > 0 ? 1 : 0;
            }
        }
    }

    public enum EnumMovementFlag
    {
        BLOCKS_MOVEMENT     = 1 << 0,
        BLOCKS_PLACEMENT    = 1 << 1,
        BLOCKS_CUSTOMERS    = 1 << 2,
        BLOCKS_FALL_THROUGH = 1 << 3,
        BLOCKS_PLAYER       = 1 << 4,
    }

    #region TO-DO: Custom Component names (Could buy a Unity extension and study it)
    public static class CustomComponentNames
    {
        public static List<string> m_ComponentCustomName = new List<string>();
        public static Dictionary<Component, int> m_ComponentIndcs = new Dictionary<Component, int>();
        private static int m_CurComponentSize = 0;

        public static void AddComponent(Component p_Component)
        {
            if (m_ComponentIndcs.ContainsKey(p_Component))
            {
                return;
            }

            m_ComponentIndcs[p_Component] = m_CurComponentSize;
            m_CurComponentSize++;
            m_ComponentCustomName.Add(p_Component.GetType().Name);
        }

        public static string GetComponentName(Component p_Component)
        {
            int CurIdx;
            if (!m_ComponentIndcs.TryGetValue(p_Component, out CurIdx))
            {
                AddComponent(p_Component);
                CurIdx = m_ComponentIndcs[p_Component];
            }

            return m_ComponentCustomName[CurIdx];
        }
    }
    #endregion

    public class TupleComparer<T1, T2> : IComparer<(T1, T2)>
    {
        private readonly IComparer<T1> m_Comparer1;
        private readonly IComparer<T2> m_Comparer2;

        public TupleComparer(IComparer<T1> p_Comparer1 = null, IComparer<T2> p_Comparer2 = null)
        {
            m_Comparer1 = p_Comparer1 ?? Comparer<T1>.Default;
            m_Comparer2 = p_Comparer2 ?? Comparer<T2>.Default;
        }

        public int Compare((T1, T2) p_Tuple1, (T1, T2) p_Tuple2)
        {
            if (m_Comparer1 == null && m_Comparer2 == null)
            {
                return 0;
            }

            if (m_Comparer1 == null)
            {
                return m_Comparer1.Compare(p_Tuple1.Item1, p_Tuple2.Item1);
            }

            return m_Comparer2.Compare(p_Tuple1.Item2, p_Tuple2.Item2);
        }
    }

    public class PriorityQ<T1, T2>
        where T1 : IComparable<T1>
    {
        private SortedDictionary<T1, Queue<T2>> m_PriorityQueue;

        public PriorityQ()
        {
            m_PriorityQueue = new SortedDictionary<T1, Queue<T2>>();
        }

        public void Add((T1, T2) p_Item)
        {
            if (!m_PriorityQueue.ContainsKey(p_Item.Item1))
            {
                m_PriorityQueue.Add(p_Item.Item1, new Queue<T2>());
            }
            m_PriorityQueue[p_Item.Item1].Enqueue(p_Item.Item2);
        }

        public T2 Top()
        {
            if (Count == 0)
            {
                throw new InvalidCastException($"Priority Queue{{{typeof(T1)}, {typeof(T2)}}}: Tried to get a top element from an empty priority queue");
            }


            foreach (var KeyVal in m_PriorityQueue)
            {
                if (KeyVal.Value.Count == 0)
                {
                    continue;
                }

                return KeyVal.Value.Peek();
            }
            throw new InvalidCastException($"Priority Queue{{{typeof(T1)}, {typeof(T2)}}}: Tried to get a top element from an empty priority queue");
        }
        public T2 Pop()
        {
            if (Count == 0)
            {
                throw new InvalidCastException($"Priority Queue{{{typeof(T1)}, {typeof(T2)}}}: Tried to get a top element from an empty priority queue");
            }


            foreach (var KeyVal in m_PriorityQueue)
            {
                if (KeyVal.Value.Count == 0)
                {
                    continue;
                }

                return KeyVal.Value.Dequeue();
            }
            throw new InvalidCastException($"Priority Queue{{{typeof(T1)}, {typeof(T2)}}}: Tried to get a top element from an empty priority queue");
        }

        public bool Empty
        {
            get
            {
                return Count == 0;
            }
        }
        public int Count
        {
            get
            {
                int CountVal = 0;
                foreach (var KeyVal in m_PriorityQueue)
                {
                    CountVal += KeyVal.Value.Count;
                }
                
                return CountVal;
            }
        }
    }
}