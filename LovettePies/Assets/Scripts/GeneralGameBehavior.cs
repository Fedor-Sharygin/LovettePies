#if UNITY_EDITOR
#define DEBUG_CONTROL
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        m_Initialized = true;
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
}
