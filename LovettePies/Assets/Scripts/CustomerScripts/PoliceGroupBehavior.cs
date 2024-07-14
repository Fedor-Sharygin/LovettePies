using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoliceGroupBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform m_CriminalTransform;
    [SerializeField]
    private GameObject m_CriminalCharacter;
    public CrimeDescriptor m_CrimeDescription;

    public void ReleaseCriminal()
    {
        m_CriminalTransform.DetachChildren();
        m_CriminalCharacter.GetComponent<CellElement>().m_PositionOverhaul = false;
        var CrimCustScr = m_CriminalCharacter.GetComponent<GeneralCustomerScript>();
        if (CrimCustScr != null)
        {
            //CrimCustScr.FindPath();
            CrimCustScr.SearchForPath();
            //CrimCustScr.LeaveArea();
        }
        //m_CriminalCharacter.GetComponent<GeneralCustomerScript>()?.SearchForPath();
    }

    [SerializeField]
    private CellElement m_GroupCellElem;
    [SerializeField]
    private CellElement m_CriminalCellElem;
    private void Update()
    {
        if (m_CriminalTransform.childCount <= 0)
        {
            return;
        }
        m_CriminalCellElem.OverrideCurCellPos(m_GroupCellElem.m_CurCellPos);
    }

    public UnityEvent m_MiniGameQuitActions;
    public void DocumentEndActions()
    {
        m_MiniGameQuitActions?.Invoke();
    }
}
