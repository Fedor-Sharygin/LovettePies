using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateHolder : Interactable
{
    [SerializeField]
    private int m_MaxPlateCount = 7;
    private Stack<Plate> m_PlatesStacked;
}
