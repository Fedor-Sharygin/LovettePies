using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PoliceStationTable : MinigameHolderBase
{
    [Space(10)]
    public string m_PoliceTableMinigame;

    public override bool IsInteractable(string p_ObjectTag, int Direction = -1)
    {
        switch (p_ObjectTag)
        {
            case "Player":
                {
                    return !m_LocalSocket.AvailableForStack;
                }
            case "PoliceGroup":
                {
                    if (Direction == -1)
                    {
                        //INITIAL CHECK IF WE CAN INTERACT
                        return true;
                    }

                    return !(Direction != 2 || m_LocalSocket == null || !m_LocalSocket.AvailableForStack || m_LocalSocket.IsLocked);
                }
        }

        return false;
    }

    private PoliceGroupBehavior m_PoliceGroup;
    public override void Interact(MonoBehaviour p_Interactee, int Direction = -1)
    {
        switch (p_Interactee.tag)
        {
            case "Player":
                {
                    SceneManager.LoadSceneAsync(m_PoliceTableMinigame, LoadSceneMode.Additive);
                }
                break;
            case "PoliceGroup":
                {
                    if (Direction != 2 || m_LocalSocket == null)
                    {
                        return;
                    }

                    if (m_LocalSocket.AvailableForStack)
                    {
                        m_LocalSocket.Stack(p_Interactee.transform);
                        m_PoliceGroup = p_Interactee.GetComponent<PoliceGroupBehavior>();
                    }
                    else
                    {
                        m_LocalSocket.RemoveObj();
                    }
                }
                break;
        }
    }

    public override void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status)
    {
        switch (p_Status)
        {
            case MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0: //CRIMINAL IS BEING PUT TO JAIL
                {
                }
                break;
            case MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_1: //CRIMINAL IS BEING RELEASED
                {
                    m_PoliceGroup?.ReleaseCriminal();
                }
                break;
        }
        m_PoliceGroup?.DocumentEndActions();
    }
}


#region Custom PoliceStationTable Editor
#if UNITY_EDITOR

[CustomEditor(typeof(RestaurantTable))]
[CanEditMultipleObjects]
class PoliceStationTableEditor : MinigameHolderEditor
{
    private PoliceStationTable TargetObject_PoliceStationTable;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_PoliceStationTable = (PoliceStationTable)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_PoliceStationTable == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion
