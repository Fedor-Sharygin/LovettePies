using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class MinigameHolderBase : Interactable
{
    public ObjectSocket m_LocalSocket;
    private void Awake()
    {
        SceneManager.sceneLoaded += MinigameLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= MinigameLoaded;
    }

    protected bool m_CurrentActivated = false;
    protected virtual void MinigameLoaded(Scene p_MinigameScene, LoadSceneMode _p_LoadSceneMode)
    {
        if (!m_CurrentActivated || p_MinigameScene.name == "OneAreaNavigationTest")
        {
            return;
        }

        m_CurrentActivated = false;
        //p_MinigameScene.GetRootGameObjects();
        Debug.LogWarning($"WARNING: {p_MinigameScene.name} LOADED AND TRIGGERED FUNCTION!");
        foreach (var MGHolder in GameObject.FindGameObjectsWithTag("MinigameHolder"))
        {
            MGHolder.GetComponent<MinigameRequiredElement>().m_MinigameClosed += MinigameClosed;
        }
    }
    public abstract void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status);
}


#region Custom MinigameHolder Editor
#if UNITY_EDITOR

[CustomEditor(typeof(MinigameHolderBase))]
[CanEditMultipleObjects]
class MinigameHolderEditor : InteractableEditor
{
    private MinigameHolderBase TargetObject_MinigameHolder;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_MinigameHolder = (MinigameHolderBase)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_MinigameHolder == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion

