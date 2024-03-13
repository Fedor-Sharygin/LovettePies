using UnityEngine;

[CreateAssetMenu(fileName = "AreaDescriptor", menuName = "ScriptableObjects/RestaurantLevelObjects/AreaDescriptor", order = 1)]
public class AreaDescriptor : ScriptableObject
{
    [Header("Area Description")]
    public int RowCount;
    public int[] ColumnCounts;
    public int[] ConnectionRows;
}
