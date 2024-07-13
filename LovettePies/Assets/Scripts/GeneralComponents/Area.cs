using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


using ObjectMatrx = GlobalNamespace.MyMatrix<Interactable>;

public class Area : MonoBehaviour
{
    #region Static Methods
    public static string CurAreaName { get; private set; } = string.Empty;
    public static Area CurArea { get; private set; } = null;

    public static AreaData GetCurAreaDescription()
    {
        return CurArea?.GetAreaDescription();
    }


    //THIS IS A HORRIBLE REPRESENTATION AND NEEDS TO BE CHANGED
    //SHOULD WORK FOR NOW
    //  \/\/\/\/\/\/\/\/\/\/\/
    [System.Serializable]
    public struct TypeObject
    {
        public Interactable.EnumInteractableType Type;
        public GameObject Prefab;
    }
    public static TypeObject[] InteractablePrefabList;
    private static bool m_IPLSetup = false;
    [SerializeField]
    private TypeObject[] m_LocalInteractablePrefabList;

    [System.Serializable]
    public struct TypeIngredient
    {
        public IngredientType Type;
        public IngredientDescriptor IngredientDescriptor;
    }
    public static TypeIngredient[] IngredientDescriptorList;
    [SerializeField]
    private TypeIngredient[] m_LocalIngredientDescriptorList;
    private void StupidAwake()
    {
        if (m_IPLSetup)
        {
            return;
        }

        InteractablePrefabList = new TypeObject[m_LocalInteractablePrefabList.Length];
        IngredientDescriptorList = new TypeIngredient[m_LocalIngredientDescriptorList.Length];
        m_LocalInteractablePrefabList.CopyTo(InteractablePrefabList, 0);
        m_LocalIngredientDescriptorList.CopyTo(IngredientDescriptorList, 0);
        m_IPLSetup = true;
    }
    public static GameObject GetPrefabByInteractableType(Interactable.EnumInteractableType p_InteractableType)
    {
        foreach (var TO in InteractablePrefabList)
        {
            if (TO.Type != p_InteractableType)
            {
                continue;
            }
            return TO.Prefab;
        }
        return null;
    }
    public static IngredientDescriptor GetIngredientByType(IngredientType p_IngredientType)
    {
        foreach (var TI in IngredientDescriptorList)
        {
            if (TI.Type != p_IngredientType)
            {
                continue;
            }
            return TI.IngredientDescriptor;
        }
        return null;
    }

    public static GameObject ConstructObjectFromDescription(AreaData.AreaObject p_Description, bool p_MarkedAsNew = true)
    {
        //THIS POSITION IS BLOCKED => CANNOT CONSTRUCT ANYTHING
        if (Area.CurArea.Blocks(p_Description.PositionX, p_Description.PositionY, (GlobalNamespace.EnumMovementFlag)int.MaxValue))
        {
            return null;
        }

        var NewElem = GameObject.Instantiate(Area.GetPrefabByInteractableType(p_Description.ObjectType));
        if (NewElem == null)
        {
            return null;
        }

        var CellComp = NewElem.GetComponent<CellElement>();
        var InterComp = NewElem.GetComponent<Interactable>();
        
        InterComp.m_StartPos = new Vector2Int(p_Description.PositionY, p_Description.PositionX);
        InterComp.m_BlockFlag = p_Description.BlockingMask;
        InterComp.m_NewElement = p_MarkedAsNew;


        //This area is for utilizing specific Extra Parameters if present
        //  \/\/\/\/\/\/\/\/\/\/\/

        if (InterComp is IngridientHolder && p_Description.ExtraParams.Length > 0)
        {
            int ParamVal = System.Convert.ToInt32(p_Description.ExtraParams[0]);
            //int.TryParse((string)p_Description.ExtraParams[0], out ParamVal);
            var ParamEnumVal = (IngredientType)ParamVal;

            (InterComp as IngridientHolder).m_IngredientDescription = Area.GetIngredientByType(ParamEnumVal);
        }

        if (InterComp is PlateHolder && p_Description.ExtraParams.Length > 0)
        {
            (InterComp as PlateHolder).m_StartFull = (bool)p_Description.ExtraParams[0];
        }

        //  /\/\/\/\/\/\/\/\/\/\/\

        CellComp.m_ModelHiddenByDefault = p_Description.ModelHidden;

        return NewElem;
    }

