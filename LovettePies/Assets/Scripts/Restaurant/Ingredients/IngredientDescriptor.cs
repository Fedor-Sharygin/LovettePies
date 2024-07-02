using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum IngredientType
{
    INGREDIENT_RAW_DOUGH,
    INGREDIENT_FLAKY_DOUGH,
    INGREDIENT_VEGGIES,
    INGREDIENT_MEAT,

    DEFAULT
}

[CreateAssetMenu(fileName = "IngredientDescriptor", menuName = "ScriptableObjects/RestaurantLevelObjects/IngredientDescriptor", order = 2)]
public class IngredientDescriptor : ScriptableObject
{
    public IngredientType m_IngredientType;

    public string m_IngredientName;
    public Sprite m_IngredientSprite;

    [Space(10)]
    public string m_MinigameName;
}
