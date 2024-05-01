using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarbershopBehavior : MonoBehaviour
{
    //NEED TO COME UP WITH A WAY TO DEFINE HAIRCUTS
    //2 POSSIBLE OPTIONS:
    //A SET OF RANDOMLY GENERATED INTERPOLATED BEZIER CURVE POINTS
    //RANDOMLY CHOSEN MANUALLY DEFINED HAIRCUT

    public void AtSeat()
    {
        //DO SOMETHING WHEN SITTING DOWN AND STARTING PATIENCE TIMER
    }

    [SerializeField]
    private Timer m_PatienceTimer;
    public void HaircutBegins()
    {
        m_PatienceTimer.Stop();
    }

    [SerializeField]
    private UnityEvent m_HaircutReceivedActions;
    public void HaircutEnds_Success()
    {
        m_HaircutReceivedActions?.Invoke();
    }

    [SerializeField]
    private IngredientDescriptor m_CustomerIngredient;
    public void HaircutEnds_Fail()
    {
        //PASS INGREDIENTS TO THE RESTAURANT
    }
}
