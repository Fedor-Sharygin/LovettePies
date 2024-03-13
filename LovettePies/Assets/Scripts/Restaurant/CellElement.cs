using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellElement : MonoBehaviour
{
    private Area[] m_AreaArray;
    private int m_CurAreaIdx = 1;
    private Area m_CurArea;
    private Vector2Int m_CurCellPos = Vector2Int.zero;

    private Vector3 m_TargetPos;
    private void Start()
    {
        m_AreaArray = FindObjectsOfType<Area>();
        Array.Sort(m_AreaArray,
            Comparer<Area>.Create(
                (obj1, obj2) => obj1.transform.GetSiblingIndex().CompareTo(obj2.transform.GetSiblingIndex())
                ));

        GetCurArea();
        transform.position = m_CurArea.GetCenterOfCell(m_CurCellPos);
        m_TargetPos = transform.position;
    }

    private void Update()
    {
        GetToTargetPos();
    }

    public bool m_IsObject = false;
    public void MoveBy(Vector2Int p_MoveDir)
    {
        if (!AtTargetPos())
        {
            return;
        }

        Vector2Int TargetCellPos = m_CurCellPos + p_MoveDir;
        if (TargetCellPos.x < 0 || TargetCellPos.x >= m_CurArea.Rows)
        {
            return;
        }
        if (TargetCellPos.y < 0)
        {
            if (m_IsObject || TargetCellPos.x == m_CurArea.LeftConnection)
            {
                int CurAreaIdx = m_CurAreaIdx;
                m_CurAreaIdx--;
                GetCurArea();
                if (CurAreaIdx != m_CurAreaIdx)
                {
                    TargetCellPos.x = m_CurArea.RightConnection;
                    TargetCellPos.y = m_CurArea.Columns - 1;
                }
                else
                {
                    TargetCellPos.y = 0;
                }
            }
            else
            {
                return;
            }
        }
        if (TargetCellPos.y >= m_CurArea.Columns)
        {
            if (m_IsObject || TargetCellPos.x == m_CurArea.RightConnection)
            {
                int CurAreaIdx = m_CurAreaIdx;
                m_CurAreaIdx++;
                GetCurArea();
                if (CurAreaIdx != m_CurAreaIdx)
                {
                    TargetCellPos.x = m_CurArea.LeftConnection;
                    TargetCellPos.y = 0;
                }
                else
                {
                    TargetCellPos.y = m_CurArea.Columns - 1;
                }
            }
            else
            {
                return;
            }
        }

        m_CurCellPos = TargetCellPos;
        m_TargetPos = m_CurArea.GetCenterOfCell(m_CurCellPos);
    }

    private void GetCurArea()
    {
        if (m_AreaArray == null || m_AreaArray.Length == 0)
        {
            Debug.LogError($"{name}: Could not load and position the player. Quitting the game!");
            return;
        }

        if (m_CurAreaIdx < 0)
        {
            m_CurAreaIdx = 0;
        }
        if (m_CurAreaIdx >= m_AreaArray.Length)
        {
            m_CurAreaIdx = m_AreaArray.Length - 1;
        }

        m_CurArea = m_AreaArray[m_CurAreaIdx];
    }

    [SerializeField]
    private float m_DistEpsilon = .2f;
    private void GetToTargetPos()
    {
        if (AtTargetPos())
        {
            return;
        }

        float DistToTarget = Vector3.Distance(m_TargetPos, transform.position);
        float t = DistToTarget > m_DistEpsilon ? .024f : 1;
        transform.position = Vector3.Lerp(transform.position, m_TargetPos, t);
    }
    private bool AtTargetPos()
    {
        return Vector3.Distance(m_TargetPos, transform.position) < m_DistEpsilon;
    }
}
