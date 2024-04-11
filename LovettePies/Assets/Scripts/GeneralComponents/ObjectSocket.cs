using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSocket : MonoBehaviour
{
    public Transform Socket
    {
        get
        {
            return transform;
        }
    }
    public bool AvailableForStack
    {
        get
        {
            return transform.childCount <= 0;
        }
    }

    [SerializeField]
    private float m_PlateSpeed = 0.02f;
    private void Update()
    {
        if (AvailableForStack)
        {
            return;
        }

        if (Vector3.SqrMagnitude(transform.GetChild(0).localPosition) <= .0001f)
        {
            return;
        }
        transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, Vector3.zero, m_PlateSpeed);
    }

    public void Stack(Transform p_StackObj)
    {
        if (!AvailableForStack || p_StackObj == null)
        {
            return;
        }

        p_StackObj.SetParent(Socket);
        //p_StackObj.localPosition = Vector3.zero;
    }

    public Transform RemoveObj()
    {
        if (AvailableForStack)
        {
            return null;
        }

        var Ret = transform.GetChild(0);
        transform.DetachChildren();
        return Ret;
    }

    public Transform PeekObj()
    {
        if (AvailableForStack)
        {
            return null;
        }

        return transform.GetChild(0);
    }
}
