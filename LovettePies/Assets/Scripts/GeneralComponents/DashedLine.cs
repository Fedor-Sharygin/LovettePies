using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLine : MonoBehaviour
{
    private Camera m_CurCamera;
    private Material m_DashLineMaterial;
    private void Awake()
    {
        m_CurCamera = Camera.main;
        m_DashLineMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (m_CurCamera == null || m_DashLineMaterial == null)
        {
            return;
        }

        //ONLY FOR TESTING
        var MouseRay = m_CurCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit MouseHit;

        if (Physics.Raycast(MouseRay, out MouseHit, 5f))
        {
            transform.rotation = Quaternion.LookRotation(transform.parent.parent.forward, MouseHit.point - transform.position);
            transform.localScale = new Vector3(1f, 13f * Vector3.Distance(MouseHit.point, transform.position), 1f);

            if (m_DashLineMaterial.HasProperty("_Length"))
            {
                m_DashLineMaterial.SetFloat("_Length", transform.localScale.y);
            }
        }
    }
}
