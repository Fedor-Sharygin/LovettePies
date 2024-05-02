using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DishWasher : MinigameHolderBase
{
    private ObjectSocket m_OriginSocket;

    private Plate m_CurPlate = null;
    [SerializeField]
    private string m_DishWasherMinigame;
    public override void Interact(MonoBehaviour m_Interactee, int Direction = -1)
    {
        //base.Interact(m_Interactee, Direction);
        if (!m_Interactee.CompareTag("Player"))
        {
            return;
        }

        var PlayerComp = m_Interactee as PlayerRestaurantController;
        if (PlayerComp == null || PlayerComp.m_DishPos.AvailableForStack)
        {
            return;
        }

        m_OriginSocket = PlayerComp.m_DishPos;
        m_CurPlate = PlayerComp.m_DishPos.PeekObj().GetComponent<Plate>();
        if (m_CurPlate == null || m_CurPlate.m_PlateState != Plate.EnumPlateState.PLATE_DIRTY)
        {
            return;
        }

        m_LocalSocket.Stack(m_OriginSocket.RemoveObj());
        SceneManager.LoadSceneAsync(m_DishWasherMinigame, LoadSceneMode.Additive);
        m_IsInteractable[0] = false;
    }

    public override void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status)
    {
        if (p_Status > MinigameRequiredElement.MinigameStatus.MINIGAME_FAIL)
        {
            m_CurPlate?.Clean();
        }

        m_OriginSocket.Stack(m_LocalSocket.RemoveObj());
        m_IsInteractable[0] = true;
    }
}


#region Custom DishWasher Editor
#if UNITY_EDITOR

[CustomEditor(typeof(DishWasher))]
[CanEditMultipleObjects]
class DishWasherEditor : MinigameHolderEditor
{
    private DishWasher TargetObject_DishWasher;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_DishWasher = (DishWasher)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_DishWasher == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion

