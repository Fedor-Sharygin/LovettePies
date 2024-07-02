using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DocumentMiniGameController : MonoBehaviour
{
    private PlayerControls m_PlayerControls;
    private void Awake()
    {
        var ReqElem = GetComponent<MinigameRequiredElement>();

        //ReqElem.m_PlayerInput.SwitchCurrentActionMap("Basic Minigame Controls");
        m_PlayerControls = ReqElem.m_PlayerControls;
        m_Confirm_Button = m_PlayerControls.BasicMinigameControls.BasicPress;
        m_Decline_Button = m_PlayerControls.BasicMinigameControls.AlternatePress;
        m_CloseMinigame = m_PlayerControls.BasicMinigameControls.QuitMinigame;

        //m_DoughEnterDelegate.OnTriggerEntered += DoughTouched;
    }

    private InputAction m_Confirm_Button;
    private InputAction m_Decline_Button;
    private InputAction m_CloseMinigame;
    private void OnEnable()
    {
        Debug.LogWarning($"WARING: {GetInstanceID()} IS ENABLING INPUT!!!!");

        m_Confirm_Button.Enable();
        m_Confirm_Button.performed += ConfirmCrime_Button;

        m_Decline_Button.Enable();
        m_Decline_Button.performed += DeclineCrime_Button;

        m_CloseMinigame.Enable();
        m_CloseMinigame.performed += CloseMinigame;
    }

    private void OnDisable()
    {
        DisableInput();
    }
    private void DisableInput()
    {
        Debug.LogWarning($"WARING: {GetInstanceID()} IS DISABLING INPUT!!!!");
        m_Confirm_Button.performed -= ConfirmCrime_Button;
        m_Decline_Button.performed -= DeclineCrime_Button;
        m_CloseMinigame.performed -= CloseMinigame;
    }


    private void ConfirmCrime_Button(InputAction.CallbackContext obj)
    {
        this.DisableInput();
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0);
    }

    private void DeclineCrime_Button(InputAction.CallbackContext obj)
    {
        this.DisableInput();
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_1);
    }

    private void CloseMinigame(InputAction.CallbackContext obj)
    {
        this.DisableInput();
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_FAIL);
    }
}
