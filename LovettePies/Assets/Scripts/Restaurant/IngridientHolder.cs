using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class IngridientHolder : Interactable
{
    [Space(10)]
    [SerializeField]
    private int m_MaxIngridientCount = 100;
    private int m_CurIngridientCount;
    private void Start()
    {
        PlaceOnArea();
        m_CurIngridientCount = m_MaxIngridientCount;
    }


    [SerializeField]
    private IngredientDescriptor m_IngredientDescription;
    [SerializeField]
    private GameObject m_IngredientPrefab;

    public ObjectSocket m_IngredientSpawnPos;
    public void SpawnIngridient(PlayerRestaurantController p_PlayerController)
    {
        if (p_PlayerController.m_DishPos.AvailableForStack)
        {
            return;
        }

        var PlayerPlate = p_PlayerController.m_DishPos.Socket.GetChild(0).GetComponent<Plate>();
        if (!PlayerPlate.AvailableForIngredients)
        {
            return;
        }

        var IngredientInstance = GameObject.Instantiate(m_IngredientPrefab, m_IngredientSpawnPos.Socket);
        IngredientInstance.transform.localPosition = Vector3.zero;
        IngredientInstance.transform.localRotation = Quaternion.identity;
        IngredientInstance.GetComponentInChildren<SpriteRenderer>().sprite = m_IngredientDescription.m_IngredientSprite;

        int IngredientIdx = PlayerPlate.AddIngredient(m_IngredientDescription);
        PlayerPlate.m_IngredientSockets[IngredientIdx].Stack(m_IngredientSpawnPos.RemoveObj());
    }
}


#region Custom PlateHolder Editor
#if UNITY_EDITOR

[CustomEditor(typeof(IngridientHolder))]
[CanEditMultipleObjects]
class IngridientHolderEditor : InteractableEditor
{
    private IngridientHolder TargetObject_IngridientHolder;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_IngridientHolder = (IngridientHolder)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_IngridientHolder == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion