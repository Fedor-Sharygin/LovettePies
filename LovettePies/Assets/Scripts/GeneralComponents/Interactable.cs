using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Interactable : MonoBehaviour
{
    [Serializable]
    protected enum EnumInteractableType
    {
        CONNECTION,


        #region Restaurant Enums

        RESTAURANT_TABLE,
        RESTAURANT_STOVE,
        RESTAURANT_OVEN,
        RESTAURANT_PLATE_HOLDER,
        RESTAURANT_SINK,

        RESTAURANT_END,
        #endregion


        #region Barber Shop Enums

        BARBERSHOP_END,
        #endregion


        DEFAULT
    }
    [Tooltip("Object Type describes what type of interaction it has")]
    [SerializeField]
    protected EnumInteractableType m_ObjectType;


    [HideInInspector]
    public GlobalNamespace.EnumMovementFlag m_MovementFlag = 0x0;
    public bool Blocks(GlobalNamespace.EnumMovementFlag p_MovementFlag)
    {
        return (m_MovementFlag & p_MovementFlag) != 0x0;
    }

    [Space(15)]
    [Tooltip("The Area type and Area Index are interchangeable")]
    [SerializeField]
    protected int m_AreaType;

    [SerializeField]
    private Vector2Int m_StartPos;
    private Vector2Int m_PrevCellPos;
    private Vector2Int m_CellPos;
    private CellElement m_CellElem;
    private void Start()
    {
        m_CellElem = GetComponent<CellElement>();
        m_CellPos = m_StartPos;
        //m_PrevCellPos = GlobalNamespace.ObjectExtension.Clone(m_CellPos);
        m_PrevCellPos.x = m_CellPos.x;
        m_PrevCellPos.y = m_CellPos.y;

        m_CellElem.MoveBy(m_CellPos);
        MoveObjectToCell();
    }

    private void MoveObjectToCell()
    {
        CellElement.AreaArray[m_AreaType].RemoveObjectFromMatrix(m_PrevCellPos.y, m_PrevCellPos.x);
        CellElement.AreaArray[m_AreaType].AddObjectToMatrix(this, m_CellPos.y, m_CellPos.x);

        //m_PrevCellPos = GlobalNamespace.ObjectExtension.Clone(m_CellPos);
        m_PrevCellPos.x = m_CellPos.x;
        m_PrevCellPos.y = m_CellPos.y;
    }

    [SerializeField]
    private bool m_IsInteractable = false;
    public bool IsInteractable
    {
        get
        {
            return m_IsInteractable;
        }
    }

    [Space(15)]
    [SerializeField]
    protected UnityEvent m_InteractionActions;
    public virtual void Interact()
    {
        if (!m_IsInteractable)
        {
            return;
        }

        m_InteractionActions?.Invoke();
    }


    public void TestInteractReaction()
    {
        Debug.Log($"Interacting with object {name}-{m_ObjectType}");
    }
}

#region Custom Interactable Editor
#if UNITY_EDITOR

[CustomEditor(typeof(Interactable))]
class InteractableEditor : Editor
{
    private Interactable TargetObject;
    private SerializedProperty m_MovementFlagProperty;
    private void OnEnable()
    {
        TargetObject = (Interactable)target;
        m_MovementFlagProperty = serializedObject.FindProperty("m_MovementFlag");
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject == null)
        {
            return;
        }

        Undo.RecordObject(TargetObject, "Interactable Object");

        TargetObject.m_MovementFlag = (GlobalNamespace.EnumMovementFlag)EditorGUILayout.EnumFlagsField("Movement Mask", TargetObject.m_MovementFlag);

        EditorGUILayout.Space(5);
        DrawDefaultInspector();
    }
}

#endif
#endregion
