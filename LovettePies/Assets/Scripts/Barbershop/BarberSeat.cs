using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarberSeat : MinigameHolderBase
{
    [Space(10)]
    public ObjectSocket m_SeatSocket;
    public string m_BarberMinigame;
    public override bool IsInteractable(string p_ObjectTag, int Direction = -1)
    {
        switch (p_ObjectTag)
        {
            case "Player":
                {
                    return base.IsInteractable(p_ObjectTag, Direction);
                }
            case "BarbershopCustomer":
                {
                    if (Direction == -1)
                    {
                        //INITIAL CHECK IF WE CAN INTERACT
                        return true;
                    }

                    return !(Direction != 2 || m_SeatSocket == null || !m_SeatSocket.AvailableForStack || m_SeatSocket.IsLocked);
                }
        }
        return false;
    }

    public override void Interact(MonoBehaviour p_Interactee, int Direction = -1)
    {
        switch (p_Interactee.tag)
        {
            case "Player":
                {
                    //IF SOMEONE IN THERE - FIRE UP THE BARBER MINIGAME
                    if (m_SeatSocket.AvailableForStack)
                    {
                        break;
                    }

                    m_SeatSocket.PeekObj().GetComponent<BarbershopBehavior>().HaircutBegins();
                    SceneManager.LoadSceneAsync(m_BarberMinigame, LoadSceneMode.Additive);
                }
                break;
            case "BarbershopCustomer":
                {
                    if (Direction != 2 || m_SeatSocket == null)
                    {
                        return;
                    }

                    if (m_SeatSocket.AvailableForStack)
                    {
                        m_SeatSocket.Stack(p_Interactee.transform);
                    }
                    else
                    {
                        m_SeatSocket.RemoveObj();
                    }
                    FlipTagInteractableState("Player");
                }
                break;
        }
    }

    protected override void MinigameLoaded(Scene p_MinigameScene, LoadSceneMode _p_LoadSceneMode)
    {
        base.MinigameLoaded(p_MinigameScene, _p_LoadSceneMode);

        foreach (var MGHolder in GameObject.FindGameObjectsWithTag("MinigameHolder"))
        {
            //if ((HaircutController HCControl = MGHolder.GetComponent<HaircutMinigameController>()) == null) {
            //    continue;
            //}
        }
    }

    public ObjectSocket m_CustomerDropSocket;
    public override void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status)
    {
        switch (p_Status)
        {
            //SUCCESS_0 - HAIRCUT IS SUCCESSFUL
            case MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0:
                {
                    m_SeatSocket.PeekObj().GetComponent<BarbershopBehavior>()?.HaircutEnds_Success();
                }
                break;

            //SUCCESS_1 - WE GOT THE MEATS
            case MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_1:
                {
                    m_SeatSocket.PeekObj().GetComponent<BarbershopBehavior>()?.HaircutEnds_Fail();
                    m_CustomerDropSocket.ForceStack(m_SeatSocket.RemoveObj());
                    FreeDirection(2);
                    FlipTagInteractableState("Player");
                }
                break;
        }
    }
}


#region Custom BarberSeat Editor
#if UNITY_EDITOR

[CustomEditor(typeof(BarberSeat))]
[CanEditMultipleObjects]
class BarberSeatEditor : MinigameHolderEditor
{
    private BarberSeat TargetObject_BarberSeat;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_BarberSeat = (BarberSeat)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_BarberSeat == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion

