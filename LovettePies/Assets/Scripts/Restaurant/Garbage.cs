using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Garbage : Interactable
{
    [Space(10)]
    public ObjectSocket m_GarbageSocket;

    public override void Interact(MonoBehaviour m_Interactee, int Direction = -1)
    {
        //base.Interact(m_Interactee, Direction);

        switch (m_Interactee.tag)
        {
            case "Player":
                {
                    var PlayerScript = m_Interactee as PlayerRestaurantController;
                    if (PlayerScript == null || PlayerScript.m_DishPos.AvailableForStack)
                    {
                        break;
                    }

                    var Plate = PlayerScript.m_DishPos.PeekObj().GetComponent<Plate>();
                    if (Plate == null)
                    {
                        break;
                    }

                    Plate.Dump(m_GarbageSocket);
                }
                break;
        }
    }
}

#region Custom Garbage Editor
#if UNITY_EDITOR

[CustomEditor(typeof(Garbage))]
[CanEditMultipleObjects]
class GarbageEditor : InteractableEditor
{
    private Garbage TargetObject_Garbage;
    protected override void CustomOnEnable()
    {
        base.CustomOnEnable();
        TargetObject_Garbage = (Garbage)target;
    }

    private void OnEnable()
    {
        CustomOnEnable();
    }

    public override void OnInspectorGUI()
    {
        if (TargetObject_Garbage == null)
        {
            return;
        }

        base.OnInspectorGUI();
    }
}

#endif
#endregion