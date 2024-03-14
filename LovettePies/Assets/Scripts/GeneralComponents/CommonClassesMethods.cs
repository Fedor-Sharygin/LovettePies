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
            if (p_Idx >= m_RowNum)
            {
                throw new System.IndexOutOfRangeException($"Tried to access matrix Row Out of Index: {p_Idx}/{m_RowNum}");
            }
        }
        private void AssertCols(int p_Idx)
        {
            if (p_Idx >= m_ColNum)
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

        public MyMatrix(int p_RowNum, int p_ColNum)
        {
            m_RowNum = p_RowNum;
            m_ColNum = p_ColNum;

            m_Elements = new T[m_RowNum, m_ColNum];

            for (int i = 0; i < m_RowNum; ++i)
            {
                for (int j = 0; j < m_ColNum; ++j)
                {
                    m_Elements[i, j] = default(T);
                }
            }
        }

        ~MyMatrix()
        {
            m_Elements = null;
        }
    }

    public static class ObjectExtension
    {
        public static T Clone<T>(T p_Object)
        {
            return JsonUtility.FromJson<T>(JsonUtility.ToJson(p_Object));
        }
    }

    public enum EnumMovementFlag
    {
        BLOCKS_MOVEMENT     = 1 << 0,
        BLOCKS_PLACEMENT    = 1 << 1,
        BLOCKS_CUSTOMERS    = 1 << 2,
        BLOCKS_FALL_THROUGH = 1 << 3,
    }
}