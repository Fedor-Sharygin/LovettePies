using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEnterDelegation : MonoBehaviour
{
    public delegate void TriggerDelegate(Collider2D p_Trigger, Collider2D p_Other);
    public event TriggerDelegate OnTriggerEntered;

    private void OnTriggerEnter2D(Collider2D p_Other)
    {
        OnTriggerEntered?.Invoke(GetComponent<Collider2D>(), p_Other);
    }
}
