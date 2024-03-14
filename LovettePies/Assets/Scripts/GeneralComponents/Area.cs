using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


using ObjectMatrx = GlobalNamespace.MyMatrix<Interactable>;

public class Area : MonoBehaviour
{
    #region Complete Area Description

    public AreaDescriptor m_AreaDescription;
    public int m_AreaIdx;
    public int Columns
    {
        get
        {
            return !m_OverrideRow ? m_AreaDescription.ColumnCounts[m_AreaIdx] : m_ColumnCount;
        }
    }
    public int Rows
    {
        get
        {
            return !m_OverrideRow ? m_AreaDescription.RowCount : m_RowsCount;
        }
    }

    public int LeftConnection
    {
        get
        {
            if (m_OverrideRow)
            {
                return m_LeftRowConnection;
            }

            int LeftIdx = m_AreaIdx - 1;
            if (m_AreaDescription == null || LeftIdx < 0)
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
            if (m_OverrideRow)
            {
                return m_RightRowConnection;
            }

            if (m_AreaDescription == null || m_AreaIdx >= m_AreaDescription.ConnectionRows.Length)
            {
                return -100;
            }
            return m_AreaDescription.ConnectionRows[m_AreaIdx];
        }
    }

    #endregion


    private ObjectMatrx m_Interactables;
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

        m_CellSize.x = 2f * Size.x / Columns;
        m_CellSize.z = 2f * Size.z / Rows;

        m_Interactables = new ObjectMatrx(Rows, Columns);
    }



    public void AddObjectToMatrix(Interactable p_Object, int p_RowPos, int p_ColPos)
    {
        m_Interactables[p_RowPos, p_ColPos] = p_Object;
    }
    public void AddObjectToMatrix(Interactable p_Object, CellElement p_CellPos)
    {
        m_Interactables[p_CellPos.m_CurCellPos.y, p_CellPos.m_CurCellPos.x] = p_Object;
    }

    public void RemoveObjectFromMatrix(int p_RowPos, int p_ColPos)
    {
        m_Interactables[p_RowPos, p_ColPos] = null;
    }
    public void RemoveObjectFromMatrix(CellElement p_CellPos)
    {
        m_Interactables[p_CellPos.m_CurCellPos.y, p_CellPos.m_CurCellPos.x] = null;
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

    public bool Blocks(int p_RowIdx, int p_ColIdx, GlobalNamespace.EnumMovementFlag p_MovementFlag)
    {
        if (p_RowIdx < 0 || p_RowIdx >= m_Interactables.Rows ||
            p_ColIdx < 0 || p_ColIdx >= m_Interactables.Columns)
        {
            return false;
        }

        if (m_Interactables[p_RowIdx, p_ColIdx] == null)
        {
            return false;
        }

        return m_Interactables[p_RowIdx, p_ColIdx].Blocks(p_MovementFlag);
    }
    public bool Blocks(Vector2Int p_TargetCell, GlobalNamespace.EnumMovementFlag p_MovementFlag)
    {
        return Blocks(p_TargetCell.x, p_TargetCell.y, p_MovementFlag);
    }

    private static int[,] NeighborIndcs = new int[,] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
    public Interactable[] GetInteractables(int p_RowIdx, int p_ColIdx)
    {
        Interactable[] NeighborArray = new Interactable[4];

        for (int i = 0; i < 4; ++i)
        {
            int NRow = p_RowIdx + NeighborIndcs[i, 0];
            int NCol = p_ColIdx + NeighborIndcs[i, 1];

            if (NRow < 0 || NRow >= m_Interactables.Rows ||
                NCol < 0 || NCol >= m_Interactables.Columns)
            {
                continue;
            }

            NeighborArray[i] = m_Interactables[NRow, NCol];
        }
        return NeighborArray;
    }
    public Interactable[] GetInteractables(Vector2Int p_CellPos)
    {
        return GetInteractables(p_CellPos.y, p_CellPos.x);
    }
    
    
    public bool m_OverrideRow = false;

    public int m_RowsCount = -1;
    public int m_ColumnCount = -1;
    public int m_LeftRowConnection = -100;
    public int m_RightRowConnection = -100;
}


#region Custom Area Editor
#if UNITY_EDITOR

[CustomEditor(typeof(Area))]
class AreaEditor : Editor
{
    private Area TargetArea;
    private SerializedProperty m_AreaDescriptionProperty;
    private void OnEnable()
    {
        TargetArea = (Area)target;
        m_AreaDescriptionProperty = serializedObject.FindProperty("m_AreaDescription");
    }

    public override void OnInspectorGUI()
    {
        if (TargetArea == null)
        {
            return;
        }

        //DrawDefaultInspector();

        Undo.RecordObject(TargetArea, "Complete Area Description");

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_AreaDescriptionProperty, new GUIContent("Area Description"));
        serializedObject.ApplyModifiedProperties();

        TargetArea.m_AreaIdx = EditorGUILayout.IntField("Area Idx", TargetArea.m_AreaIdx);


        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Area Size Overrides", EditorStyles.boldLabel);
        TargetArea.m_OverrideRow        = EditorGUILayout.BeginToggleGroup("Override", TargetArea.m_OverrideRow);
        TargetArea.m_RowsCount          = EditorGUILayout.IntField("Rows Count", TargetArea.m_RowsCount);
        TargetArea.m_ColumnCount        = EditorGUILayout.IntField("Columns Count", TargetArea.m_ColumnCount);
        TargetArea.m_LeftRowConnection  = EditorGUILayout.IntField("Left Row Connection", TargetArea.m_LeftRowConnection);
        TargetArea.m_RightRowConnection = EditorGUILayout.IntField("Right Row Connection", TargetArea.m_RightRowConnection);
        EditorGUILayout.EndToggleGroup();

        /*
        Rect AreaDescriptorDropBox = GUILayoutUtility.GetRect(50f, 50f);
        GUI.Box(AreaDescriptorDropBox, "Area Description");

        Event CurEvent = Event.current;
        switch (CurEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                {
                    if (!AreaDescriptorDropBox.Contains(CurEvent.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (CurEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object DraggedObject in DragAndDrop.objectReferences)
                        {
                            AreaDescriptor DraggedAreaDescriptor = DraggedObject as AreaDescriptor;
                            if (DraggedAreaDescriptor == null)
                            {
                                continue;
                            }

                            TargetArea.m_AreaDescription = DraggedAreaDescriptor;
                            break;
                        }
                    }
                }
                break;
        }
        */
    }
}

#endif
#endregion
