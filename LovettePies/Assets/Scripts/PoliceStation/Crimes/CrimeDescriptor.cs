using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrimeDescriptor", menuName = "ScriptableObjects/PoliceStationObjects/CrimeDescriptor", order = 1)]
public class CrimeDescriptor : ScriptableObject
{
    public string m_CrimeName;
    public string m_CriminalName;
    public string m_PolicemanName;
}
