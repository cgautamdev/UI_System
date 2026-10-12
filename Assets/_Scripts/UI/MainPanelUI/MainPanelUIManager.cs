using System;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelUIManager : MonoBehaviour
{
    public static MainPanelUIManager Instance;

    public UIManager uiManager;
    public InventoryTabGroup tabGroup;

    public List<MainCard> cards;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenInventoryWithCategory(InventoryItemType cardType)
    {
        uiManager.mainPanel.SetActive(false);
        uiManager.inventoryPanel.SetActive(true);

        uiManager.inventoryUIManager.ResetPrimaryWeaponFilter();

        tabGroup.OnTabSelectedFromCard(cardType);
    }

    public void UpdateMenuCard(InventoryItemSO item)
    {
        foreach (MainCard card in cards) {
            if(card.cardType != item.itemType) {
                continue;
            }

            card.SetMenuCard(item);
            return;
        }
    }

    public void UpdateMenuCardIcons(InventoryItemSO item)
    {
        List<WeaponAttachmentDataSO> attachments = item.itemType switch {
            InventoryItemType.PrimaryWeapon => InventoryManager.Instance.primaryWeaponAttachments,
            InventoryItemType.SecondaryWeapon => InventoryManager.Instance.secondaryWeaponAttachments,
            _ => null
        };

        if(attachments == null)
            return;

        foreach(MainCard card in cards) {
            if(card.cardType != item.itemType)
                continue;

            card.UpdateAttachmentIcons(attachments);
            return;
        }
    }
}
