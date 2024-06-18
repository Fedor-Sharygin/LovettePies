using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    //perhaps have this name appear as a customer name?
    //would give a reason for a silly name (no restrictions, since we are not making an online game)
    public string m_SaveName;
    public bool m_SaveExists;

    //CURRENT PLAN: WEEKLY ROTATION LOOP:
    //  5 days in restaurant (main gameplay loop of cooking and serving food)
    //  1 day  in barbershop (cut hair and collect murders for restaurant ingredients)
    //  1 day  police investigation (go after a lead to collect information about murders)
    public uint m_CurDay;
    //will be used to determine the story state - flag
    //  police choice - 1 bit positive val
    //  murderer choice - 0 bit negative val
    public uint m_ChoiceFlag;
    public uint m_BarberLevel;
    public uint m_PoliceLevel;
    //Sum = police level - murderer/barber level + points(m_ChoiceFlag)
    //public  int m_StoryValue;

    public List<IngredientStorage.IngredientCount> m_Ingredients;
    //public Barber upgrades
    //public Police upgrades
}

[System.Serializable]
public class GameData
{
    public string[] m_SaveFiles;
}

public class SaveLoadSystem
{
    private static SaveLoadSystem m_Manager;
    private SaveLoadSystem()
    {
        if (m_Manager != null)
        {
            return;
        }
        m_Manager = this;
    }
    public static SaveLoadSystem Manager
    {
        get
        {
            if (m_Manager == null)
            {
                new SaveLoadSystem();
            }
            return m_Manager;
        }
    }

    private static string m_SaveFileName = "TestSaveFile.json";
    private static string GameDataPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, m_SaveFileName);
        }
    }
    private static string GetFullPath(string p_FileName) => Path.Combine(Application.persistentDataPath, p_FileName);

    private GameData m_GameData;
    private SaveData[] m_Saves = new SaveData[3];
    public void OnGameStart()
    {
        //if (!File.Exists(Application.persistentDataPath))
        //{
        //    File.Create(Application.persistentDataPath);
        //}
        if (!File.Exists(GameDataPath))
        {
            m_GameData = new GameData();
            m_GameData.m_SaveFiles = new string[3];
            for (int i = 0; i < m_GameData.m_SaveFiles.Length; ++i)
            {
                m_Saves[i] = new SaveData();
            }
            return;
        }

        var JsonDesc = File.ReadAllText(GameDataPath);
        m_GameData = JsonUtility.FromJson<GameData>(JsonDesc);
        for (int i = 0; i < m_GameData.m_SaveFiles.Length; ++i)
        {
            var FullPath = GetFullPath(m_GameData.m_SaveFiles[i] + ".json");
            if (string.IsNullOrEmpty(m_GameData.m_SaveFiles[i]) || !File.Exists(FullPath))
            {
                m_Saves[i] = new SaveData();
                continue;
            }
            var SaveInfo = File.ReadAllText(FullPath);
            m_Saves[i] = JsonUtility.FromJson<SaveData>(SaveInfo);
        }
    }
    public void OnGameEnd()
    {
        //SAVE EVERYTHING
        File.WriteAllText(GameDataPath, JsonUtility.ToJson(m_GameData, true));
        for (int i = 0; i < m_GameData.m_SaveFiles.Length; ++i)
        {
            var FullPath = GetFullPath(m_GameData.m_SaveFiles[i] + ".json");
            if (string.IsNullOrEmpty(m_GameData.m_SaveFiles[i]))
            {
                return;
            }
            File.WriteAllText(FullPath, JsonUtility.ToJson(m_Saves[i], true));
        }
    }


    private static int m_CurSaveIdx = -1;
    public void LoadSave(int p_Idx)
    {
        if (p_Idx >= m_Saves.Length)
        {
            return;
        }
        if (m_Saves[p_Idx].m_SaveExists == false)
        {
            NewGame(p_Idx);
            return;
        }

        IngredientStorage.Manager.Initialize(m_Saves[p_Idx].m_Ingredients);
        m_CurSaveIdx = p_Idx;
        //DO THE REST OF GAME INITIALIZATION HERE
    }
    public void NewGame(int p_Idx)
    {
        if (p_Idx >= m_Saves.Length)
        {
            return;
        }

        //Enter a state of waiting for input
        string Name = "BlaBlaBla";
        m_GameData.m_SaveFiles[p_Idx] = Name;
        m_Saves[p_Idx].m_SaveName = Name;
        m_Saves[p_Idx].m_SaveExists = true;
        IngredientStorage.Manager.Initialize(new List<IngredientStorage.IngredientCount>());
        m_CurSaveIdx = p_Idx;
    }
    public void StartGame()
    {
        //parse current info and load a level with specific parameters
        SceneManager.LoadScene("OneAreaNavigationTest");
    }
    public void Reset()
    {
        m_Saves[m_CurSaveIdx].m_SaveExists = false;
        IngredientStorage.Manager.ResetState();
    }

    //Perhaps force a save whenever a story choice is made?
    //could create an incentive to replay the game
    public void Save()
    {
        //PROCESS ALL OF THE CURRENT GAME INFO
        //JsonUtility.ToJson for each serializable element

        m_Saves[m_CurSaveIdx].m_SaveExists = true;
        m_Saves[m_CurSaveIdx].m_Ingredients = IngredientStorage.Manager.m_Ingredients.DeepCopy();

        var FullPath = GetFullPath(m_Saves[m_CurSaveIdx].m_SaveName + ".json");
        File.WriteAllText(FullPath, JsonUtility.ToJson(m_Saves[m_CurSaveIdx], true));
    }
}
