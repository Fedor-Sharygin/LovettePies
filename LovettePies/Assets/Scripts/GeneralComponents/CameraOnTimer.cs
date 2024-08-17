using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnTimer : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera m_CameraOnTimerElem;
    public void SwitchToMyCamera()
    {
        GeneralGameBehavior.SwitchVirtualCamera(m_CameraOnTimerElem);
    }
}
