using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameRequiredElement : MonoBehaviour
{
    public delegate void MinigameClosedDelegate(bool p_Success);
    public event MinigameClosedDelegate m_MinigameClosed;

    public PlayerInput m_PlayerInput;
    public PlayerControls m_PlayerControls;
    private void Awake()
    {
        Debug.LogWarning($"WARNING: {gameObject.GetInstanceID()} OBJ IS LOADING MINIGAME!!!!");
        var PlayerController = FindObjectOfType<PlayerRestaurantController>();
        m_PlayerControls = PlayerController?.m_PlayerRestaurantControls;
        if (m_PlayerControls == null)
        {
            m_PlayerControls = new PlayerControls();
        }
        if (m_PlayerControls == null)
        {
            Debug.LogError($"{name}: Could not load Player Controls. Quitting the game!");
            Application.Quit();
            return;
        }
        m_PlayerControls.Enable();
        m_PlayerControls.BasicMinigameControls.Enable();
        m_PlayerControls.RestaurantControls.Disable();

        m_PlayerInput = PlayerController?.m_PlayerInput;
        if (m_PlayerInput == null)
        {
            m_PlayerInput = GetComponent<PlayerInput>();
        }
        else
        {
            GetComponent<PlayerInput>().DeactivateInput();
        }
        if (m_PlayerInput == null)
        {
            Debug.LogError($"{name}: Could not load Player Input. Quitting the game!");
            Application.Quit();
            return;
        }
        //m_PlayerInput.SwitchCurrentActionMap("Basic Minigame Controls");

        var GameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (GameCamera == null)
        {
            return;
        }
        transform.GetComponentInParent<Canvas>().worldCamera = GameCamera.GetComponent<Camera>();
    }


    private bool m_SceneLoaded = false;
    public void RemoveAdditiveScene()
    {
        if (!m_SceneLoaded)
        {
            Debug.LogWarning("WARNING: LOADING MINIGAME IS DONE!");
            m_SceneLoaded = true;
            return;
        }

        //m_PlayerInput.SwitchCurrentActionMap("Restaurant Controls");
        //m_PlayerControls.BasicMinigameControls.Disable();
        //m_PlayerControls.RestaurantControls.Enable();
        Debug.LogWarning("WARNING: UNLOADING MINIGAME!");
        this.enabled = false;
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public void PlayEndgameAnimation(bool p_Success)
    {
        Debug.LogWarning($"WARNING: {gameObject.GetInstanceID()} OBJ IS PLAYING MINIGAME END ANIMATION!");
        m_PlayerControls.BasicMinigameControls.Disable();
        m_PlayerControls.RestaurantControls.Enable();
        GetComponent<Animator>()?.SetTrigger("EndGame");
        Debug.LogWarning($"WARNING: m_MinigameClosed event has no functions - {m_MinigameClosed == null}");
        m_MinigameClosed?.Invoke(p_Success);
    }
}
