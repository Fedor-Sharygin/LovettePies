using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRestaurantController : MonoBehaviour
{
    [SerializeField]
    private int m_OriginalArea;
    [SerializeField]
    private Vector2Int m_OriginalPosition;

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
        m_CellElement.AreaIdx = m_OriginalArea;
        m_CellElement.MoveTo(m_OriginalPosition);
    }

    private void Update()
    {
        MoveWhileHeld();
    }

    private Vector2Int GetMoveDirection()
    {
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


    public ObjectSocket m_DishPos;
    public void Interact(InputAction.CallbackContext p_CallbackContext)
    {
        if (!p_CallbackContext.started)
        {
            return;
        }

        //foreach (var InteractObject in m_CellElement.GetNeighbors())
        //{
        //    if (InteractObject == null)
        //    {
        //        continue;
        //    }

        //    if (InteractObject.IsInteractable(this.tag))
        //    {
        //        InteractObject.Interact(this);
        //        break;
        //    }
        //}

        Vector2Int MoveDirInt = GetMoveDirection();
        var InteractObject = m_CellElement.GetNeighbor(MoveDirInt);

        if (InteractObject == null)
        {
            return;
        }
        if (!InteractObject.IsInteractable(this.tag, GlobalNamespace.GeneralFunctions.GetDirection(MoveDirInt)))
        {
            return;
        }
        InteractObject.Interact(this, GlobalNamespace.GeneralFunctions.GetDirection(MoveDirInt));

        if (InteractObject is PlateHolder &&
           (InteractObject.InteractableType == Interactable.EnumInteractableType.RESTAURANT_PLATE_HOLDER))
        {
            PlateHolder CurHolder = InteractObject as PlateHolder;
            if (m_DishPos.AvailableForStack)
            {
                var TopPlate = CurHolder.GrabPlate();
                if (TopPlate != null)
                {
                    TopPlate.transform.SetParent(m_DishPos.Socket);
                }
            }
            else
            {
                Plate MyPlate = m_DishPos.Socket.GetChild(0).gameObject.GetComponent<Plate>();
                if (MyPlate != null)
                {
                    CurHolder.StackPlate(MyPlate);
                }
            }
        }
    }
}
