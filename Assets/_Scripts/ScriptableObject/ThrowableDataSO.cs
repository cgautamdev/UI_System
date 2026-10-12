using UnityEngine;

public enum ThrowableType
{
    lethal,
    tactical
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/Items/Throwable")]
public class ThrowableDataSO : InventoryItemSO
{
    public ThrowableType throwableType;
    // other armor data
}
