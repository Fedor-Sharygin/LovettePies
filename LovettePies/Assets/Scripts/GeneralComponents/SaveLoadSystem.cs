using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;



[System.Serializable]
//AreaData contains information for each of the 3 main areas
//including extra-placed objects, upgrades, and cosmetics
public class AreaData
{
    [System.Serializable]
    public struct AreaObject
    {
        public int PositionX;
        public int PositionY;
        public Interactable.EnumInteractableType ObjectType;
        public GlobalNamespace.EnumMovementFlag BlockingMask;
        public bool ModelHidden;
        public object[] ExtraParams;    //optional parameters such as: Ingredient Type, Spawn Plates, etc.
    }

    //This class is missing the size of the area
    //we are ASSUMING that each area has a fixed size
    //IF we want to change it => need to add area size here
    //(better we don't)

    public string AreaName;

    public AreaObject[] Objects;

    public int LastUpgradeDay;  //determines what day the last Upgrade state was saved
    public string[] Upgrades;   //contains names of upgrade function calls for the CURRENT day
    public string[] Cosmetics;  //no idea yet. just a placeholder. can and SHOULD be changed
}


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

    public AreaData m_RestaurantArea;
    public AreaData m_BarberArea;
    public AreaData m_PoliceArea;   //MIGHT CHANGE BECAUSE POLICEMAN HAS A DIFFERENT GAMEPLAY LOOP
                                    //POSSIBLY EVEN MULTIPLE OF THEM
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


    private enum LoadState
    {
        DEFAULT_STATE = 0,
        NEW_GAME_STATE,
        LOAD_GAME_STATE,

        DEFAULT
    }
    private LoadState m_State = LoadState.DEFAULT_STATE;

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
        m_GameData = JsonConvert.DeserializeObject<GameData>(JsonDesc);
        for (int i = 0; i < m_GameData.m_SaveFiles.Length; ++i)
        {
            var FullPath = GetFullPath(m_GameData.m_SaveFiles[i] + ".json");
            if (string.IsNullOrEmpty(m_GameData.m_SaveFiles[i]) || !File.Exists(FullPath))
            {
                m_Saves[i] = new SaveData();
                continue;
            }
            var SaveInfo = File.ReadAllText(FullPath);
            m_Saves[i] = JsonConvert.DeserializeObject<SaveData>(SaveInfo);
        }
    }
    public void OnGameEnd()
    {
        //SAVE EVERYTHING(?)
        //File.WriteAllText(GameDataPath, JsonUtility.ToJson(m_GameData, true));
        //for (int i = 0; i < m_GameData.m_SaveFiles.Length; ++i)
        //{
        //    var FullPath = GetFullPath(m_GameData.m_SaveFiles[i] + ".json");
        //    if (string.IsNullOrEmpty(m_GameData.m_SaveFiles[i]))
        //    {
        //        return;
        //    }
        //    File.WriteAllText(FullPath, JsonUtility.ToJson(m_Saves[i], true));
        //}
    }


    private static int m_CurSaveIdx = -1;
    public int DoesSaveExist(int p_Idx) => (p_Idx < 0 || p_Idx >= m_GameData.m_SaveFiles.Length) ?
                                                -1 : (
                                                (string.IsNullOrEmpty(m_GameData.m_SaveFiles[p_Idx]) || !m_Saves[p_Idx].m_SaveExists) ?
                                                    0 :
                                                    1
                                            );

    //MUST BE ONLY CALLED WHEN THE SAVE EXISTS
    public void LoadSave(int p_Idx)
    {
        m_CurSaveIdx = p_Idx;

        IngredientStorage.Manager.Initialize(m_Saves[m_CurSaveIdx].m_Ingredients);
    }
    public void NewGame(int p_Idx, string p_SaveName)
    {
        m_CurSaveIdx = p_Idx;

        m_GameData.m_SaveFiles[m_CurSaveIdx] = p_SaveName;
        m_Saves[m_CurSaveIdx].m_SaveName = p_SaveName;
        m_Saves[m_CurSaveIdx].m_SaveExists = true;
        
        IngredientStorage.Manager.Initialize(new List<IngredientStorage.IngredientCount>());
    }


    [System.Obsolete]
    public void LoadSave_Old(int p_Idx)
    {
        //if (m_State != LoadState.DEFAULT_STATE)
        //{
        //    return;
        //}

        if (p_Idx >= m_Saves.Length)
        {
            return;
        }
        if (m_Saves[p_Idx].m_SaveExists == false)
        {
            m_State = LoadState.NEW_GAME_STATE;
            //NewGame(p_Idx);
            return;
        }

        IngredientStorage.Manager.Initialize(m_Saves[p_Idx].m_Ingredients);
        m_CurSaveIdx = p_Idx;
        //DO THE REST OF GAME INITIALIZATION HERE
    }
    [System.Obsolete]
    public void NewGame_Old(int p_Idx)
    {
        //if (m_State != LoadState.NEW_GAME_STATE)
        //{
        //    return;
        //}

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

        AreaData CurData = Area.GetCurAreaDescription();
        if ((m_Saves[m_CurSaveIdx].m_RestaurantArea != null && m_Saves[m_CurSaveIdx].m_RestaurantArea.AreaName == Area.CurAreaName)
            || Area.CurAreaName == "Restaurant")
        {
            m_Saves[m_CurSaveIdx].m_RestaurantArea = CurData;
        }
        if ((m_Saves[m_CurSaveIdx].m_BarberArea != null && m_Saves[m_CurSaveIdx].m_BarberArea.AreaName == Area.CurAreaName)
            || Area.CurAreaName == "Barber")
        {
            m_Saves[m_CurSaveIdx].m_BarberArea = CurData;
        }
        if ((m_Saves[m_CurSaveIdx].m_PoliceArea != null && m_Saves[m_CurSaveIdx].m_PoliceArea.AreaName == Area.CurAreaName)
            || Area.CurAreaName == "Police")
        {
            m_Saves[m_CurSaveIdx].m_PoliceArea = CurData;
        }

        var FullPath = GetFullPath(m_GameData.m_SaveFiles[m_CurSaveIdx] + ".json");
        File.WriteAllText(FullPath, JsonConvert.SerializeObject(m_Saves[m_CurSaveIdx], Formatting.Indented));
        File.WriteAllText(GameDataPath, JsonConvert.SerializeObject(m_GameData, Formatting.Indented));
    }


    //private string m_DefaultRestaurantDataPath = "DefaultRestaurant.json";
    private AreaData m_DefaultRestaurantData
    {
        get
        {
            var JsonDesc = File.ReadAllText(GetFullPath("DefaultRestaurant.json"));
            if (string.IsNullOrEmpty(JsonDesc))
            {
                return null;
            }
            AreaData FullData = JsonConvert.DeserializeObject<AreaData>(JsonDesc);
            return FullData;
        }
    }
    
    //private string m_DefaultBarbershopDataPath = "DefaultBarber.json";
    private AreaData m_DefaultBarbershopData
    {
        get
        {
            var JsonDesc = File.ReadAllText(GetFullPath("DefaultBarber.json"));
            if (string.IsNullOrEmpty(JsonDesc))
            {
                return null;
            }
            AreaData FullData = JsonConvert.DeserializeObject<AreaData>(JsonDesc);
            return FullData;
        }
    }
    
    //private string m_DefaultPoliceDataPath = "DefaultPolice.json";
    private AreaData m_DefaultPoliceData
    {
        get
        {
            var JsonDesc = File.ReadAllText(GetFullPath("DefaultPolice.json"));
            if (string.IsNullOrEmpty(JsonDesc))
            {
                return null;
            }
            AreaData FullData = JsonConvert.DeserializeObject<AreaData>(JsonDesc);
            return FullData;
        }
    }

    public AreaData GetAreaDescription(string p_AreaName)
    {
        if (m_CurSaveIdx < 0 || m_CurSaveIdx >= m_Saves.Length)
        {
            return null;
        }


        //Check if the area exists in the current save
        if (m_Saves[m_CurSaveIdx].m_RestaurantArea != null && m_Saves[m_CurSaveIdx].m_RestaurantArea.AreaName == p_AreaName)
        {
            return m_Saves[m_CurSaveIdx].m_RestaurantArea;
        }
        if (m_Saves[m_CurSaveIdx].m_BarberArea != null && m_Saves[m_CurSaveIdx].m_BarberArea.AreaName == p_AreaName)
        {
            return m_Saves[m_CurSaveIdx].m_BarberArea;
        }
        if (m_Saves[m_CurSaveIdx].m_PoliceArea != null && m_Saves[m_CurSaveIdx].m_PoliceArea.AreaName == p_AreaName)
        {
            return m_Saves[m_CurSaveIdx].m_PoliceArea;
        }


        //Check if the area asks for a default data
        if (m_DefaultRestaurantData != null && (m_DefaultRestaurantData.AreaName == p_AreaName || p_AreaName == "Restaurant"))
        {
            return m_DefaultRestaurantData;
        }
        if (m_DefaultBarbershopData != null && (m_DefaultBarbershopData.AreaName == p_AreaName || p_AreaName == "Barber"))
        {
            return m_DefaultBarbershopData;
        }
        if (m_DefaultPoliceData != null && (m_DefaultPoliceData.AreaName == p_AreaName || p_AreaName == "Police"))
        {
            return m_DefaultPoliceData;
        }


        //WTF? Should not be possible
        return null;
    }
}
