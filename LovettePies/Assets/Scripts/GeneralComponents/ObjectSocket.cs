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

    private bool m_Locked = false;
    public bool IsLocked
    {
        get
        {
            return m_Locked;
        }
    }

    public bool m_DestroyObjects;

    [SerializeField]
    private float m_ObjectSpeed = 0.02f;
    private void Update()
    {
        if (AvailableForStack)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            if (Vector3.SqrMagnitude(transform.GetChild(i).localPosition) <= .0001f)
            {
                if (m_DestroyObjects)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                continue;
            }
            transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, Vector3.zero, m_ObjectSpeed);
            if (m_DestroyObjects)
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, Vector3.zero, m_ObjectSpeed);
            }
        }
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
    public void ForceStack(Transform p_StackObj)
    {
        p_StackObj.SetParent(Socket);
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

    public void Lock(bool p_Locked = true)
    {
        m_Locked = p_Locked;
    }
    public void Unlock()
    {
        Lock(false);
    }
}
