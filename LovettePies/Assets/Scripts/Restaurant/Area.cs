using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField]
    private AreaDescriptor m_AreaDescription;
    [SerializeField]
    private int m_AreaIdx;

    public int Columns
    {
        get
        {
            return m_AreaDescription.ColumnCounts[m_AreaIdx];
        }
    }
    public int Rows
    {
        get
        {
            return m_AreaDescription.RowCount;
        }
    }


    public int LeftConnection
    {
        get
        {
            int LeftIdx = m_AreaIdx - 1;
            if (LeftIdx < 0)
            {
                return -100;
            }
            return m_AreaDescription.ConnectionRows[LeftIdx];
        }
    }
    public int RightConnection
    {
        get
        {
            if (m_AreaIdx >= m_AreaDescription.ConnectionRows.Length)
            {
                return -100;
            }
            return m_AreaDescription.ConnectionRows[m_AreaIdx];
        }
    }

    private Vector3 m_AreaStartPos = Vector3.zero;
    private Vector3 m_CellSize = Vector3.zero;
    private void Awake()
    {
        var SizeHolder = GetComponent<Renderer>();
        if (!SizeHolder)
        {
            return;
        }

        Vector3 Size = SizeHolder.bounds.extents;
        Size.y = 0;
        m_AreaStartPos = transform.position;
        m_AreaStartPos.x -= Size.x;
        m_AreaStartPos.z += Size.z;

        m_CellSize.x = 2f * Size.x / m_AreaDescription.ColumnCounts[m_AreaIdx];
        m_CellSize.z = 2f * Size.z / m_AreaDescription.RowCount;
    }

    public Vector3 GetCenterOfCell(int p_RowIdx, int p_ColIdx)
    {
        Vector3 CellCenterPos = m_AreaStartPos;
        CellCenterPos.x += m_CellSize.x * p_ColIdx;
        CellCenterPos.z -= m_CellSize.z * p_RowIdx;

        CellCenterPos.x += m_CellSize.x * .5f;
        CellCenterPos.z -= m_CellSize.z * .5f;
        return CellCenterPos;
    }
    public Vector3 GetCenterOfCell(Vector2Int p_CellPos)
    {
        return GetCenterOfCell(p_CellPos.x, p_CellPos.y);
    }
}
