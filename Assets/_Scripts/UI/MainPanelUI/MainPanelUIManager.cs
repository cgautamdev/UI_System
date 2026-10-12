using System.Collections.Generic;
using UnityEngine;

public class MainPanelUIManager : MonoBehaviour
{
    public static MainPanelUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InventoryTabGroup tabGroup;

    [Header("Main Menu Cards")]
    [SerializeField] private List<MainCard> cards = new List<MainCard>();

    private void Awake()
    {
        if(Instance != null && Instance != this) {
            Debug.LogWarning(
                "Multiple MainPanelUIManager instances detected. " +
                "Destroying the duplicate.",
                this);

            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }

    public void OpenInventoryWithCategory(InventoryItemType cardType)
    {
        if(!ValidateUIReferences())
            return;

        InventoryUIManager inventoryUI = uiManager.InventoryUIManager;

        inventoryUI.ResetPrimaryWeaponFilter();
        inventoryUI.ResetSecondaryWeaponFilter();
        inventoryUI.ResetThrowableWeaponFilter();

        tabGroup.OnTabSelectedFromCard(cardType);

        uiManager.MainPanel.SetActive(false);
        uiManager.InventoryPanel.SetActive(true);
    }

    public void UpdateMenuCard(InventoryItemSO item)
    {
        if(item == null) {
            Debug.LogWarning(
                "Cannot update main menu card: item is null.",
                this);
            return;
        }

        if(cards == null) {
            Debug.LogError("Main menu card list is not assigned.", this);
            return;
        }

        foreach(MainCard card in cards) {
            if(card == null || card.CardType != item.itemType)
                continue;

            card.SetMenuCard(item);

            // Refresh attachment icons from runtime state when updating
            // a weapon card, so stale icons are cleared as well.
            if(item is WeaponDataSO)
                UpdateMenuCardIcons(item);

            return;
        }

        Debug.LogWarning(
            $"No main menu card found for item type '{item.itemType}'.",
            this);
    }

    public void UpdateMenuCardIcons(InventoryItemSO item)
    {
        if(item == null) {
            Debug.LogWarning(
                "Cannot update attachment icons: item is null.",
                this);
            return;
        }

        if(item is not WeaponDataSO weapon)
            return;

        if(cards == null) {
            Debug.LogError("Main menu card list is not assigned.", this);
            return;
        }

        InventoryManager inventory = InventoryManager.Instance;

        if(inventory == null) {
            Debug.LogError(
                "Cannot update attachment icons: InventoryManager.Instance " +
                "is missing.",
                this);
            return;
        }

        // Attachments come exclusively from runtime memory.
        List<WeaponAttachmentDataSO> attachments =
            inventory.GetEquippedAttachments(weapon);

        foreach(MainCard card in cards) {
            if(card == null || card.CardType != item.itemType)
                continue;

            card.UpdateAttachmentIcons(attachments);
            return;
        }

        Debug.LogWarning(
            $"No main menu card found for weapon type '{item.itemType}'.",
            this);
    }

    private bool ValidateUIReferences()
    {
        if(uiManager == null) {
            Debug.LogError("UIManager reference is missing.", this);
            return false;
        }

        if(uiManager.InventoryUIManager == null) {
            Debug.LogError(
                "InventoryUIManager reference is missing from UIManager.",
                this);
            return false;
        }

        if(uiManager.MainPanel == null ||
            uiManager.InventoryPanel == null) {
            Debug.LogError(
                "Main panel or inventory panel reference is missing.",
                this);
            return false;
        }

        if(tabGroup == null) {
            Debug.LogError("InventoryTabGroup reference is missing.", this);
            return false;
        }

        return true;
    }
}