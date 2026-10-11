using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }
    public InventoryItemSO[] inventory;
    public List<InventoryItemSO> equippedItems;

    public List<InventoryItemSO> primaryInventoryWeapons;
    public List<InventoryItemSO> secondaryInventoryWeapons;
    public List<InventoryItemSO> throwableInventoryItems;
    public List<InventoryItemSO> armorInventoryItems;

    private void Awake()
    {
        Instance = this;
    }

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

    public void CheckWeaponEquipStateAndEquip(InventoryItemSO item)
    {
        if(equippedItems.Contains(item)) {
            return;
        }

        InventoryItemSO existingItemOfType = equippedItems.Find(x => x.itemType == item.itemType);

        if(existingItemOfType != null) {
            // replace item with new one
            equippedItems.Remove(existingItemOfType);
        }

        equippedItems.Add(item);

        MainPanelUIManager.Instance.UpdateMenuCard(item);

        FindFirstObjectByType<InventoryUIManager>()?.RefreshEquipButtonStates();
    }
}
