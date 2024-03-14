using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCustomerScript : MonoBehaviour
{
    private CellElement m_CellPos;
    private void Start()
    {
        m_CellPos = GetComponent<CellElement>();
    }

    [SerializeField]
    private Timer m_MovementTimer;
    [SerializeField]
    private Timer m_EatingTimer;
}
