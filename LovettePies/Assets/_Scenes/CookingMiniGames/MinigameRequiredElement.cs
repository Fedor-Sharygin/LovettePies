using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameRequiredElement : MonoBehaviour
{
    public PlayerInput m_PlayerInput;
    public PlayerControls m_PlayerControls;
    private void Awake()
    {
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
            m_SceneLoaded = true;
            return;
        }

        //m_PlayerInput.SwitchCurrentActionMap("Restaurant Controls");
        //m_PlayerControls.BasicMinigameControls.Disable();
        //m_PlayerControls.RestaurantControls.Enable();
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public void PlayEndgameAnimation()
    {
        m_PlayerControls.BasicMinigameControls.Disable();
        m_PlayerControls.RestaurantControls.Enable();
        GetComponent<Animator>()?.SetTrigger("EndGame");
    }
}
