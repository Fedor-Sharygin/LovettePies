using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RestaurantTable : Interactable
{
    [Space(10)]

    [SerializeField]
    private Plate.EnumPlateState[] m_RequiredPlateState;

    [SerializeField]
    private ObjectSocket[] m_PlatePositions;
    [SerializeField]
    private ObjectSocket[] m_SeatPositions;

    public override bool IsInteractable(string p_ObjectTag, int Direction = -1)
    {
        if (Direction < 0 || Direction >= 4)
        {
            return base.IsInteractable(p_ObjectTag);
        }
        switch (p_ObjectTag)
        {
            case "Player":
            case "RestaurantCustomer":
                {
                    return true;
                }
        }
        return false;
    }

    public override void Interact(MonoBehaviour m_Interactee, int Direction = -1)
    {
        if (Direction < 0 || Direction >= 4)
        {
            base.Interact(m_Interactee);
            return;
        }
        switch (m_Interactee.tag)
        {
            case "Player":
                {
                    if (m_PlatePositions[Direction].IsLocked)
                    {
                        break;
                    }

                    var PlayerObject = m_Interactee as PlayerRestaurantController;
                    if (!m_PlatePositions[Direction].AvailableForStack)
                    {
                        if (!PlayerObject.m_DishPos.AvailableForStack)
                        {
                            break;
                        }
                        PlayerObject.m_DishPos.Stack(m_PlatePositions[Direction].RemoveObj());
                    }
                    else
                    {
                        var PlateTransform = PlayerObject.m_DishPos.PeekObj();
                        var PlayerPlate = PlateTransform?.gameObject?.GetComponent<Plate>();
                        if (PlayerPlate == null)
                        {
                            break;
                        }
                        bool IsOfReqState = false;
                        foreach (var State in m_RequiredPlateState)
                        {
                            if (PlayerPlate.m_PlateState == State)
                            {
                                IsOfReqState = true;
                                break;
                            }
                        }
                        if (!IsOfReqState)
                        {
                            break;
                        }
                        m_PlatePositions[Direction].Stack(PlayerObject.m_DishPos.RemoveObj());

                        if (!m_SeatPositions[Direction].AvailableForStack)
                        {
                            m_SeatPositions[Direction].PeekObj().GetComponent<RestaurantBehavior>()?.ReceiveFood(m_PlatePositions[Direction]);
                        }
                    }
                }
                break;
            case "RestaurantCustomer":
                {
                    if (m_SeatPositions[Direction].IsLocked)
                    {
                        break;
                    }

                    if (!m_SeatPositions[Direction].AvailableForStack)
                    {
                        m_SeatPositions[Direction].RemoveObj();
                    }
                    else
                    {
                        m_SeatPositions[Direction].Stack(m_Interactee.transform);
                        if (!m_PlatePositions[Direction].AvailableForStack)
                        {
                            m_SeatPositions[Direction].PeekObj().GetComponent<RestaurantBehavior>()?.ReceiveFood(m_PlatePositions[Direction]);
                        }
                    }
                }
                break;
        }
    }

    public void SitDown(GameObject p_Seater)
    {

    }
}

#region Custom RestaurantTable Editor
#if UNITY_EDITOR

[CustomEditor(typeof(RestaurantTable))]
[CanEditMultipleObjects]
class RestaurantTableEditor : InteractableEditor
{
    private RestaurantTable TargetObject_RestaurantTable;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_RestaurantTable = (RestaurantTable)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_RestaurantTable == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion