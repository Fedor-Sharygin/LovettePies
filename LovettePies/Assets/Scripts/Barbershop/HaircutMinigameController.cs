using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HaircutMinigameController : MonoBehaviour
{
    private PlayerControls m_PlayerControls;
    private void Awake()
    {
        var ReqElem = GetComponent<MinigameRequiredElement>();

        //ReqElem.m_PlayerInput.SwitchCurrentActionMap("Basic Minigame Controls");
        m_PlayerControls = ReqElem.m_PlayerControls;

        //m_DoughEnterDelegate.OnTriggerEntered += DoughTouched;
    }

    private InputAction m_Haircut_MouseLB;
    private InputAction m_Haircut_Button;
    private InputAction m_Lever_Button;
    private InputAction m_Hand_MouseMovement;
    private InputAction m_CloseMinigame;
    private void OnEnable()
    {
        Debug.LogWarning($"WARING: {GetInstanceID()} IS ENABLING INPUT!!!!");


        m_Haircut_MouseLB = m_PlayerControls.BasicMinigameControls.MousePress;
        m_Haircut_MouseLB.Enable();
        m_Haircut_MouseLB.started += HaircutAction_MouseStart;
        m_Haircut_MouseLB.canceled += HaircutAction_MouseEnd;

        m_Haircut_Button = m_PlayerControls.BasicMinigameControls.BasicPress;
        m_Haircut_Button.Enable();
        m_Haircut_Button.performed += HaircutAction_Button;

        m_Lever_Button = m_PlayerControls.BasicMinigameControls.AlternatePress;
        m_Lever_Button.Enable();
        m_Lever_Button.performed += LeverAction_Button;

        //m_Hand_MouseMovement = m_PlayerControls.BasicMinigameControls.ControllerMovementLeft;
        //m_Hand_MouseMovement.Enable();
        //m_Hand_MouseMovement.started += ControlHands_Controller_LeftStick;
        //m_Hand_MouseMovement.canceled += ControlHands_Controller_LeftStick;

        //m_Hand_RightStick = m_PlayerControls.BasicMinigameControls.ControllerMovementRight;
        //m_Hand_RightStick.Enable();
        //m_Hand_RightStick.started += ControlHands_Controller_RightStick;
        //m_Hand_RightStick.canceled += ControlHands_Controller_RightStick;


        m_CloseMinigame = m_PlayerControls.BasicMinigameControls.QuitMinigame;
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


        m_Haircut_MouseLB.started       -= HaircutAction_MouseStart;
        m_Haircut_MouseLB.canceled      -= HaircutAction_MouseEnd;
        m_Haircut_Button.performed      -= HaircutAction_Button;
        m_Lever_Button.performed        -= LeverAction_Button;
        //m_Hand_MouseMovement.started  -= ControlHands_Controller_LeftStick;
        //m_Hand_MouseMovement.canceled -= ControlHands_Controller_LeftStick;
        //m_Hand_RightStick.started     -= ControlHands_Controller_RightStick;
        //m_Hand_RightStick.canceled    -= ControlHands_Controller_RightStick;
        m_CloseMinigame.performed       -= CloseMinigame;
    }

    private float m_HaircutPercent = 0f;
    private const float m_PercentEpsilon = 0.005f;
    private void HaircutAction_Button(InputAction.CallbackContext obj)
    {
        m_HaircutPercent += .05f;
        if (m_HaircutPercent >= 1f - m_PercentEpsilon)
        {
            //FINISH THE HAIRCUT - SUCCESS
            this.DisableInput();
            GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_0);
        }
    }

    private void LeverAction_Button(InputAction.CallbackContext obj)
    {
        //KILL THE PERSON FOR MEAT - SUCCESS
        this.DisableInput();
        GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(MinigameRequiredElement.MinigameStatus.MINIGAME_SUCCESS_1);
    }


    [SerializeField]
    private GameObject m_DashedLinePrefab;
    private GameObject m_CurDashedLine;
    private void HaircutAction_MouseStart(InputAction.CallbackContext obj)
    {
        if (m_DashedLinePrefab == null)
        {
            return;
        }

        //ONLY FOR TESTING
        var MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit MouseHit;

        if (Physics.Raycast(MouseRay, out MouseHit, 5f))
        {
            m_CurDashedLine = GameObject.Instantiate(m_DashedLinePrefab, MouseHit.point, Quaternion.identity, transform);
        }
    }
    private void HaircutAction_MouseEnd(InputAction.CallbackContext obj)
    {
        if (m_CurDashedLine == null)
        {
            return;
        }

        Destroy(m_CurDashedLine);
    }

    private void CloseMinigame(InputAction.CallbackContext obj)
    {
        this.DisableInput();
        //Debug.LogWarning($"WARNING: {GetInstanceID()} HAS FINISHED COOKING DOUGH!!!");
        //GetComponent<MinigameRequiredElement>()?.PlayEndgameAnimation(m_Dough.localScale.x >= m_TargetDoughScale);
    }
}
