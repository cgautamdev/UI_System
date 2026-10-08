using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryItemSO[] inventory;
    public InventoryItemSO[] equippedItems;

    public List<InventoryItemSO> primaryInventoryWeapons;
    public List<InventoryItemSO> secondaryInventoryWeapons;
    public List<InventoryItemSO> throwableInventoryItems;
    public List<InventoryItemSO> armorInventoryItems;

    public void CategorizeInventoryItem()
    {
        primaryInventoryWeapons.Clear();
        secondaryInventoryWeapons.Clear();
        throwableInventoryItems.Clear();
        armorInventoryItems.Clear();

        foreach (InventoryItemSO item in inventory) {
            switch(item.itemType) {
                case InventoryItemType.PrimaryWeapon:
                    primaryInventoryWeapons.Add(item);
                    break;
                case InventoryItemType.SecondaryWeapon:
                    secondaryInventoryWeapons.Add(item);
                    break;
                case InventoryItemType.Throwable:
                    throwableInventoryItems.Add(item);
                    break;
                case InventoryItemType.Armor:
                    armorInventoryItems.Add(item);
                    break;
            }
        }
    }
}
