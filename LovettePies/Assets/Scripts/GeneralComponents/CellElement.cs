using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
public class CellElement : MonoBehaviour
{
    #region Static Area Methods
    public static Area[] AreaArray;
    private static void GetAreaArray()
    {
        if (AreaArray == null || AreaArray.Length == 0)
        {
            AreaArray = FindObjectsOfType<Area>();
            Array.Sort(AreaArray,
                Comparer<Area>.Create(
                    (obj1, obj2) => obj1.transform.GetSiblingIndex().CompareTo(obj2.transform.GetSiblingIndex())
                    ));
        }
    }
    private static Area GetAreaAtIdx(int p_AreaIdx)
    {
        GetAreaArray();

        if (p_AreaIdx < 0)
        {
            p_AreaIdx = 0;
        }
        if (p_AreaIdx >= AreaArray.Length)
        {
            p_AreaIdx = AreaArray.Length - 1;
        }

        return AreaArray[p_AreaIdx];
    }

    public static void AStar(int p_StartAreaIdx, Vector2Int p_StartCell, int p_EndAreaIdx, Vector2Int p_EndElement, out Vector2Int[] OutPath)
    {
        GetAreaArray();

        GetAreaAtIdx(p_StartAreaIdx).AStar(p_StartCell, p_EndElement, out OutPath);

        //TO-DO:
        //Write an appropriate AStar pathfinding algorithm
    }
    #endregion


    private int m_CurAreaIdx = 1;
    private Area m_CurArea;
    public Vector2Int m_CurCellPos { get; private set; } = Vector2Int.zero;

    private Vector3 m_TargetPos;
    private void Start()
    {
        GetAreaArray();
        GetCurArea();
        transform.position = m_CurArea.GetCenterOfCell(m_CurCellPos);
        m_TargetPos = transform.position;
    }

    private void Update()
    {
        GetToTargetPos();
    }

    #region Movement
    [HideInInspector]
    public GlobalNamespace.EnumMovementFlag m_MovementFlag;
    public void MoveBy(Vector2Int p_MoveDir)
    {
        if (!AtTargetPos())
        {
            return;
        }
        GetAreaArray();
        GetCurArea();

        Vector2Int TargetCellPos = m_CurCellPos + p_MoveDir;
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
            if (TargetCellPos.x == m_CurArea.RightConnection)
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

        if (m_CurArea.Blocks(TargetCellPos, m_MovementFlag))
        {
            return;
        }

        m_CurCellPos = TargetCellPos;
        m_TargetPos = m_CurArea.GetCenterOfCell(m_CurCellPos);
    }

    private void GetCurArea()
    {
        m_CurArea = GetAreaAtIdx(m_CurAreaIdx);
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
    #endregion


    public Interactable[] GetNeighbors()
    {
        GetAreaArray();
        GetCurArea();

        return m_CurArea.GetInteractables(m_CurCellPos);
    }
}

#region Custom Cell Element Editor
#if UNITY_EDITOR

[CustomEditor(typeof(CellElement))]
class CellElementEditor : Editor
{
    private CellElement TargetCell;
    private SerializedProperty m_MovementFlagProperty;
    private void OnEnable()
    {
        TargetCell = (CellElement)target;
        m_MovementFlagProperty = serializedObject.FindProperty("m_MovementFlag");
    }

    public override void OnInspectorGUI()
    {
        if (TargetCell == null)
        {
            return;
        }

        Undo.RecordObject(TargetCell, "Interactable Object");

        TargetCell.m_MovementFlag = (GlobalNamespace.EnumMovementFlag)EditorGUILayout.EnumFlagsField("Movement Mask", TargetCell.m_MovementFlag);

        EditorGUILayout.Space(5);
        DrawDefaultInspector();
    }
}

#endif
#endregion
