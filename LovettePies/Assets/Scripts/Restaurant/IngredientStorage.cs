using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class IngredientStorage
{
    [System.Serializable]
    public struct IngredientCount : System.ICloneable
    {
        public string m_IngredientName;
        public int m_Count;

        public IngredientCount(string p_IngredientName, int p_Count) : this()
        {
            m_IngredientName = p_IngredientName;
            m_Count = p_Count;
        }

        public object Clone()
        {
            return new IngredientCount(m_IngredientName, m_Count);
        }
    }

    private static IngredientStorage m_Manager;
    private IngredientStorage()
    {
        if (m_Manager != null)
        {
            return;
        }
        m_Manager = this;
        SceneManager.sceneLoaded += OnGameMenuEnter;
    }
    public static IngredientStorage Manager
    {
        get
        {
            if (m_Manager == null)
            {
                new IngredientStorage();
            }
            return m_Manager;
        }
    }

    public List<IngredientCount> m_Ingredients { get; private set; }
    private bool m_Initialized = false;

    [System.Obsolete]
    public void Initialize(string p_JsonDescription)
    {
        if (m_Initialized)
        {
            return;
        }

        //THIS FUNCTION MUST BE CALLED DURING LOADING ROUTINE
        //THE LOADING ROUTINE HOLDS THE JSON DESCRIPTION OF THE SAVE
        m_Ingredients = new List<IngredientCount>(JsonConvert.DeserializeObject<IngredientCount[]>(p_JsonDescription));
        m_Initialized = true;
    }
    public void Initialize(List<IngredientCount> p_Ingredients)
    {
        if (m_Initialized)
        {
            return;
        }

        //THIS FUNCTION MUST BE CALLED DURING LOADING ROUTINE
        //THE LOADING ROUTINE HOLDS THE JSON DESCRIPTION OF THE SAVE

        //CLONE THE LIST AND DON'T USE THE REFERENCE
        //I THINK IT CAN BREAK IF WE USE THE REFERENCE
        //OTHERWISE COULD BE IMPROVED
        m_Ingredients = p_Ingredients.DeepCopy();
        m_Initialized = true;
    }
    public void ResetState()
    {
        m_Initialized = false;
    }

    private void OnGameMenuEnter(Scene p_Scene, LoadSceneMode _p_LSM)
    {
        //CHECK THAT THE MAIN MENU IS LOADED
        //'UNINITIALIZES' THE SINGLETON FOR A NEW SAVE SINGLETON TO LOAD
        if (p_Scene.name != "MainMenuWIP")
        {
            return;
        }

        m_Initialized = false;
    }

    public int GetIngredientCount(IngredientDescriptor p_Ingred)
    {
        if (m_Ingredients == null)
        {
            m_Ingredients = new List<IngredientCount>();
        }

        foreach (var IC in m_Ingredients)
        {
            if (string.IsNullOrEmpty(IC.m_IngredientName) || IC.m_IngredientName != p_Ingred.m_IngredientName)
            {
                continue;
            }

            return IC.m_Count;
        }

        //we never met the ingredient in the array add the element to it
        m_Ingredients.Add(new IngredientCount(p_Ingred.m_IngredientName, 0));

        return 0;
    }


    public void UpdateIngredientCount(IngredientDescriptor p_Ingred, int p_Count = 1, bool p_Override = false)
    {
        if (m_Ingredients == null)
        {
            m_Ingredients = new List<IngredientCount>();
        }

        for (int i = 0; i < m_Ingredients.Count; ++i)
        {
            if (string.IsNullOrEmpty(m_Ingredients[i].m_IngredientName) || m_Ingredients[i].m_IngredientName != p_Ingred.m_IngredientName)
            {
                continue;
            }

            var TempIngred = m_Ingredients[i];
            if (p_Override)
            {
                TempIngred.m_Count = p_Count;
            }
            else
            {
                TempIngred.m_Count += p_Count;
            }
            m_Ingredients[i] = TempIngred;
            return;
        }

        //we never met the ingredient in the array add the element to it
        m_Ingredients.Add(new IngredientCount(p_Ingred.m_IngredientName, p_Count));
    }
}
