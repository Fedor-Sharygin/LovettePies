using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCustomerScript : MonoBehaviour
{
    private CellElement m_CellPos;
    private void Start()
    {
        m_CellPos = GetComponent<CellElement>();
        m_StartingSquare = (m_CellPos.m_CurCellPos, m_CellPos.AreaIdx);
        StartCoroutine("FindPath");
    }

    [SerializeField]
    private Timer m_MovementTimer;
    [SerializeField]
    private Timer m_ActionTimer;

    private bool m_SearchingForPath = true;
    public void SearchForPath()
    {
        m_SearchingForPath = true;
    }

    private void Update()
    {
        if (!m_SearchingForPath)
        {
            return;
        }

        if (m_CurPath == null || m_CurPath.Count == 0)
        {
            StartCoroutine("FindPath");
        }
    }

    private List<(Vector2Int, int)> m_CurPath;
    private int m_PathIdx = 0;
    public Interactable.EnumInteractableType m_InteractableTargetType;
    private (Vector2Int, int) m_StartingSquare;
    private bool m_GoBack = false;
    public void FindPath()
    {
        while (CellElement.AreaArray.Length == 0) ;

        if (m_CurPath != null && m_CurPath.Count != 0)
        {
            return;
        }

        if (m_GoBack)
        {
            do
            {
                CellElement.AStar(
                    m_CellPos.AreaIdx, m_CellPos.m_CurCellPos,
                    m_StartingSquare.Item2, m_StartingSquare.Item1,
                    m_CellPos.m_MovementFlag,
                    out m_CurPath);
            } while (m_CurPath == null || m_CurPath.Count == 0);

            m_MovementTimer.ResetTimer();
            m_MovementTimer.Play();
            m_PathIdx = 0;
            m_SearchingForPath = false;
            return;
        }

        var TargetPlacement = CellElement.FindInteractableByType(m_InteractableTargetType, this.tag);

        if (TargetPlacement.Item1 == -1)
        {
            return;
        }

        do
        {
            CellElement.AStar(
                m_CellPos.AreaIdx, m_CellPos.m_CurCellPos,
                TargetPlacement.Item1, new Vector2Int(TargetPlacement.Item2.y, TargetPlacement.Item2.x),
                m_CellPos.m_MovementFlag,
                out m_CurPath);
        } while (m_CurPath == null || m_CurPath.Count == 0);

        m_MovementTimer.ResetTimer();
        m_MovementTimer.Play();
        m_PathIdx = 0;
        m_SearchingForPath = false;
    }
    public void MoveByPath()
    {
        if (!m_CellPos.AtTargetPos())
        {
            return;
        }

        m_PathIdx++;

        if (m_PathIdx >= m_CurPath.Count - 1)
        {
            if (!m_GoBack)
            {
                var PosAIdx = m_CurPath[m_PathIdx];
                CellElement.AreaArray[m_CellPos.AreaIdx].GetInteractable(PosAIdx.Item1).Interact(this);
                SitInArea();
            }
            else
            {
                m_CellPos.MoveTo(new Vector2Int(m_CurPath[m_PathIdx].Item1.y, m_CurPath[m_PathIdx].Item1.x));
            }
            m_CurPath.Clear();
            m_MovementTimer.Stop();
            return;
        }

        if (m_CurPath[m_PathIdx - 1].Item2 != m_CurPath[m_PathIdx].Item2)
        {
            if (m_CurPath[m_PathIdx - 1].Item1.x == 0 &&
                m_CurPath[m_PathIdx - 1].Item1.y == CellElement.AreaArray[m_CellPos.AreaIdx].LeftConnection)
            {
                m_CellPos.MoveBy(new Vector2Int(0, -1));
                return;
            }
            if (m_CurPath[m_PathIdx - 1].Item1.x == CellElement.AreaArray[m_CellPos.AreaIdx].Columns - 1 &&
                m_CurPath[m_PathIdx - 1].Item1.y == CellElement.AreaArray[m_CellPos.AreaIdx].RightConnection)
            {
                m_CellPos.MoveBy(new Vector2Int(0, 1));
                return;
            }
        }

        m_CellPos.MoveTo(new Vector2Int(m_CurPath[m_PathIdx].Item1.y, m_CurPath[m_PathIdx].Item1.x));
    }
    public void SitInArea()
    {
        m_ActionTimer.Play();
        m_CellPos.MoveTo(new Vector2Int(m_CurPath[m_PathIdx].Item1.y, m_CurPath[m_PathIdx].Item1.x));
        m_GoBack = true;
    }
}
