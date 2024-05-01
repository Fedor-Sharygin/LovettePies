using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantCustomerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CustomerPrefab;

    [SerializeField]
    private Vector2Int m_SpawnCellPos;

    [SerializeField]
    private IngredientDescriptor[] m_DesiredIngredients;

    public void SpawnCustomer()
    {
        var NewCustomer = GameObject.Instantiate(m_CustomerPrefab);

        if (NewCustomer == null)
        {
            return;
        }

        NewCustomer.GetComponent<Interactable>().m_StartPos = m_SpawnCellPos;
        var NewCustRestBeh = NewCustomer.GetComponent<RestaurantBehavior>();
        if (NewCustRestBeh == null)
        {
            return;
        }
        for (int i = 0; i < NewCustRestBeh.m_FoodIngredients.Length; ++i)
        {
            NewCustRestBeh.m_FoodIngredients[i] = m_DesiredIngredients[Random.Range(0, m_DesiredIngredients.Length)];
        }
    }
}
