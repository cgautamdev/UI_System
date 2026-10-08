using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public BuyCard buyCard;

    [Header("Panels Parents")]
    public Transform primaryWeaponContent;
    public Transform secondaryWeaponContent;
    public Transform throwableContent;
    public Transform armorContent;

    private void Start()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        ClearCards(primaryWeaponContent);
        ClearCards(secondaryWeaponContent);
        ClearCards(throwableContent);
        ClearCards(armorContent);

        inventoryManager.CategorizeInventoryItem();

        PopulateCategory(inventoryManager.primaryInventoryWeapons, primaryWeaponContent);
        PopulateCategory(inventoryManager.secondaryInventoryWeapons, secondaryWeaponContent);
        PopulateCategory(inventoryManager.throwableInventoryItems, throwableContent);
        PopulateCategory(inventoryManager.armorInventoryItems, armorContent);
    }

    public void PopulateCategory(List<InventoryItemSO> items, Transform container)
    {
        foreach(InventoryItemSO item in items) {
            BuyCard card = Instantiate(buyCard, container);
            card.itemName.text = item.itemName;
            card.itemImage.sprite = item.itemSprite;
        }
    }

    public void ClearCards(Transform container)
    {
        foreach(Transform child in container) {
            Destroy(child.gameObject);
        }
    }
}
