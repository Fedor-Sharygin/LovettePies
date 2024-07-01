using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DishWasherMinigame : MonoBehaviour
{
    [SerializeField]
    private Transform m_BubbleTransform;
    [SerializeField]
    private float m_TargetHeight = -3.8f;


    private PlayerControls m_PlayerControls;
    private void Awake()
    {
        var ReqElem = GetComponent<MinigameRequiredElement>();

        //ReqElem.m_PlayerInput.SwitchCurrentActionMap("Basic Minigame Controls");
        m_PlayerControls = ReqElem.m_PlayerControls;
        m_WashDish = m_PlayerControls.BasicMinigameControls.BasicPress;
        m_CloseMinigame = m_PlayerControls.BasicMinigameControls.QuitMinigame;

        //m_DoughEnterDelegate.OnTriggerEntered += DoughTouched;
    }

    private InputAction m_WashDish;
    private InputAction m_CloseMinigame;
    private void OnEnable()
    {
        m_WashDish.Enable();
        m_WashDish.performed += WashDish;

        //m_Hand_LeftStick = m_PlayerControls.BasicMinigameControls.ControllerMovementLeft;
        //m_Hand_LeftStick.Enable();
        //m_Hand_LeftStick.started += ControlHands_Controller_LeftStick;
        //m_Hand_LeftStick.canceled += ControlHands_Controller_LeftStick;

        //m_Hand_RightStick = m_PlayerControls.BasicMinigameControls.ControllerMovementRight;
        //m_Hand_RightStick.Enable();
        //m_Hand_RightStick.started += ControlHands_Controller_RightStick;
        //m_Hand_RightStick.canceled += ControlHands_Controller_RightStick;


        m_CloseMinigame.Enable();
        m_CloseMinigame.performed += CloseMinigame;
    }

    private void OnDisable()
    {
        DisableInput();
    }
    private void DisableInput()
    {
        m_WashDish.performed -= WashDish;
        //m_Hand_LeftStick.started -= ControlHands_Controller_LeftStick;
        //m_Hand_LeftStick.canceled -= ControlHands_Controller_LeftStick;
        //m_Hand_RightStick.started -= ControlHands_Controller_RightStick;
        //m_Hand_RightStick.canceled -= ControlHands_Controller_RightStick;
        m_CloseMinigame.performed -= CloseMinigame;
    }


    [SerializeField]
    private float m_WashSpeed;
    private void WashDish(InputAction.CallbackContext p_CallbackContext)
    {
        m_BubbleTransform.localPosition += Mathf.Min(1f / 60f, Time.deltaTime) * m_WashSpeed * Vector3.up;
        if (m_BubbleTransform.localPosition.y >= m_TargetHeight - 0.001f)
        {
            this.DisableInput();
            GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0);
        }
    }

    private void CloseMinigame(InputAction.CallbackContext p_CallbackContext)
    {
        this.DisableInput();
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(
            m_BubbleTransform.localPosition.y >= m_TargetHeight - 0.001f ?
            MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0 :
            MinigameRequiredElement.MinigameStatus.MINIGAME_FAIL);
    }
}
