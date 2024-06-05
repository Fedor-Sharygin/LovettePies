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

        BARBERSHOP_SEAT,

        BARBERSHOP_END,
        #endregion


        #region Police Station Enums

        POLICE_DOCUMENT,
        POLICE_JAIL,

        POLICE_END,
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

    public Vector2Int m_StartPos;
    private Vector2Int m_PrevCellPos;
    private Vector2Int m_CellPos;
    [SerializeField]
    private CellElement m_CellElem;
    protected virtual void PlaceOnArea()
    {
        StartCoroutine("MoveObjectToCell");
    }

    private void Start()
    {
        PlaceOnArea();
    }
    private void OnDestroy()
    {
        CellElement.AreaArray[m_AreaType].RemoveObjectFromMatrix(m_CellPos.y, m_CellPos.x);
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

        MoveToPosition();
    }

    public void MoveToPosition()
    {
        m_CellElem.MoveTo(new Vector2Int(m_CellPos.y, m_CellPos.x));
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
    public virtual void Interact(MonoBehaviour m_Interactee, int Direction = -1)
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

    public virtual bool IsInteractable(string p_ObjectTag, int Direction = -1)
    {
        bool ActionIsInteractable = false;
        for (int i = 0; i < m_InteracteeTags.Length; ++i)
        {
            if (p_ObjectTag != m_InteracteeTags[i])
            {
                continue;
            }

            ActionIsInteractable |= m_IsInteractable[i];
        }

        return ActionIsInteractable;
    }


    private bool[] m_DirectionsTaken = new bool[4];
    public bool RequestDirection(int p_Direction)
    {
        if (m_DirectionsTaken[p_Direction])
        {
            return false;
        }
        m_DirectionsTaken[p_Direction] = true;
        return true;
    }
    public void FreeDirection(int p_Direction)
    {
        m_DirectionsTaken[p_Direction] = false;
    }


    public void TestInteractReaction()
    {
        Debug.Log($"Interacting with object {name}-{m_ObjectType}");
    }
    public void TestInteractReaction2()
    {
        Debug.Log($"Customer Interacting with object {name}-{m_ObjectType}");
    }

    public void FlipTagInteractableState(string p_InteracteeTag)
    {
        for (int i = 0; i < m_InteracteeTags.Length; ++i)
        {
            if (p_InteracteeTag != m_InteracteeTags[i])
            {
                continue;
            }

            m_IsInteractable[i] = !m_IsInteractable[i];
        }
    }
}

#region Custom Interactable Editor
#if UNITY_EDITOR

[CustomEditor(typeof(Interactable), true)]
[CanEditMultipleObjects]
class InteractableEditor : Editor
{
    private Interactable TargetObject;
    protected virtual void CustomOnEnable()
    {
        TargetObject = (Interactable)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
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
