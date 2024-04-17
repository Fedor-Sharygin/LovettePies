using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public enum EnumPlateState
    {
        PLATE_EMPTY,
        PLATE_DIRTY,
        PLATE_FULL_UNCOOKED,
        PLATE_FULL_COOKED,

        DEFAULT
    }

    public EnumPlateState m_PlateState { get; private set; } = EnumPlateState.PLATE_EMPTY;

    public List<IngredientDescriptor> m_DishIngredients;
    public ObjectSocket[] m_IngredientSockets;
    public bool AvailableForIngredients
    {
        get
        {
            return m_DishIngredients.Count < m_IngredientSockets.Length;
        }
    }

    public ObjectSocket m_DishPos;

    private void Start()
    {
        m_DishIngredients = new List<IngredientDescriptor>(m_IngredientSockets.Length);
    }

    public int AddIngredient(IngredientDescriptor p_NewIngredient)
    {
        int RetIdx = m_DishIngredients.Count;
        m_DishIngredients.Add(p_NewIngredient);
        m_PlateState = EnumPlateState.PLATE_FULL_UNCOOKED;
        return RetIdx;
    }

    public void Cook()
    {
        if (m_PlateState != EnumPlateState.PLATE_FULL_UNCOOKED)
        {
            return;
        }

        m_PlateState = EnumPlateState.PLATE_FULL_COOKED;
    }

    public void Dump(ObjectSocket p_DumpSocket = null)
    {
        m_DishIngredients.Clear();
        foreach (var IngSock in m_IngredientSockets)
        {
            var CurObj = IngSock.RemoveObj();
            if (p_DumpSocket == null)
            {
                Destroy(CurObj.gameObject);
            }
            else
            {
                p_DumpSocket.ForceStack(CurObj);
            }
            m_PlateState = EnumPlateState.PLATE_EMPTY;
        }
    }
}
