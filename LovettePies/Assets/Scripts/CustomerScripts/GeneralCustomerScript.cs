using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralCustomerScript : MonoBehaviour
{
    private CellElement m_CellPos;
    private Interactable m_CustomerInteractable;
    private void Start()
    {
        m_CellPos = GetComponent<CellElement>();
        m_CustomerInteractable = GetComponent<Interactable>();
        m_StartingSquare = (m_CellPos.m_CurCellPos, /*m_CellPos.AreaIdx*/0);
        //StartCoroutine("FindPath");
    }

    [SerializeField]
    private Timer m_MovementTimer;
    [SerializeField]
    private Timer m_PatienceTimer;

    private bool m_SearchingForPath = false;
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
    public Interactable.EnumInteractableType[] m_InteractableTargetTypesOrder;
    private int m_TargetIdx = 0;
    private (Vector2Int, int) m_StartingSquare;
    public IEnumerator FindPath()
    {
        //while (Area.CurArea == null) ;

        if (m_CurPath != null && m_CurPath.Count != 0)
        {
            yield break;
        }

        if (m_TargetIdx >= m_InteractableTargetTypesOrder.Length)
        {
            while (true)
            {
                CellElement.AStar(
                    /*m_CellPos.AreaIdx*/ 0, m_CellPos.m_CurCellPos,
                    /*m_StartingSquare.Item2*/ 0, m_StartingSquare.Item1,
                    m_CellPos.m_MovementFlag,
                    out m_CurPath);

                if (m_CurPath != null && m_CurPath.Count != 0)
                {
                    m_MovementTimer.ResetTimer();
                    m_MovementTimer.Play();
                    m_PathIdx = 0;
                    m_SearchingForPath = false;
                    yield break;
                }

                yield return null;
            }
        }
        else
        {
            var TargetPlacement = CellElement.FindInteractableByType(m_InteractableTargetTypesOrder[m_TargetIdx], this.tag, m_CellPos.m_MovementFlag);

            if (TargetPlacement.Item1 == -1)
            {
                yield break;
            }

            while (true)
            {
                CellElement.AStar(
                    /*m_CellPos.AreaIdx*/ 0, m_CellPos.m_CurCellPos,
                    /*TargetPlacement.Item1*/ 0, new Vector2Int(TargetPlacement.Item2.y, TargetPlacement.Item2.x),
                    m_CellPos.m_MovementFlag,
                    out m_CurPath);

                if (m_CurPath != null && m_CurPath.Count != 0)
                {
                    m_CurPath.Add((TargetPlacement.Item3, TargetPlacement.Item1));

                    m_MovementTimer.ResetTimer();
                    m_MovementTimer.Play();
                    m_PathIdx = 0;
                    m_SearchingForPath = false;
                    yield break;
                }

                yield return null;
            }
        }
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
            if (m_TargetIdx < m_InteractableTargetTypesOrder.Length)
            {
                SitInArea();
            }
            else
            {
                m_CellPos.MoveTo(new Vector2Int(m_CurPath[m_PathIdx].Item1.y, m_CurPath[m_PathIdx].Item1.x));
                m_CustomerInteractable?.MoveToPosition();
                Destroy(gameObject);
            }
            m_CurPath.Clear();
            m_MovementTimer.Stop();
            return;
        }

        //THIS IS A LEFT-OVER FROM WHEN THERE WERE MULTIPLE AREAS
        //ON A SINGLE MAP => IS NOT REQUIRED AND SHOULD BE REMOVED
        //if (m_CurPath[m_PathIdx - 1].Item2 != m_CurPath[m_PathIdx].Item2)
        //{
        //    if (m_CurPath[m_PathIdx - 1].Item1.x == 0 &&
        //        m_CurPath[m_PathIdx - 1].Item1.y == CellElement.AreaArray[m_CellPos.AreaIdx].LeftConnection)
        //    {
        //        m_CellPos.MoveBy(new Vector2Int(0, -1));
        //        m_CustomerInteractable?.MoveToPosition();
        //        return;
        //    }
        //    if (m_CurPath[m_PathIdx - 1].Item1.x == CellElement.AreaArray[m_CellPos.AreaIdx].Columns - 1 &&
        //        m_CurPath[m_PathIdx - 1].Item1.y == CellElement.AreaArray[m_CellPos.AreaIdx].RightConnection)
        //    {
        //        m_CellPos.MoveBy(new Vector2Int(0, 1));
        //        m_CustomerInteractable?.MoveToPosition();
        //        return;
        //    }
        //}

        m_CellPos.MoveTo(new Vector2Int(m_CurPath[m_PathIdx].Item1.y, m_CurPath[m_PathIdx].Item1.x));

        m_CustomerInteractable?.MoveToPosition();
    }

    private int m_InteractionDirection;
    private Interactable m_CurrentInteractableSeat = null;
    public void LeaveArea()
    {
        m_CurrentInteractableSeat.Interact(this, m_InteractionDirection);
        m_CurrentInteractableSeat.FreeDirection(m_InteractionDirection);
    }

    public UnityEvent m_PatienceActions;
    public void SitInArea()
    {
        var PosAIdx = m_CurPath[m_PathIdx];
        m_InteractionDirection = GlobalNamespace.GeneralFunctions.GetDirection(m_CellPos.m_CurCellPos, new Vector2Int(PosAIdx.Item1.y, PosAIdx.Item1.x));
        m_CurrentInteractableSeat = Area.CurArea.GetInteractable(PosAIdx.Item1);
        m_CurrentInteractableSeat.Interact(this, m_InteractionDirection);
        m_PatienceTimer?.Play();
        m_TargetIdx++;

        m_PatienceActions?.Invoke();
    }
}
