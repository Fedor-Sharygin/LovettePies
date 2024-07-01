using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[Flags]
public enum UpgradeType
{
    OBJECT_PLACEMENT = 1 << 0,
    FUNCTION_CALL = 1 << 1,

    DEFAULT = 1 << 10
}

[CreateAssetMenu(fileName = "UpgradeDescriptor", menuName = "ScriptableObjects/UpgradeDescriptor", order = 3)]
public class UpgradeDescription : ScriptableObject
{
    public UpgradeType m_UpgradeType;
    public Sprite m_UI_UpgradeImage;


    public GameObject m_ObjectToPlace;
    public string m_FunctionName;
    //public object[] m_FunctionParameters;


    //This function invokes the m_FunctionName functionality
    //with passed m_FunctionParameters parameters
    //
    //Whenever you require a new upgrade function add it here
    //as a public static function inside THIS class
    //
    //If you want to invoke the function => COPY AND PASTE THE NAME
    public void InvokeFunction()
    {
        Type type = Type.GetType("UpgradeDescription");
        if (type == null)
        {
            Debug.LogError($"Class UpgradeDescription not found");
            return;
        }

        MethodInfo methodInfo = type.GetMethod(m_FunctionName, BindingFlags.Static);
        if (methodInfo == null)
        {
            Debug.LogError($"Method {m_FunctionName} not found in class UpgradeDescription");
            return;
        }

        methodInfo.Invoke(null, null);
    }
}



#region Upgrade Editor
#if UNITY_EDITOR

[CustomEditor(typeof(UpgradeDescription))]
[CanEditMultipleObjects]
public class SwitchableParametersEditor : Editor
{

}

#endif
#endregion

