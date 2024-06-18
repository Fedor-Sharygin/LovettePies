#if UNITY_EDITOR
#define DEBUG_FUNCTIONS
#endif


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
    [SerializeField]
    private SpriteRenderer m_IngredientSprite;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_CountText;
    private void Start()
    {
        PlaceOnArea();
        //m_CurIngridientCount = m_MaxIngridientCount;
        m_CurIngridientCount = IngredientStorage.Manager.GetIngredientCount(m_IngredientDescription);
        m_IngredientSprite.sprite = m_IngredientDescription?.m_IngredientSprite;
        m_CountText.text = m_CurIngridientCount.ToString();
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

        if (m_CurIngridientCount <= 0)
        {
            return;
        }

        m_CurIngridientCount--;
        m_CountText.text = m_CurIngridientCount.ToString();
        IngredientStorage.Manager.UpdateIngredientCount(m_IngredientDescription, -1);

        var IngredientInstance = GameObject.Instantiate(m_IngredientPrefab, m_IngredientSpawnPos.Socket);
        IngredientInstance.transform.localPosition = Vector3.zero;
        IngredientInstance.transform.localRotation = Quaternion.identity;
        IngredientInstance.GetComponent<IngredientContainer>().m_IngredientDescription = m_IngredientDescription;
        IngredientInstance.GetComponentInChildren<SpriteRenderer>().sprite = m_IngredientDescription.m_IngredientSprite;

        PlayerPlate.AddIngredient(IngredientInstance);
        //PlayerPlate.m_IngredientSockets[IngredientIdx].Stack(m_IngredientSpawnPos.RemoveObj());
    }

    

    #region Debug Functionality
    #if DEBUG_FUNCTIONS
    
    public void IncreaseIngredientCount()
    {
        m_CurIngridientCount++;
        m_CountText.text = m_CurIngridientCount.ToString();
        IngredientStorage.Manager.UpdateIngredientCount(m_IngredientDescription);
    }

    public void DecreaseIngredientCount()
    {
        m_CurIngridientCount--;
        m_CountText.text = m_CurIngridientCount.ToString();
        IngredientStorage.Manager.UpdateIngredientCount(m_IngredientDescription, -1);
    }
    
    #endif
    #endregion
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