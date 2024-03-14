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
    public enum EnumInteractableType
    {
        KITCHEN_CONNECTION,
        STORE_CONNECTION,

        RESTAURANT_CUSTOMER,
        BARBERSHOP_CUSTOMER,


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
    public EnumInteractableType InteractableType
    {
        get
        {
            return m_ObjectType;
        }
    }


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
    [SerializeField]
    private CellElement m_CellElem;
    private void Start()
    {
        StartCoroutine("MoveObjectToCell");
    }

    private void MoveObjectToCell()
    {
        while (CellElement.AreaArray.Length == 0) ;

        //m_CellElem = GetComponent<CellElement>();
        m_CellPos = m_StartPos;
        m_CellElem.AreaIdx = m_AreaType;
        //m_PrevCellPos = GlobalNamespace.ObjectExtension.Clone(m_CellPos);
        m_PrevCellPos.x = m_CellPos.x;
        m_PrevCellPos.y = m_CellPos.y;

        m_CellElem.MoveBy(new Vector2Int(m_CellPos.y, m_CellPos.x));
        CellElement.AreaArray[m_AreaType].RemoveObjectFromMatrix(m_PrevCellPos.y, m_PrevCellPos.x);
        CellElement.AreaArray[m_AreaType].AddObjectToMatrix(this, m_CellPos.y, m_CellPos.x);

        m_PrevCellPos.x = m_CellPos.x;
        m_PrevCellPos.y = m_CellPos.y;
    }

    [Space(15)]
    [SerializeField]
    protected UnityEvent[] m_InteractionActions;
    [SerializeField]
    public bool[] m_IsInteractable;
    [SerializeField]
    protected string[] m_InteracteeTags;
    public virtual void Interact(MonoBehaviour m_Interactee)
    {
        for (int i = 0; i < m_InteracteeTags.Length; ++i)
        {
            if (m_Interactee.tag != m_InteracteeTags[i] || !m_IsInteractable[i])
            {
                continue;
            }

            m_InteractionActions[i]?.Invoke();
        }
    }

    public bool IsInteractable(string p_ObjectTag)
    {
        for (int i = 0; i < m_InteracteeTags.Length; ++i)
        {
            if (p_ObjectTag != m_InteracteeTags[i])
            {
                continue;
            }

            return m_IsInteractable[i];
        }

        return false;
    }


    public void TestInteractReaction()
    {
        Debug.Log($"Interacting with object {name}-{m_ObjectType}");
    }
    public void TestInteractReaction2()
    {
        Debug.Log($"Customer Interacting with object {name}-{m_ObjectType}");
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