    public static AreaData.AreaObject ConstructDescriptionFromObject(Interactable p_Object, int p_X, int p_Y)
    {
        var AreaObj = new AreaData.AreaObject
        {
            PositionX = p_X,
            PositionY = p_Y,
            ObjectType = p_Object.InteractableType,
            BlockingMask = p_Object.m_BlockFlag,
            ModelHidden = p_Object.GetComponent<CellElement>().m_ModelHiddenByDefault,
        };

        if (p_Object is IngridientHolder)
        {
            AreaObj.ExtraParams = new object[1];
            AreaObj.ExtraParams[0] = (p_Object as IngridientHolder).m_IngredientDescription.m_IngredientType;
        }

        if (p_Object is PlateHolder)
        {
            AreaObj.ExtraParams = new object[1];
            AreaObj.ExtraParams[0] = (p_Object as PlateHolder).m_StartFull;
        }

        return AreaObj;
    }
    //  /\/\/\/\/\/\/\/\/\/\/\


    #endregion

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

    public string m_AreaName;
    private ObjectMatrx m_Interactables;
    private Vector3 m_AreaStartPos = Vector3.zero;
    private Vector3 m_CellSize = Vector3.zero;
    private Vector3 m_RightDir;
    private Vector3 m_ForwardDir;
    private void Awake()
    {
        StupidAwake(); //THIS NEEEEEEDS TO GO AWAY!!!!

        CellGridSetup();
        ObjectsSetup();
    }
    private void CellGridSetup()
    {
        var SizeHolder = GetComponent<Renderer>();
        if (!SizeHolder)
        {
            return;
        }

        Vector3 Size = SizeHolder.bounds.extents;
        Size.y = 0;
        m_AreaStartPos = transform.position;
        m_AreaStartPos += transform.forward * Size.z;
        m_AreaStartPos -= transform.right * Size.x;

        m_CellSize.x = 2f * Size.x / Columns;
        m_CellSize.z = 2f * Size.z / Rows;

        m_Interactables = new ObjectMatrx(Rows, Columns);

        m_RightDir = transform.right;
        m_ForwardDir = -transform.forward;
    }
    private void ObjectsSetup()
    {
        Area.CurAreaName = m_AreaName;
        Area.CurArea = this;

        AreaData LoadValues = SaveLoadSystem.Manager.GetAreaDescription(Area.CurAreaName);

        if (LoadValues == null)
        {
            return;
        }

        foreach (var Obj in LoadValues.Objects)
        {
            Area.ConstructObjectFromDescription(Obj, false);
        }
    }

    public Vector3 GetCenterOfCell(int p_RowIdx, int p_ColIdx)
    {
        Vector3 CellCenterPos = m_AreaStartPos;
        CellCenterPos += m_CellSize.x * ((float)p_ColIdx + .5f) * m_RightDir;
        CellCenterPos += m_CellSize.z * ((float)p_RowIdx + .5f) * m_ForwardDir;
        return CellCenterPos;
    }
    public Vector3 GetCenterOfCell(Vector2Int p_CellPos)
    {
        return GetCenterOfCell(p_CellPos.x, p_CellPos.y);
    }



