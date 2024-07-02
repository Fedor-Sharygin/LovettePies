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
        AreaManager AM = FindObjectOfType<AreaManager>();
        if (AM != null)
        {
            AreaArray = AM.m_Areas;
            return;
        }

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

    public static void AStar(int p_StartAreaIdx, Vector2Int p_StartCell, int p_EndAreaIdx, Vector2Int p_EndCell, GlobalNamespace.EnumMovementFlag p_MovementFlag, out List<(Vector2Int, int)> OutPath)
    {
        GetAreaArray();

        OutPath = new List<(Vector2Int, int)>();
        p_StartCell = new Vector2Int(p_StartCell.y, p_StartCell.x);
        p_EndCell = new Vector2Int(p_EndCell.y, p_EndCell.x);

        int AreaDir = (int)Mathf.Sign(p_EndAreaIdx - p_StartAreaIdx);
        var CurArea = GetAreaAtIdx(p_StartAreaIdx);
        List<(Vector2Int, int)> PartialPath;
        for (int AreaIdx = p_StartAreaIdx; AreaIdx != p_EndAreaIdx; AreaIdx += AreaDir)
        {
            var CurTargetCell = AreaDir == 1 ? new Vector2Int(CurArea.Columns - 1, CurArea.RightConnection) : new Vector2Int(0, CurArea.LeftConnection);
            CurArea.AStar(p_StartCell, CurTargetCell, p_MovementFlag, out PartialPath);
            OutPath.AddRange(PartialPath);
            var NextArea = GetAreaAtIdx(AreaIdx + AreaDir);
            p_StartCell = AreaDir == 1 ? new Vector2Int(0, NextArea.LeftConnection) : new Vector2Int(NextArea.Columns - 1, NextArea.RightConnection);
            PartialPath.Clear();
            CurArea = NextArea;
        }
        CurArea.AStar(p_StartCell, p_EndCell, p_MovementFlag, out PartialPath);
        OutPath.AddRange(PartialPath);
    }

    public static (int, Vector2Int, Vector2Int) FindInteractableByType(Interactable.EnumInteractableType p_Type, string p_ObjectTag, GlobalNamespace.EnumMovementFlag p_MovementFlag)
    {
        GetAreaArray();

        int RetIdx = -1;
        Vector2Int RetCell = -Vector2Int.one;
        Vector2Int EntryCell = -Vector2Int.one;
        for (int i = 0; i < AreaArray.Length; ++i)
        {
            var CurCheck = AreaArray[i].FindInteractableByType(p_Type, p_ObjectTag, p_MovementFlag);
            if (CurCheck.Item2 == RetCell)
            {
                continue;
            }

            EntryCell = CurCheck.Item1;
            RetCell = CurCheck.Item2;
            RetIdx = i;
            break;
        }

        return (RetIdx, EntryCell, RetCell);
    }
    #endregion


    private int m_CurAreaIdx = 0;
    public int AreaIdx
    {
        get
        {
            return m_CurAreaIdx;
        }
        set
        {
            m_CurAreaIdx = value;
        }
    }
    private Area m_CurArea;
    public Vector2Int m_CurCellPos { get; private set; } = Vector2Int.zero;
    public void OverrideCurCellPos(Vector2Int p_OverridePos)
    {
        m_CurCellPos = p_OverridePos;
    }

    private Vector3 m_TargetPos;
    private void Awake()
    {
        GetAreaArray();
        GetCurArea();
        transform.position = m_CurArea.GetCenterOfCell(m_CurCellPos);
        m_TargetPos = transform.position;
    }

    public bool m_PositionOverhaul = false;
    private void Update()
    {
        if (m_PositionOverhaul)
        {
            return;
        }

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
    public void MoveTo(Vector2Int p_TargetCell)
    {
        MoveBy(p_TargetCell - m_CurCellPos);
    }

    private void GetCurArea()
    {
        m_CurArea = GetAreaAtIdx(m_CurAreaIdx);
    }

    [SerializeField]
    private float m_DistEpsilon = .2f;
    [SerializeField]
    private float m_MovementSpeed = 5f;
    private void GetToTargetPos()
    {
        if (AtTargetPos())
        {
            return;
        }

        float DistToTarget = Vector3.Distance(m_TargetPos, transform.position);
        float t = DistToTarget > m_DistEpsilon ? m_MovementSpeed * Mathf.Min(Time.deltaTime, .016f) : 1;
        transform.position = Vector3.Lerp(transform.position, m_TargetPos, t);
    }
    public bool AtTargetPos()
    {
        return Vector3.Distance(m_TargetPos, transform.position) <= m_DistEpsilon;
    }
    #endregion


    public Interactable[] GetNeighbors()
    {
        GetAreaArray();
        GetCurArea();

        return m_CurArea.GetInteractables(m_CurCellPos);
    }

    public Interactable GetNeighbor(Vector2Int p_Direction)
    {
        GetAreaArray();
        GetCurArea();

        var NeighborPos = m_CurCellPos + p_Direction;
        NeighborPos = new Vector2Int(NeighborPos.y, NeighborPos.x);
        return m_CurArea.GetInteractable(NeighborPos);
    }
}

#region Custom Cell Element Editor
#if UNITY_EDITOR

[CustomEditor(typeof(CellElement))]
[CanEditMultipleObjects]
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
