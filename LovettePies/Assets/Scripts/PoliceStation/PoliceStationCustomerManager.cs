using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceStationCustomerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CustomerPrefab;
    [SerializeField]
    private Vector2Int m_SpawnCellPos;
    public void SpawnCustomer()
    {
        var NewCustomer = GameObject.Instantiate(m_CustomerPrefab);
        if (NewCustomer == null)
        {
            return;
        }
        NewCustomer.GetComponent<Interactable>().m_StartPos = m_SpawnCellPos;

        NewCustomer.GetComponent<GeneralCustomerScript>()?.SearchForPath();
    }
}
