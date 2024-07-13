using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
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
    //This Function is a requirement
    //allows Area to invoke a list of saved Upgrade Functions
    //when reloading a save file, which was closed during the day
    public static List<string> CurrentUpgradesList { get; private set; } = new List<string>();
    public static void InvokeFunction(string p_FunctionName)
    {
        Type type = Type.GetType("UpgradeDescription");
        if (type == null)
        {
            Debug.LogError($"Class UpgradeDescription not found");
            return;
        }

        MethodInfo methodInfo = type.GetMethod(p_FunctionName, BindingFlags.Static);
        if (methodInfo == null)
        {
            Debug.LogError($"Method {p_FunctionName} not found in class UpgradeDescription");
            return;
        }

        methodInfo.Invoke(null, null);
        CurrentUpgradesList.Add(p_FunctionName);
    }


    public UpgradeType m_UpgradeType;
    public Sprite m_UI_UpgradeImage;


    public GameObject m_ObjectToPlace;
    public AreaData.AreaObject m_ObjectDescription; //IGNORE THE POSITION PARAMS
                                                    //FIND A WAY TO ADD EXTRA PARAMS
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
        UpgradeDescription.InvokeFunction(m_FunctionName);
    }
}



#region Upgrade Editor
#if UNITY_EDITOR

[CustomEditor(typeof(UpgradeDescription))]
[CanEditMultipleObjects]
public class UpgradeEditor : Editor
{

}

#endif
#endregion