    #region Interactable Matrix methods
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
        m_Interactables[p_CellPos.m_CurCellPos.x, p_CellPos.m_CurCellPos.y] = null;
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
        return GetInteractables(p_CellPos.x, p_CellPos.y);
    }
    public Interactable GetInteractable(int p_RowIdx, int p_ColIdx)
    {
        return m_Interactables[p_RowIdx, p_ColIdx];
    }
    public Interactable GetInteractable(Vector2Int p_CellPos)
    {
        return m_Interactables[p_CellPos.y, p_CellPos.x];
    }

    //       entry pos, target pos
    public (Vector2Int, Vector2Int) FindInteractableByType(Interactable.EnumInteractableType p_Type, string p_ObjectTag, GlobalNamespace.EnumMovementFlag p_MovementFlag)
    {
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                if (m_Interactables[i, j] == null || !m_Interactables[i, j].IsInteractable(p_ObjectTag))
                {
                    continue;
                }
                for (int Dir = 0; Dir < 4; ++Dir)
                {
                    int NRow = i;
                    int NCol = j;
                    if (Dir < 2)
                    {
                        NRow += (Dir == 0 ? 1 : -1);
                    }
                    else
                    {
                        NCol += (Dir == 2 ? -1 : 1);
                    }

                    if (Blocks(NRow, NCol, p_MovementFlag))
                    {
                        continue;
                    }

                    if (m_Interactables[i, j].IsInteractable(p_ObjectTag, Dir) && m_Interactables[i, j].RequestDirection(Dir))
                    {
                        return (new Vector2Int(NCol, NRow), new Vector2Int(j, i));
                    }
                }
            }
        }

        return (-Vector2Int.one, -Vector2Int.one);
    }
    #endregion

    #region Block Functionality
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
    #endregion


    public void AStar(Vector2Int p_StartPos, Vector2Int p_EndPos, GlobalNamespace.EnumMovementFlag p_MovementFlag, out List<(Vector2Int, int)> OutPath)
    {
        OutPath = new List<(Vector2Int, int)>();

        var CellQueue = new GlobalNamespace.PriorityQ<float, (Vector2Int, float)>();
        CellQueue.Add( ( 0, (p_StartPos, 0) ) );
        var MatrixVisits = new GlobalNamespace.MyMatrix<(float, (int, int))>(Rows, Columns, (float.MaxValue, (0, int.MaxValue)) );

        bool PathFound = false;
        while (!CellQueue.Empty)
        {
            var Cur = CellQueue.Pop();
            Vector2Int CurCell = Cur.Item1;
            float GWeight = Cur.Item2;

            GWeight += 1f;
            for (int i = 1; i < 1 << 4; i <<= 1)
            {
                Vector2Int NextCell = new Vector2Int(
                    CurCell.x + (i % 3 == 1 ? (i / 3 == 0 ? -1 : 1) : 0),
                    CurCell.y + (i % 3 == 2 ? (i / 3 == 0 ? -1 : 1) : 0)
                    );
                float FWeight = (float)System.Math.Round(GWeight + Vector2Int.Distance(NextCell, p_EndPos), 2);
                if (NextCell.y < 0 || NextCell.y >= Rows || NextCell.x < 0 || NextCell.x >= Columns ||
                    MatrixVisits[NextCell.y, NextCell.x].Item1 <= FWeight || (MatrixVisits[NextCell.y, NextCell.x].Item2.Item1 & i) != 0 ||
                    (NextCell != p_EndPos && Blocks(NextCell.y, NextCell.x, p_MovementFlag)))
                {
                    continue;
                }
                if (NextCell == p_EndPos)
                {
                    PathFound = true;
                    MatrixVisits[NextCell.y, NextCell.x] = (0f, (0, i));
                    break;
                }
                MatrixVisits[NextCell.y, NextCell.x] = (GWeight + Vector2Int.Distance(CurCell, p_EndPos),
                    (MatrixVisits[NextCell.y, NextCell.x].Item2.Item1 | i, i) );
                CellQueue.Add( ( FWeight, (NextCell, GWeight) ) );
            }

            if (PathFound)
            {
                break;
            }
        }

        if (!PathFound)
        {
            return;
        }

        OutPath.Add( ( p_EndPos, m_AreaIdx ) );
        Vector2Int OutCell = p_EndPos;
        do
        {
            int Dir = MatrixVisits[OutCell.y, OutCell.x].Item2.Item2;
            OutCell = new Vector2Int(
                OutCell.x + (Dir % 3 == 1 ? (Dir / 3 == 0 ? 1 : -1) : 0),
                OutCell.y + (Dir % 3 == 2 ? (Dir / 3 == 0 ? 1 : -1) : 0)
                );
            OutPath.Add( ( OutCell, m_AreaIdx ) );
        }
        while (OutCell != p_StartPos);

        OutPath.Reverse();
    }

    public bool m_OverrideRow = false;

    public int m_RowsCount = -1;
    public int m_ColumnCount = -1;
    public int m_LeftRowConnection = -100;
    public int m_RightRowConnection = -100;


    #region Extra Functions

    private AreaData GetAreaDescription()
    {
        AreaData CompleteAreaDescription = new AreaData();

        CompleteAreaDescription.AreaName = m_AreaName;

        List<AreaData.AreaObject> AreaObjects = new List<AreaData.AreaObject>();
        for (int i = 0; i < m_Interactables.Rows; ++i)
        {
            for (int j = 0; j < m_Interactables.Columns; ++j)
            {
                if (m_Interactables[i][j] == null)
                {
                    continue;
                }
                AreaObjects.Add(ConstructDescriptionFromObject(m_Interactables[i][j], i, j));
            }
        }
        CompleteAreaDescription.Objects = AreaObjects.ToArray();

        return CompleteAreaDescription;
    }

    public void RemoveElement(CellElement p_Cell)
    {
        //THIS FUNCTION WILL BE USED TO REMOVE AN ELEMENT FROM THE GIVEN CELL
        //POSSIBLE REFUND
        if (!Blocks(p_Cell.m_CurCellPos, (GlobalNamespace.EnumMovementFlag)int.MaxValue))
        {
            return;
        }

        m_Interactables[p_Cell.m_CurCellPos.x, p_Cell.m_CurCellPos.y].GetComponent<CellElement>().HideModel();
        RemoveObjectFromMatrix(p_Cell);
    }
    public void RemoveElement(int p_PosX, int p_PosY)
    {
        if (m_Interactables[p_PosX, p_PosY] == null)
        {
            return;
        }

        //THIS FUNCTION WILL BE USED TO REMOVE AN ELEMENT FROM THE GIVEN CELL
        //POSSIBLE REFUND
        if (!Blocks(p_PosX, p_PosY, (GlobalNamespace.EnumMovementFlag)int.MaxValue))
        {
            return;
        }

        m_Interactables[p_PosX, p_PosY].GetComponent<CellElement>().HideModel();
        RemoveObjectFromMatrix(p_PosX, p_PosY);
    }

    public void ResetElements()
    {
        var AD = SaveLoadSystem.Manager.GetAreaDescription(m_AreaName);

        foreach (var Elem in AD.Objects)
        {
            //object exists in description and not on map
            //OR
            //object in description and real object have different types
            if (m_Interactables[Elem.PositionX, Elem.PositionY] == null
                || m_Interactables[Elem.PositionX, Elem.PositionY].InteractableType != Elem.ObjectType)
            {
                RemoveElement(Elem.PositionX, Elem.PositionY);
                Area.ConstructObjectFromDescription(Elem, false);
            }
        }

        for (int i = 0; i < m_Interactables.Rows; ++i)
        {
            for (int j = 0; j < m_Interactables.Columns; ++j)
            {
                if (m_Interactables[i, j] == null || m_Interactables[i, j].m_NewElement == false)
                {
                    continue;
                }
                RemoveElement(i, j);
            }
        }
    }
    #endregion
}


