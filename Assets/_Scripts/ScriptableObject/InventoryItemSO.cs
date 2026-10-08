using UnityEngine;

public enum InventoryItemType
{
    PrimaryWeapon,
    SecondaryWeapon,
    Throwable,
    Armor
}

public class InventoryItemSO : ScriptableObject
{
    [Header("Base Item Info")]
    public string itemName;
    public Sprite itemSprite;
    public InventoryItemType itemType;
}
