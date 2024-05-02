using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameRequiredElement : MonoBehaviour
{
    [Serializable]
    public enum MinigameStatus
    {
        MINIGAME_FAIL,
        MINIGAME_SUCCESS_0,
        MINIGAME_SUCCESS_1,
        MINIGAME_SUCCESS_2,
        MINIGAME_SUCCESS_3,
        MINIGAME_SUCCESS_4,
        MINIGAME_SUCCESS_5,

        DEFAULT
    }

    public delegate void MinigameClosedDelegate(MinigameStatus p_Status);
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

    [Serializable]
    public struct StatusAnimation
    {
        public MinigameStatus m_Status;
        public string m_Animation;
    }
    [SerializeField]
    private StatusAnimation[] m_EndingAnimations;
    public void PlayEndgameAnimation(MinigameStatus p_Status)
    {
        Debug.LogWarning($"WARNING: {gameObject.GetInstanceID()} OBJ IS PLAYING MINIGAME END ANIMATION!");
        m_PlayerControls.BasicMinigameControls.Disable();
        m_PlayerControls.RestaurantControls.Enable();
        if (m_EndingAnimations == null || m_EndingAnimations.Length <= 0)
        {
            GetComponent<Animator>()?.SetTrigger("EndGame");
        }
        Debug.LogWarning($"WARNING: m_MinigameClosed event has no functions - {m_MinigameClosed == null}");
        m_MinigameClosed?.Invoke(p_Status);
    }
}
