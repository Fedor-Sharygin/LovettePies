using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDescriptor", menuName = "ScriptableObjects/RestaurantLevelObjects/IngredientDescriptor", order = 2)]
public class IngredientDescriptor : ScriptableObject
{
    public string m_IngredientName;
    public Sprite m_IngredientSprite;

    [Space(10)]
    public string m_MinigameName;
}
