using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoughMinigameController : MonoBehaviour
{
    [SerializeField]
    private Transform m_Dough;
    [SerializeField]
    private TriggerEnterDelegation m_DoughEnterDelegate;
    [SerializeField]
    private float m_TargetDoughScale = 2f;
    private PlayerControls m_PlayerControls;
    private void Awake()
    {
        var ReqElem = GetComponent<MinigameRequiredElement>();

        //ReqElem.m_PlayerInput.SwitchCurrentActionMap("Basic Minigame Controls");
        m_PlayerControls = ReqElem.m_PlayerControls;
        m_Hand_Button = m_PlayerControls.BasicMinigameControls.BasicPress;
        m_Hand_LeftStick = m_PlayerControls.BasicMinigameControls.ControllerMovementLeft;
        m_Hand_RightStick = m_PlayerControls.BasicMinigameControls.ControllerMovementRight;
        m_CloseMinigame = m_PlayerControls.BasicMinigameControls.QuitMinigame;

        //m_DoughEnterDelegate.OnTriggerEntered += DoughTouched;
    }

    private InputAction m_Hand_Button;
    private InputAction m_Hand_LeftStick;
    private InputAction m_Hand_RightStick;
    private InputAction m_CloseMinigame;
    private void OnEnable()
    {
        Debug.LogWarning($"WARING: {GetInstanceID()} IS ENABLING INPUT!!!!");
        m_Hand_Button.Enable();
        m_Hand_Button.performed     += ControlHands_Button;

        m_Hand_LeftStick.Enable();
        m_Hand_LeftStick.started    += ControlHands_Controller_LeftStick;
        m_Hand_LeftStick.canceled   += ControlHands_Controller_LeftStick;

        m_Hand_RightStick.Enable();
        m_Hand_RightStick.started   += ControlHands_Controller_RightStick;
        m_Hand_RightStick.canceled  += ControlHands_Controller_RightStick;


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
        m_Hand_Button.performed     -= ControlHands_Button;
        m_Hand_LeftStick.started    -= ControlHands_Controller_LeftStick;
        m_Hand_LeftStick.canceled   -= ControlHands_Controller_LeftStick;
        m_Hand_RightStick.started   -= ControlHands_Controller_RightStick;
        m_Hand_RightStick.canceled  -= ControlHands_Controller_RightStick;
        m_CloseMinigame.performed   -= CloseMinigame;
    }

    [SerializeField]
    private Transform[] m_Hands;
    //private int m_HandIdx = 0;
    [SerializeField]
    private float m_DoughScaleSpeed_Button;
    public void ControlHands_Button(InputAction.CallbackContext p_CallbackContext)
    {
        if (!p_CallbackContext.performed)
        {
            return;
        }
        IncreaseDoughSize(m_DoughScaleSpeed_Button);
    }

    [SerializeField]
    private float m_DoughScaleSpeed_Controller;
    [SerializeField]
    private float m_HandSpeed;
    private void Update()
    {
        Vector3 LeftMovement = Mathf.Min(Time.deltaTime, 1f / 60f) * m_HandSpeed * GetLeftHandInput();
        m_Hands[0].localPosition += LeftMovement;

        Vector3 RightMovement = Mathf.Min(Time.deltaTime, 1f / 60f) * m_HandSpeed * GetRightHandInput();
        m_Hands[1].localPosition += RightMovement;
    }

    private Vector2 GetLeftHandInput()
    {
        return m_Hand_LeftStick.ReadValue<Vector2>();
    }
    private Vector2 GetRightHandInput()
    {
        return m_Hand_RightStick.ReadValue<Vector2>();
    }

    public void ControlHands_Controller_LeftStick(InputAction.CallbackContext p_CallbackContext)
    {

    }
    public void ControlHands_Controller_RightStick(InputAction.CallbackContext p_CallbackContext)
    {

    }

    //private void DoughTouched(Collider2D p_DoughCollider, Collider2D p_Other)
    //{
    //    if (GetLeftHandInput() == Vector2.zero && GetRightHandInput() == Vector2.zero)
    //    {
    //        //IncreaseDoughSize(m_DoughScaleSpeed_Button);
    //    }
    //    else
    //    {
    //        IncreaseDoughSize(m_DoughScaleSpeed_Controller);
    //    }
    //}

    private void IncreaseDoughSize(float p_DoughScaleSpeed)
    {
        m_Dough.localScale += p_DoughScaleSpeed * Vector3.one;

        if (m_Dough.localScale.x >= m_TargetDoughScale)
        {
            this.DisableInput();
            Debug.LogWarning($"WARNING: {GetInstanceID()} HAS FINISHED COOKING DOUGH!!!");
            GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0);
        }
    }


    private void CloseMinigame(InputAction.CallbackContext obj)
    {
        this.DisableInput();
        Debug.LogWarning($"WARNING: {GetInstanceID()} HAS FINISHED COOKING DOUGH!!!");
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(
            m_Dough.localScale.x >= m_TargetDoughScale
            ? MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0
            : MinigameRequiredElement.MinigameStatus.MINIGAME_FAIL);
    }
}
