#if UNITY_EDITOR
#define DEBUG_CONTROLS
#endif



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GeneralGameBehavior
{
    public enum GameState
    {
        MENU_STATE,
        UPDGRADE_STATE,
        CUTSCENE_STATE,
        DEFAULT_GAME_STATE,
        MINI_GAME_STATE,

        DEFAULT
    }
    public static GameState m_CurState { get; private set; }

    //MUST BE CALLED ONLY FROM THE MAIN MENU
    private static bool m_Initialized = false;
    private static PlayerControls m_FullPlayerControls;
    public static PlayerControls Controls
    {
        get
        {
            return m_FullPlayerControls;
        }
    }
    public static void Initialize()
    {
        if (m_Initialized)
        {
            return;
        }


        m_FullPlayerControls = new PlayerControls();
        if (m_FullPlayerControls == null)
        {
            Debug.LogError($"Could not load Player Restaurant Controls. Quitting the game!");
            Application.Quit();
            return;
        }
        m_FullPlayerControls.Enable();

        GeneralGameBehavior.SwitchState(GameState.MENU_STATE);

        
        #if DEBUG_CONTROLS
        m_IncreaseIngredients = m_FullPlayerControls.DebugControls.AddIngredients;
        m_DecreaseIngredients = m_FullPlayerControls.DebugControls.SubtractIngredients;
        m_NavigateLevels = m_FullPlayerControls.DebugControls.NavigateLevels;
        EnableDebugInput();
        #endif

        m_Initialized = true;
    }

    private static void OnGameEnd()
    {
        #if DEBUG_CONTROLS
        DisableDebugInput();
        #endif
    }

    public static void SwitchState(GameState p_GameState)
    {
        m_CurState = p_GameState;
        switch (m_CurState)
        {
            case GameState.MENU_STATE:
                {
                    m_FullPlayerControls.BasicMinigameControls.Disable();
                    m_FullPlayerControls.UIControls.Enable();
                    m_FullPlayerControls.BasicGameControls.Disable();
                    m_FullPlayerControls.UpgradeControls.Disable();
                    m_FullPlayerControls.CutsceneControls.Disable();
                }
                break;
            case GameState.UPDGRADE_STATE:
                {
                    m_FullPlayerControls.BasicMinigameControls.Disable();
                    m_FullPlayerControls.UIControls.Disable();
                    m_FullPlayerControls.BasicGameControls.Disable();
                    m_FullPlayerControls.UpgradeControls.Enable();
                    m_FullPlayerControls.CutsceneControls.Disable();
                }
                break;
            case GameState.CUTSCENE_STATE:
                {
                    m_FullPlayerControls.BasicMinigameControls.Disable();
                    m_FullPlayerControls.UIControls.Disable();
                    m_FullPlayerControls.BasicGameControls.Disable();
                    m_FullPlayerControls.UpgradeControls.Disable();
                    m_FullPlayerControls.CutsceneControls.Enable();
                }
                break;
            case GameState.DEFAULT_GAME_STATE:
                {
                    m_FullPlayerControls.BasicMinigameControls.Disable();
                    m_FullPlayerControls.UIControls.Disable();
                    m_FullPlayerControls.BasicGameControls.Enable();
                    m_FullPlayerControls.UpgradeControls.Disable();
                    m_FullPlayerControls.CutsceneControls.Disable();
                }
                break;
            case GameState.MINI_GAME_STATE:
                {
                    m_FullPlayerControls.BasicMinigameControls.Enable();
                    m_FullPlayerControls.UIControls.Disable();
                    m_FullPlayerControls.BasicGameControls.Disable();
                    m_FullPlayerControls.UpgradeControls.Disable();
                    m_FullPlayerControls.CutsceneControls.Disable();
                }
                break;

            default:
                { }
                break;
        }
    }

    private static int m_DayCounter = 0;
    public static int CurrentDay
    {
        get
        {
            return m_DayCounter;
        }
    }
    public static void EndTheDay()
    {
        m_DayCounter++;
    }

    

    #region Debug Control
    #if DEBUG_CONTROLS
    
    private static InputAction m_IncreaseIngredients;
    private static InputAction m_DecreaseIngredients;
    private static InputAction m_NavigateLevels;
    private static void EnableDebugInput()
    {
        m_IncreaseIngredients.Enable();
        m_IncreaseIngredients.started += IncreaseAllIngredientCount;

        m_DecreaseIngredients.Enable();
        m_DecreaseIngredients.started += DecreaseAllIngredientCount;

        m_NavigateLevels.Enable();
        m_NavigateLevels.started += NavigateLevels_Started;
    }

    private static void DisableDebugInput()
    {
        m_IncreaseIngredients.started -= IncreaseAllIngredientCount;
        m_IncreaseIngredients.Disable();

        m_DecreaseIngredients.started -= DecreaseAllIngredientCount;
        m_DecreaseIngredients.Disable();

        m_NavigateLevels.started -= NavigateLevels_Started;
        m_NavigateLevels.Disable();
    }



    private static void IncreaseAllIngredientCount(InputAction.CallbackContext p_CallbackContext)
    {
        foreach (var IngHold in MonoBehaviour.FindObjectsOfType<IngridientHolder>())
        {
            IngHold.IncreaseIngredientCount();
        }
    }

    private static void DecreaseAllIngredientCount(InputAction.CallbackContext p_CallbackContext)
    {
        foreach (var IngHold in MonoBehaviour.FindObjectsOfType<IngridientHolder>())
        {
            IngHold.DecreaseIngredientCount();
        }
    }

    private static void NavigateLevels_Started(InputAction.CallbackContext obj)
    {
        var CurDir = m_NavigateLevels.ReadValue<float>();
        int NextLevelIdx = 
            (SceneManager.GetActiveScene().buildIndex + (CurDir < 0f ? -1 : (CurDir > 0f ? 1 : 0))
            + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(NextLevelIdx);
    }

#endif
    #endregion
}
