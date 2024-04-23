using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinigameHolder : Interactable
{
    public ObjectSocket m_IngredientSocket;

    private ObjectSocket m_OriginalSocket = null;
    private int m_IngredIdx = 0;
    private string m_CookingName = string.Empty;
    public override void Interact(MonoBehaviour m_Interactee, int Direction = -1)
    {
        //base.Interact(m_Interactee, Direction);
        if (!m_Interactee.CompareTag("Player"))
        {
            return;
        }

        var PlayerComp = m_Interactee as PlayerRestaurantController;
        if (PlayerComp == null || PlayerComp.m_DishPos.AvailableForStack)
        {
            return;
        }

        var PlayerPlate = PlayerComp.m_DishPos.PeekObj().GetComponent<Plate>();
        if (PlayerPlate == null)
        {
            return;
        }

        m_IngredIdx = 0;
        foreach (var IngSock in PlayerPlate.m_IngredientSockets)
        {
            IngredientContainer Ingred;
            if (IngSock.AvailableForStack || IngSock.PeekObj() == null
                || (Ingred = IngSock.PeekObj().GetComponent<IngredientContainer>()) == null
                || string.IsNullOrEmpty(Ingred.m_IngredientDescription.m_MinigameName))
            {
                ++m_IngredIdx;
                continue;
            }

            m_CookingName = Ingred.m_IngredientDescription.m_MinigameName;
            m_OriginalSocket = PlayerPlate.m_IngredientSockets[m_IngredIdx];
            m_IngredientSocket.Stack(m_OriginalSocket.RemoveObj());
            SceneManager.LoadSceneAsync(m_CookingName, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += MinigameLoaded;
            break;
        }
    }

    private void MinigameLoaded(Scene p_MinigameScene, LoadSceneMode _p_LoadSceneMode)
    {
        //p_MinigameScene.GetRootGameObjects();

        GameObject.FindGameObjectWithTag("MinigameHolder").GetComponent<MinigameRequiredElement>().m_MinigameClosed += MinigameClosed;
    }

    [Serializable]
    public struct IngredientCookResult
    {
        public string m_MinigameName;
        public IngredientDescriptor m_IngredientDescription;
    }
    public IngredientCookResult[] m_CookingResults;
    private void MinigameClosed(bool p_Success)
    {
        if (m_OriginalSocket == null || m_IngredientSocket.AvailableForStack)
        {
            return;
        }

        if (p_Success)
        {
            foreach (var CookingResult in m_CookingResults)
            {
                if (CookingResult.m_MinigameName != m_CookingName)
                {
                    continue;
                }

                //m_OriginalSocket.m_DishIngredients[m_IngredIdx] = CookingResult.m_IngredientDescription;
                m_IngredientSocket.PeekObj().GetComponent<IngredientContainer>().m_IngredientDescription = CookingResult.m_IngredientDescription;
                m_IngredientSocket.PeekObj().GetComponentInChildren<SpriteRenderer>().sprite = CookingResult.m_IngredientDescription.m_IngredientSprite;
                break;
            }
        }

        m_OriginalSocket.Stack(m_IngredientSocket.RemoveObj());
    }
}


#region Custom MinigameHolder Editor
#if UNITY_EDITOR

[CustomEditor(typeof(MinigameHolder))]
[CanEditMultipleObjects]
class MinigameHolderEditor : InteractableEditor
{
    private MinigameHolder TargetObject_MinigameHolder;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_MinigameHolder = (MinigameHolder)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_MinigameHolder == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion