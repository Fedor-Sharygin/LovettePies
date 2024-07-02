using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateMenu : MonoBehaviour
{
    [SerializeField]
    private CellElement m_CellElem;
    public Vector2Int m_StartPos;
    private void Awake()
    {
        SetupUpgrades();
        SetupInput();

        //m_CellElem = GetComponent<CellElement>();
        m_CellElem.AreaIdx = 0;
        //m_CellElem.MoveTo(new Vector2Int(m_StartPos.y, m_StartPos.x));
        m_CellElem.MoveTo(m_StartPos);

        m_UIHandler?.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }


    #region Setup
    private UpgradeDescription[] m_Upgrades;
    private void SetupUpgrades()
    {
        //GIANT ASSUMPTION THAT THE LAST THREE "BUTTONS" WILL BE
        //RESET - REFUND - EXIT
        m_Upgrades = new UpgradeDescription[transform.childCount - 3];
        int Idx = 0;
        foreach (var UC in GetComponentsInChildren<UpgradeContainer>())
        {
            m_Upgrades[Idx] = UC.m_UpgradeDescription;
            Idx++;
        }
    }

    private InputAction m_OptionNavigation;
    private InputAction m_PlaceElement;
    private InputAction m_QuitUpgrade;
    private InputAction m_AreaNavigation;
    private PlayerControls m_UpgradeControls;
    private void SetupInput()
    {
        GeneralGameBehavior.Initialize();
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.UPDGRADE_STATE);
        m_UpgradeControls = GeneralGameBehavior.Controls;

        m_OptionNavigation = m_UpgradeControls.UpgradeControls.UpgradeNavigation;
        m_PlaceElement = m_UpgradeControls.UpgradeControls.PlaceElement;
        m_QuitUpgrade = m_UpgradeControls.UpgradeControls.EndUpgrade;
        m_AreaNavigation = m_UpgradeControls.UpgradeControls.AreaNavigation;
    }


    private void EnableInput()
    {
        m_OptionNavigation.Enable();
        m_OptionNavigation.performed += OptionNavigation_Performed;
        m_OptionNavigation.canceled += OptionNavigation_Canceled;

        m_PlaceElement.Enable();
        m_PlaceElement.started += PlaceElement_Started;

        m_QuitUpgrade.Enable();
        m_QuitUpgrade.started += QuitUpgrade_Started;

        m_AreaNavigation.Enable();
        m_AreaNavigation.started += AreaNavigation_Started;
        m_AreaNavigation.canceled += AreaNavigation_Canceled;
    }


    private void OnEnable()
    {
        EnableInput();
    }


    private void DisableInput()
    {
        m_OptionNavigation.performed -= OptionNavigation_Performed;
        m_OptionNavigation.canceled -= OptionNavigation_Canceled;
        m_OptionNavigation.Disable();

        m_PlaceElement.started -= PlaceElement_Started;
        m_PlaceElement.Disable();

        m_QuitUpgrade.started -= QuitUpgrade_Started;
        m_QuitUpgrade.Disable();

        m_AreaNavigation.started -= AreaNavigation_Started;
        m_AreaNavigation.canceled -= AreaNavigation_Canceled;
        m_AreaNavigation.Disable();

        //ENORMOUS ASSUMPTION - WE SWITCH TO THE REGULAR GAMEPLAY LOOP
        //IF WE EVER CHANGE THIS DECISION - NEED TO UPDATE THE FUNCTION CALL
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.DEFAULT_GAME_STATE);
    }
    private void OnDisable()
    {
        DisableInput();
    }
    private void OnDestroy()
    {
        DisableInput();
    }
    #endregion


    #region Input Actions
    [SerializeField]
    private UnityEngine.EventSystems.EventSystem m_UIHandler;
    private float m_OptionNavigationDirection = 0f;
    private int m_CurSelectedChild = 0;
    private bool m_SelectionHeld;
    private float m_CurTimeHeld = 0f;
    private int m_CurTimerIdx = 0;
    private float[] m_Timers = { 1f, .7f, .35f, .16f };
    private void OptionNavigation_Performed(InputAction.CallbackContext p_Obj)
    {
        m_OptionNavigationDirection = m_OptionNavigation.ReadValue<float>();
        m_SelectionHeld = true;
    }
    private void OptionNavigation_Canceled(InputAction.CallbackContext p_Obj)
    {
        m_CurTimerIdx = 0;
        m_CurTimeHeld = 0f;
        m_SelectionHeld = false;
    }

    private void SelectNextUI()
    {
        int CurDir = m_OptionNavigationDirection > 0 ? 1 : (m_OptionNavigationDirection < 0 ? -1 : 0);
        m_CurSelectedChild = (m_CurSelectedChild + CurDir + transform.childCount) % transform.childCount;
        m_UIHandler?.SetSelectedGameObject(transform.GetChild(m_CurSelectedChild).gameObject);
    }
    private void UpdateSelection()
    {
        if (!m_SelectionHeld)
        {
            return;
        }

        m_CurTimeHeld += Time.deltaTime;
        if (m_CurTimeHeld == Time.deltaTime && m_CurTimerIdx == 0)
        {
            SelectNextUI();
            return;
        }

        if (m_CurTimeHeld >= m_Timers[m_CurTimerIdx])
        {
            SelectNextUI();
            if (m_CurTimerIdx < m_Timers.Length - 1)
            {
                m_CurTimerIdx++;
            }
            m_CurTimeHeld = 0f;
        }
    }


    private void PlaceElement_Started(InputAction.CallbackContext p_Obj)
    {
        //If any of the last 3 buttons are selected and used
        //run the corresponding function and stop
        switch (m_CurSelectedChild)
        {
            case int n when n == transform.childCount - 3:
                {
                    //Remove the object Button

                }
                return;
            case int n when n == transform.childCount - 2:
                {
                    //Reset the current upgrades Button

                }
                return;
            case int n when n == transform.childCount - 1:
                {
                    //Quit upgrade Button
                    QuitUpgrade();
                }
                return;
        }



        if ((m_Upgrades[m_CurSelectedChild].m_UpgradeType & UpgradeType.FUNCTION_CALL) != 0x0
            && !string.IsNullOrWhiteSpace(m_Upgrades[m_CurSelectedChild].m_FunctionName))
        {
            m_Upgrades[m_CurSelectedChild].InvokeFunction();
        }

        if ((m_Upgrades[m_CurSelectedChild].m_UpgradeType & UpgradeType.OBJECT_PLACEMENT) != 0x0
            && m_Upgrades[m_CurSelectedChild].m_ObjectToPlace != null)
        {
            var NewElem = GameObject.Instantiate(m_Upgrades[m_CurSelectedChild].m_ObjectToPlace);
            if (NewElem == null)
            {
                return;
            }

            NewElem.GetComponent<Interactable>().m_StartPos = m_CellElem.m_CurCellPos;
        }

    }


    private void QuitUpgrade()
    {
        this.gameObject.SetActive(false);
    }
    private void QuitUpgrade_Started(InputAction.CallbackContext p_Obj)
    {
        QuitUpgrade();
    }



    private Vector2Int GetMoveDirection()
    {
        Vector2 MoveDirection = m_AreaNavigation.ReadValue<Vector2>();
        if (Mathf.Abs(MoveDirection.x) >= Mathf.Abs(MoveDirection.y))
        {
            MoveDirection.x = Mathf.Abs(MoveDirection.x) > m_JoystickEffectThreshold ? Mathf.Sign(MoveDirection.x) : 0;
            MoveDirection.y = 0f;
        }
        else
        {
            MoveDirection.y = Mathf.Abs(MoveDirection.y) > m_JoystickEffectThreshold ? -Mathf.Sign(MoveDirection.y) : 0;
            MoveDirection.x = 0f;
        }
        return new Vector2Int((int)MoveDirection.y, (int)MoveDirection.x);
    }

    [SerializeField]
    private float m_JoystickEffectThreshold = .3f;
    private bool m_ControlsHeld = false;
    private InputAction.CallbackContext m_NavigationContext;
    private void MoveWhileHeld()
    {
        if (!m_ControlsHeld)
        {
            return;
        }

        Vector2Int MoveDirInt = GetMoveDirection();
        m_CellElem.MoveBy(MoveDirInt);
    }
    private void AreaNavigation_Started(InputAction.CallbackContext p_Obj)
    {
        m_ControlsHeld = true;
    }
    private void AreaNavigation_Canceled(InputAction.CallbackContext p_Obj)
    {
        m_ControlsHeld = false;
    }
    #endregion


    private void Update()
    {
        UpdateSelection();
        MoveWhileHeld();
    }
}