#region Custom Area Editor
#if UNITY_EDITOR

[CustomEditor(typeof(Area))]
[CanEditMultipleObjects]
class AreaEditor : Editor
{
    private Area TargetArea;
    private SerializedProperty m_AreaDescriptionProperty;
    private SerializedProperty m_LocalIPLProperty;
    private SerializedProperty m_LocalIILProperty;
    private void OnEnable()
    {
        TargetArea = (Area)target;
        m_AreaDescriptionProperty = serializedObject.FindProperty("m_AreaDescription");
        m_LocalIPLProperty = serializedObject.FindProperty("m_LocalInteractablePrefabList");
        m_LocalIILProperty = serializedObject.FindProperty("m_LocalIngredientDescriptorList");
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
        EditorGUILayout.PropertyField(m_LocalIPLProperty, new GUIContent("Interactable Object Types", "List of all available interactable objects in game. Must be moved to a separate class."));
        EditorGUILayout.PropertyField(m_LocalIILProperty, new GUIContent("Ingredient Descriptors", "List of all available ingredient descriptors in game. Must be moved to a separate class."));
        EditorGUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();

        GUIContent AreaNameTooltip = new GUIContent("Area Name", "This is a unique name for each area in each level. There is only 1 Area per level");
        TargetArea.m_AreaName = EditorGUILayout.TextField(AreaNameTooltip, TargetArea.m_AreaName);
        TargetArea.m_AreaIdx = EditorGUILayout.IntField("Area Idx", TargetArea.m_AreaIdx);


        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Area Size Overrides", EditorStyles.boldLabel);
        TargetArea.m_OverrideRow        = EditorGUILayout.BeginToggleGroup("Override", TargetArea.m_OverrideRow);
        TargetArea.m_RowsCount          = EditorGUILayout.IntField("Rows Count", TargetArea.m_RowsCount);
        TargetArea.m_ColumnCount        = EditorGUILayout.IntField("Columns Count", TargetArea.m_ColumnCount);
        TargetArea.m_LeftRowConnection  = EditorGUILayout.IntField("Left Row Connection", TargetArea.m_LeftRowConnection);
        TargetArea.m_RightRowConnection = EditorGUILayout.IntField("Right Row Connection", TargetArea.m_RightRowConnection);
        EditorGUILayout.EndToggleGroup();
    }
}

#endif
#endregion
