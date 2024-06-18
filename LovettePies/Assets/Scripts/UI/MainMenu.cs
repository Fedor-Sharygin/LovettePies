using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        SaveLoadSystem.Manager.OnGameStart();
    }

    public void QuitGame()
    {
        SaveLoadSystem.Manager.Save();
        SaveLoadSystem.Manager.OnGameEnd();
        Application.Quit();
    }

    public void GetSaveStateReady(int p_Idx)
    {
        SaveLoadSystem.Manager.LoadSave(p_Idx);
    }
    public void StartTheGame()
    {
        SaveLoadSystem.Manager.StartGame();
    }
    public void CancelTheSave()
    {
        SaveLoadSystem.Manager.Reset();
    }
}
