using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRestaurantController : MonoBehaviour
{
    private CellElement m_CellElement;
    private PlayerInput m_PlayerInput;
    private void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        if (m_PlayerInput == null)
        {
            Debug.LogError($"{name}: Could not load Player Input Controls. Quitting the game!");
            Application.Quit();
            return;
        }

        m_CellElement = GetComponent<CellElement>();
        if (m_CellElement == null)
        {
            Debug.LogError($"{name}: Could not load Player Cell Positioning. Quitting the game!");
            Application.Quit();
            return;
        }
    }

    private void Update()
    {
        MoveWhileHeld();
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

        Vector2 MoveDirection = m_NavigationContext.ReadValue<Vector2>();
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
        Vector2Int MoveDirInt = new Vector2Int((int)MoveDirection.y, (int)MoveDirection.x);

        m_CellElement.MoveBy(MoveDirInt);
    }
    public void NavigateArea(InputAction.CallbackContext p_CallbackContext)
    {
        if (p_CallbackContext.performed)
        {
            return;
        }

        if (p_CallbackContext.started)
        {
            m_NavigationContext = p_CallbackContext;
            m_ControlsHeld = true;
        }

        if (p_CallbackContext.canceled)
        {
            m_ControlsHeld = false;
        }
    }
}
