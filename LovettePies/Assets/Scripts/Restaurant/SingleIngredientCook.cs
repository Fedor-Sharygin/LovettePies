using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SingleIngredientCook : MinigameHolderBase
{
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

        Debug.LogWarning("WARNING: MINIGAME INTERACTION IS SUCCESSFUL!");
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
            m_LocalSocket.Stack(m_OriginalSocket.RemoveObj());
            SceneManager.LoadSceneAsync(m_CookingName, LoadSceneMode.Additive);
            Debug.LogWarning("WARNING: LOCKING MINIGAME HOLDER!");
            m_IsInteractable[0] = false;
            break;
        }
    }

    [Serializable]
    public struct IngredientCookResult
    {
        public string m_MinigameName;
        public IngredientDescriptor m_IngredientDescription;
    }
    public IngredientCookResult[] m_CookingResults;
    public override void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status)
    {
        if (m_OriginalSocket == null || m_LocalSocket.AvailableForStack)
        {
            return;
        }

        if (p_Status > MinigameRequiredElement.MinigameStatus.MINIGAME_FAIL)
        {
            foreach (var CookingResult in m_CookingResults)
            {
                if (CookingResult.m_MinigameName != m_CookingName)
                {
                    continue;
                }

                //m_OriginalSocket.m_DishIngredients[m_IngredIdx] = CookingResult.m_IngredientDescription;
                m_LocalSocket.PeekObj().GetComponent<IngredientContainer>().m_IngredientDescription = CookingResult.m_IngredientDescription;
                m_LocalSocket.PeekObj().GetComponentInChildren<SpriteRenderer>().sprite = CookingResult.m_IngredientDescription.m_IngredientSprite;
                break;
            }
        }

        m_OriginalSocket.Stack(m_LocalSocket.RemoveObj());
        m_IsInteractable[0] = true;
        Debug.LogWarning("WARNING: UNLOCKING MINIGAME HOLDER!");
    }
}



#region Custom SingleIngredientCook Editor
#if UNITY_EDITOR

[CustomEditor(typeof(SingleIngredientCook))]
[CanEditMultipleObjects]
class SingleIngredientCookEditor : MinigameHolderEditor
{
    private SingleIngredientCook TargetObject_SingleIngredientCook;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_SingleIngredientCook = (SingleIngredientCook)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_SingleIngredientCook == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion

