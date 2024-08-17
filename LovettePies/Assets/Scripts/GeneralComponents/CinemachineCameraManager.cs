using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraManager : MonoBehaviour
{
    private static Camera m_MainCamera;
    private static bool m_Initialized = false;
    public void Initialize()
    {
        if (m_Initialized)
        {
            return;
        }

        m_MainCamera = Camera.main;
        GameObject.DontDestroyOnLoad(m_MainCamera.gameObject);

        m_Initialized = true;
    }
}
