using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI m_TimerDisplay;
    public enum DisplayMode
    {
        FLOAT_DISPLAY,
        INT_DISPLAY,

        DEFAULT
    }
    [SerializeField]
    private DisplayMode m_TimerDisplayMode = DisplayMode.DEFAULT;

    [Space(10)]
    [SerializeField]
    private float m_TimerSeconds = 20f;
    public float m_CurTimeLeft { private set; get; }

    [SerializeField]
    private bool m_ResetOnEnd = true;
    private bool m_TimerEnded = false;
    [SerializeField]
    private bool m_StartOnAwake = true;

    [Space(10)]
    [SerializeField]
    private bool m_RandomTimer = false;
    [SerializeField]
    private float m_MinTimer = .5f;
    [SerializeField]
    private float m_MaxTimer = 20f;

    public UnityEvent m_EndActions;

    private bool m_Paused = false;

    private float m_TimerModifier = 1f;
    public float TimerModifier
    {
        set
        {
            m_TimerModifier = value;
        }
    }

    private void Awake()
    {
        //GlobalNamespace.CustomComponentNames.AddComponent(this);

        if (m_StartOnAwake)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    private void FixedUpdate()
    {
        if (m_TimerEnded || m_Paused)
        {
            return;
        }

        m_CurTimeLeft -= Time.fixedDeltaTime;
        if (m_CurTimeLeft <= 0f)
        {
            StartCoroutine("ExecuteOnEnd");
        }

        if (m_TimerDisplay == null)
        {
            return;
        }
        switch (m_TimerDisplayMode)
        {
            case DisplayMode.INT_DISPLAY:
                {
                    m_TimerDisplay.text = Mathf.Max(0, Mathf.FloorToInt(m_CurTimeLeft)).ToString();
                }
                break;
            case DisplayMode.FLOAT_DISPLAY:
                {
                    m_TimerDisplay.text = Mathf.Max(0, m_CurTimeLeft).ToString();
                }
                break;
        }
    }

    public void ResetTimer()
    {
        m_TimerEnded = false;
        m_CurTimeLeft = m_TimerSeconds;
        if (m_RandomTimer)
        {
            m_CurTimeLeft = UnityEngine.Random.Range(m_MinTimer, m_MaxTimer);
        }
        m_CurTimeLeft *= m_TimerModifier;
    }

    public void ExecuteOnEnd()
    {
        Debug.Log($"LOG: Timer [{name}] ended");
        m_TimerEnded = true;
        m_EndActions?.Invoke();
        if (m_ResetOnEnd)
        {
            ResetTimer();
        }
    }

    public void Pause()
    {
        m_Paused = true;
    }

    public void UnPause()
    {
        m_Paused = false;
    }

    public void Stop()
    {
        ResetTimer();
        Pause();
    }

    public void Play()
    {
        ResetTimer();
        UnPause();
    }

    public bool IsPlaying() => !m_Paused && !m_TimerEnded;
}


#region Custom Timer Editor
#if UNITY_EDITOR

//[CustomEditor(typeof(Timer))]
//[CanEditMultipleObjects]
//class TimerEditor : Editor
//{

//    private Timer TargetTimer;
//    private void OnEnable()
//    {
//        TargetTimer = (Timer)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        if (TargetTimer == null)
//        {
//            return;
//        }

//        Undo.RecordObject(TargetTimer, "Interactable Object");

//        TargetTimer.name = (string)EditorGUILayout.TextField("Timer Label", TargetTimer.name);

//        EditorGUILayout.Space(10);
//        DrawDefaultInspector();
//    }
//}

#endif
#endregion