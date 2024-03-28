using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public enum EnumPlateState
    {
        PLATE_EMPTY,
        PLATE_DIRTY,
        PLATE_FULL_UNCOOKED,
        PLATE_FULL_COOKED,

        DEFAULT
    }

    public EnumPlateState m_PlateState { get; private set; } = EnumPlateState.PLATE_EMPTY;

    private int m_MaxIngredientCount = 4;
    private List<IngredientDescriptor> m_DishIngredients;

    private void Start()
    {
        m_DishIngredients = new List<IngredientDescriptor>(m_MaxIngredientCount);
    }

    public void AddIngredient(IngredientDescriptor p_NewIngredient)
    {
        m_DishIngredients.Add(p_NewIngredient);
        m_PlateState = EnumPlateState.PLATE_FULL_UNCOOKED;
    }

    public void Cook()
    {
        if (m_PlateState != EnumPlateState.PLATE_FULL_UNCOOKED)
        {
            return;
        }

        m_PlateState = EnumPlateState.PLATE_FULL_COOKED;
    }
}
