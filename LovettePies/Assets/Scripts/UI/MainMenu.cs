using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        SaveLoadSystem.Manager.OnGameStart();
    }

    public void QuitGame()
    {
        //SaveLoadSystem.Manager.Save();
        SaveLoadSystem.Manager.OnGameEnd();
        Application.Quit();
    }

    private enum SaveGameState
    {
        DEFAULT_STATE,
        SAVE_EXISTS_STATE,
        NEW_GAME_STATE,

        DEFAULT
    }
    private SaveGameState m_State = SaveGameState.DEFAULT_STATE;

    [SerializeField]
    private UnityEvent m_OnSaveExists;
    [SerializeField]
    private UnityEvent m_OnSaveNotThere;
    private int m_CurIdx = -1;
    public void SaveButtonPressed(int p_Idx)
    {
        m_CurIdx = p_Idx;
        switch (SaveLoadSystem.Manager.DoesSaveExist(p_Idx))
        {
            case -1: //WE ARE OUTSIDE OF THE SAVE RANGE
                {
                    m_State = SaveGameState.DEFAULT_STATE;
                    m_CurIdx = -1;
                    return;
                }
            case 0: //SAVE DOES NOT EXIST => NEED TO INPUT NAME, AND CALL NEW GAME
                {
                    m_State = SaveGameState.NEW_GAME_STATE;
                    m_OnSaveNotThere?.Invoke();
                }
                break;
            case 1: //SAVE EXISTS => JUST START THE GAME THEN
                {
                    m_State = SaveGameState.SAVE_EXISTS_STATE;
                    m_OnSaveExists?.Invoke();
                }
                break;
        }
    }

    public void StartNewGame()
    {
        if (m_State != SaveGameState.NEW_GAME_STATE)
        {
            return;
        }

        SaveLoadSystem.Manager.NewGame(m_CurIdx, (Random.Range(0, int.MaxValue) % 8127341 + m_CurIdx).ToString());
    }
    public void ContinueGame()
    {
        if (m_State != SaveGameState.SAVE_EXISTS_STATE)
        {
            return;
        }

        SaveLoadSystem.Manager.LoadSave(m_CurIdx);
    }


    public void StartTheGame()
    {
        m_CurIdx = -1;
        m_State = SaveGameState.DEFAULT_STATE;
        SaveLoadSystem.Manager.StartGame();
    }
    public void CancelTheSave()
    {
        switch (m_State)
        {
            case SaveGameState.NEW_GAME_STATE:
                {
                    SaveLoadSystem.Manager.Reset();
                }
                break;
            case SaveGameState.SAVE_EXISTS_STATE:
                {
                    //something something something?
                }
                break;
        }

        m_CurIdx = -1;
        m_State = SaveGameState.DEFAULT_STATE;
    }
}
