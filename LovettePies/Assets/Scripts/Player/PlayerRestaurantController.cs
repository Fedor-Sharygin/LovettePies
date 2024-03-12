using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRestaurantController : MonoBehaviour
{
    [SerializeField]
    private Area[] m_AreaArray;
    private int m_CurAreaIdx = 1;
    private Area m_CurArea;

    private PlayerInput m_PlayerInput;
    private Vector2Int m_CurCellPos = Vector2Int.zero;
    private void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        if (m_PlayerInput == null)
        {
            Debug.LogError($"{name}: Could not load Player Input Controls. Quitting the game!");
            Application.Quit();
            return;
        }
        GetCurArea();
        transform.position = m_CurArea.GetCenterOfCell(m_CurCellPos);
        m_TargetPos = transform.position;
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

    private Vector3 m_TargetPos;
    public void NavigateArea(InputAction.CallbackContext p_CallbackContext)
    {
        if (!AtTargetPos())
        {
            return;
        }

        if (!p_CallbackContext.performed)
        {
            return;
        }
        Vector2 MoveDirection = p_CallbackContext.ReadValue<Vector2>();
        MoveDirection.y *= -1;
        Vector2Int MoveDirInt = new Vector2Int((int)MoveDirection.y, (int)MoveDirection.x);

        Vector2Int TargetCellPos = m_CurCellPos + MoveDirInt;
        if (TargetCellPos.x < 0 || TargetCellPos.x >= m_CurArea.Rows)
        {
            return;
        }
        if (TargetCellPos.y < 0)
        {
            if (TargetCellPos.x == m_CurArea.LeftConnection)
            {
                int CurAreaIdx = m_CurAreaIdx;
                m_CurAreaIdx--;
                GetCurArea();
                if (CurAreaIdx != m_CurAreaIdx)
                {
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
            if (TargetCellPos.x == m_CurArea.RightConnection)
            {
                int CurAreaIdx = m_CurAreaIdx;
                m_CurAreaIdx++;
                GetCurArea();
                if (CurAreaIdx != m_CurAreaIdx)
                {
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

    private void Update()
    {
        GetToTargetPos();
    }

    [SerializeField]
    private float m_DistEpsilon = .05f;
    private void GetToTargetPos()
    {
        if (AtTargetPos())
        {
            return;
        }

        float DistToTarget = Vector3.Distance(m_TargetPos, transform.position);
        float t = DistToTarget > m_DistEpsilon ? .03f : 1;
        transform.position = Vector3.Lerp(transform.position, m_TargetPos, t);
    }
    private bool AtTargetPos()
    {
        return Vector3.Distance(m_TargetPos, transform.position) < m_DistEpsilon;
    }
}
