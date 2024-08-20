using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneController : MonoBehaviour
{
    private Cinemachine.CinemachineBrain m_MainCameraBrain;
    private void Awake()
    {
        GeneralGameBehavior.Initialize();
        GeneralGameBehavior.SwitchState(GeneralGameBehavior.GameState.CUTSCENE_STATE);

        m_MainCameraBrain = GeneralGameBehavior.Camera.GetComponent<Cinemachine.CinemachineBrain>();
    }

    public void SwitchToCamera(string p_CameraName)
    {
        var VirtCamObj = GameObject.Find(p_CameraName);
        if (VirtCamObj == null)
        {
            VirtCamObj = GameObject.Find($"{gameObject.name}/Cameras/{p_CameraName}");
        }
        if (VirtCamObj == null)
        {
            Debug.LogError($"Object {p_CameraName} does not exist in this context!");
            return;
        }
        var VirtCamComp = VirtCamObj.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (VirtCamComp == null)
        {
            Debug.LogError($"Object {p_CameraName} does not have a 'Cinemachine.CinemachineVirtualCamera' component!");
            return;
        }

        StartCoroutine(SwitchToCamera(VirtCamComp));
    }

    public IEnumerator SwitchToCamera(Cinemachine.CinemachineVirtualCamera p_Camera)
    {
        while (m_MainCameraBrain.IsBlending)
        {
            yield return null;
        }

        GeneralGameBehavior.SwitchVirtualCamera(p_Camera);
    }

    public void LoadGameplayLevel()
    {
        SceneManager.LoadSceneAsync("IntroGameplay", LoadSceneMode.Additive);
    }

    public void UnloadCutsceneLevel()
    {
        SceneManager.UnloadSceneAsync("IntroCutscene");
    }
}
