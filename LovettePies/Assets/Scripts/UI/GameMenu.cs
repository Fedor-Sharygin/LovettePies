using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private PlayerControls m_PlayerControls;
    private PlayerInput m_PlayerInput;
    private InputAction m_ResumeAction;
    private void Awake()
    {
        //var PlayerController = FindObjectOfType<PlayerRestaurantController>();
        //m_PlayerControls = PlayerController?.m_PlayerRestaurantControls;
        //if (m_PlayerControls == null)
        //{
        //    m_PlayerControls = new PlayerControls();
        //}
        //if (m_PlayerControls == null)
        //{
        //    Debug.LogError($"{name}: Could not load Player Controls. Quitting the game!");
        //    Application.Quit();
        //    return;
        //}
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.MENU_STATE);
        m_PlayerControls = GeneralGameBehavior.Controls;


        m_ResumeAction = m_PlayerControls.UIControls.Resume;
    }

    private float m_CurSavedTimeScale;
    private float m_CurSavedFixedDeltaTime;
    private void PauseGame()
    {
        //m_PlayerControls.Enable();
        //m_PlayerControls.BasicMinigameControls.Disable();
        //m_PlayerControls.UIControls.Enable();
        //m_PlayerControls.BasicGameControls.Disable();

        m_ResumeAction.Enable();
        m_ResumeAction.started += ResumeAction_Started;


        m_CurSavedTimeScale = Time.timeScale;
        m_CurSavedFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    private void ResumeGame()
    {
        //BIG ASSUMPTION THAT WE CAN ONLY OPEN THIS MENU
        //WHEN USER IS NOT PLAYING A MINIGAME
        //m_PlayerControls.Enable();
        //m_PlayerControls.BasicMinigameControls.Disable();
        //m_PlayerControls.UIControls.Disable();
        //m_PlayerControls.BasicGameControls.Enable();
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.DEFAULT_GAME_STATE);

        m_ResumeAction.started -= ResumeAction_Started;
        m_ResumeAction.Disable();


        Time.timeScale = m_CurSavedTimeScale;
        Time.fixedDeltaTime = m_CurSavedFixedDeltaTime;
    }

    private void OnEnable()
    {
        PauseGame();
    }
    private void OnDisable()
    {
        ResumeGame();
    }

    public void Save()
    {
        SaveLoadSystem.Manager.Save();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenuWIP");
    }


    private void ResumeAction_Started(InputAction.CallbackContext p_Obj)
    {
        gameObject.SetActive(false);
        ResumeGame();
    }
}
