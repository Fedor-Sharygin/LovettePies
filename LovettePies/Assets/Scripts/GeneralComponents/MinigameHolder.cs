using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinigameHolder : Interactable
{
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

        foreach (var Ingred in PlayerPlate.m_DishIngredients)
        {
            if (string.IsNullOrEmpty(Ingred.m_MinigameName))
            {
                continue;
            }

            SceneManager.LoadSceneAsync(Ingred.m_MinigameName, LoadSceneMode.Additive);
            break;
        }
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