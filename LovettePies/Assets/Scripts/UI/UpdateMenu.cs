using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateMenu : MonoBehaviour
{
    private CellElement m_CellElem;
    private void Awake()
    {
        SetupUpgrades();
        SetupInput();

        m_CellElem = GetComponent<CellElement>();
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
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.UPDGRADE_STATE);
        m_UpgradeControls = GeneralGameBehavior.Controls;

        m_OptionNavigation = m_UpgradeControls.UpgradeControls.UpgradeNavigation;
        m_PlaceElement = m_UpgradeControls.UpgradeControls.PlaceElement;
        m_QuitUpgrade = m_UpgradeControls.UpgradeControls.EndUpgrade;
    }


    private void EnableInput()
    {
        m_OptionNavigation.Enable();
        m_OptionNavigation.started += OptionNavigation_Started;
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
        m_OptionNavigation.started -= OptionNavigation_Started;
        m_OptionNavigation.canceled -= OptionNavigation_Canceled;
        m_OptionNavigation.Disable();

        m_PlaceElement.started -= PlaceElement_Started;
        m_PlaceElement.Disable();

        m_QuitUpgrade.started -= QuitUpgrade_Started;
        m_QuitUpgrade.Disable();

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
    private int m_CurSelectedChild;
    private bool m_SelectionHeld;
    private float m_CurTimeHeld = 0f;
    private int m_CurTimerIdx = 0;
    private float[] m_Timers = { 1f, .7f, .35f, .16f };
    private void OptionNavigation_Started(InputAction.CallbackContext p_Obj)
    {
        m_CurSelectedChild += 1;
        m_UIHandler?.SetSelectedGameObject(transform.GetChild(m_CurSelectedChild).gameObject);
    }
    private void OptionNavigation_Canceled(InputAction.CallbackContext p_Obj)
    {
        m_CurTimerIdx = 0;
        m_CurTimeHeld = 0f;
        m_SelectionHeld = false;
    }
    
    private void SelectNextUI()
    {
        m_CurTimeHeld = 0f;
        m_CurSelectedChild = (m_CurSelectedChild + 1) % transform.childCount;
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
        }
    }



    private void PlaceElement_Started(InputAction.CallbackContext p_Obj)
    {
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



    private void QuitUpgrade_Started(InputAction.CallbackContext p_Obj)
    {
        this.gameObject.SetActive(false);
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
