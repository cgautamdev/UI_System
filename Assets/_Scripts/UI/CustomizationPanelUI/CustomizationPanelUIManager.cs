using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomizationPanelUIManager : MonoBehaviour
{
    [Header("Weapon Preview")]
    [SerializeField] private Transform weaponPreviewContainer;
    [SerializeField] private GameObject weaponCustomizationPreviewContainer;

    [Header("Selected Weapon")]
    [SerializeField] private TextMeshProUGUI attachmentName;
    [SerializeField] private InventoryItemSO selectedWeapon;

    [Header("Attachment Inventory")]
    [SerializeField]
    private List<WeaponAttachmentDataSO> attachmentInventory = new();

    [SerializeField]
    private List<WeaponAttachmentDataSO> tempAttachments = new();

    [Header("Attachment Cards")]
    [SerializeField] private AttachmentBuyCard attachmentBuyCard;
    [SerializeField] private Transform attachmentBuyCardSpawn;

    private AttachmentType currentAttachmentType;

    private void Start()
    {
        InitializeAttachmentInventory();
        ValidateReferences();
    }

    private void InitializeAttachmentInventory()
    {
        if(InventoryManager.Instance == null) {
            Debug.LogError("InventoryManager.Instance is missing.", this);
            return;
        }

        attachmentInventory = InventoryManager.Instance.AvailableAttachments;
    }

    private void ValidateReferences()
    {
        if(weaponPreviewContainer == null)
            Debug.LogError("Weapon Preview Container is not assigned.", this);

        if(weaponCustomizationPreviewContainer == null)
            Debug.LogError("Weapon Customization Preview Prefab is not assigned.", this);

        if(attachmentBuyCardSpawn == null)
            Debug.LogError("Attachment Buy Card Spawn is not assigned.", this);

        if(attachmentBuyCard == null)
            Debug.LogError("Attachment Buy Card Prefab is not assigned.", this);
    }

    #region Weapon Customization

    public void OnCustomizeWeapon(InventoryItemSO weapon)
    {
        if(weapon is not WeaponDataSO weaponData) {
            Debug.LogWarning("Cannot customize this item because it is not a weapon.",this);
            return;
        }

        if(weaponPreviewContainer == null ||
            weaponCustomizationPreviewContainer == null) {
            Debug.LogError("Weapon preview references are not configured.", this);
            return;
        }

        if(InventoryManager.Instance == null) {
            Debug.LogError("InventoryManager.Instance is missing.", this);
            return;
        }

        selectedWeapon = weapon;

        // Load the current session's attachments into the draft list.
        tempAttachments.Clear();

        tempAttachments.AddRange(InventoryManager.Instance.GetEquippedAttachments(weaponData));

        ClearWeaponPreview();
        ClearAttachmentBuyCards();

        GameObject customizationCard = Instantiate(weaponCustomizationPreviewContainer, weaponPreviewContainer);

        WeaponCustomizationCard cardScript = customizationCard.GetComponent<WeaponCustomizationCard>();

        if(cardScript == null) {
            Debug.LogError("Weapon customization preview prefab is missing " + "WeaponCustomizationCard.", customizationCard);

            Destroy(customizationCard);
            return;
        }

        cardScript.SetWeaponPreviewCard(weapon);
        RestoreAttachmentIcons(cardScript);
    }

    private void ClearWeaponPreview()
    {
        if(weaponPreviewContainer == null)
            return;

        for(int i = weaponPreviewContainer.childCount - 1; i >= 0; i--) {
            Transform child = weaponPreviewContainer.GetChild(i);

            if(child != null)
                Destroy(child.gameObject);
        }
    }

    private void RestoreAttachmentIcons(
        WeaponCustomizationCard preview)
    {
        if(preview == null || preview.Cards == null)
            return;

        foreach(WeaponAttachmentDataSO attachment in tempAttachments) {
            if(attachment == null)
                continue;

            foreach(CustomizationAttachmentCard card in preview.Cards) {
                if(card == null ||
                    card.Type != attachment.attachmentType) {
                    continue;
                }

                card.ChangeAttachmentCardIcon(attachment);
                break;
            }
        }
    }

    #endregion

    #region Attachment Selection

    public void OpenAttachmentBuyPanel(AttachmentType attachmentType)
    {
        currentAttachmentType = attachmentType;

        ClearAttachmentBuyCards();

        if(attachmentName != null)
            attachmentName.text = attachmentType.ToString();

        if(attachmentBuyCard == null ||
            attachmentBuyCardSpawn == null) {
            Debug.LogError("Attachment buy card prefab or spawn container is missing.", this);
            return;
        }

        List<WeaponAttachmentDataSO> attachments = FindPossibleAttachments(attachmentType);

        foreach(WeaponAttachmentDataSO attachment in attachments) {
            if(attachment == null)
                continue;

            AttachmentBuyCard card = Instantiate(attachmentBuyCard, attachmentBuyCardSpawn);

            card.SetAttachmentBuyCard(attachment);

            card.UpdateEquipText(tempAttachments.Contains(attachment));
        }
    }

    public void ClearAttachmentBuyCards()
    {
        if(attachmentBuyCardSpawn == null)
            return;

        for(int i = attachmentBuyCardSpawn.childCount - 1; i >= 0; i--) {
            Transform child = attachmentBuyCardSpawn.GetChild(i);

            if(child != null)
                Destroy(child.gameObject);
        }
    }

    public List<WeaponAttachmentDataSO> FindPossibleAttachments(
        AttachmentType attachmentType)
    {
        List<WeaponAttachmentDataSO> possibleAttachments = new();

        if(selectedWeapon is not WeaponDataSO weapon)
            return possibleAttachments;

        if(attachmentInventory == null)
            return possibleAttachments;

        foreach(WeaponAttachmentDataSO attachment in attachmentInventory) {
            if(attachment == null ||
                attachment.compatibleWeapons == null) {
                continue;
            }

            if(attachment.attachmentType == attachmentType &&
                attachment.compatibleWeapons.Contains(weapon.weaponClass)) {
                possibleAttachments.Add(attachment);
            }
        }

        return possibleAttachments;
    }

    public bool CanEquipAttachment(WeaponAttachmentDataSO attachment)
    {
        return selectedWeapon is WeaponDataSO && attachment != null;
    }

    public void OnAttachmentEquipOnWeapon(WeaponAttachmentDataSO attachment)
    {
        if(selectedWeapon is not WeaponDataSO ||
            attachment == null) {
            return;
        }

        bool isEquipped = tempAttachments.Contains(attachment);

        if(isEquipped) {
            tempAttachments.Remove(attachment);
        }
        else {
            tempAttachments.RemoveAll(existing => existing != null && existing.attachmentType == attachment.attachmentType);
            tempAttachments.Add(attachment);
        }

        RefreshPreviewAttachmentIcon(attachment.attachmentType);
        RefreshAttachmentBuyCardStates();
    }

    private void RefreshPreviewAttachmentIcon(
        AttachmentType attachmentType)
    {
        if(weaponPreviewContainer == null)
            return;

        WeaponCustomizationCard preview = weaponPreviewContainer.GetComponentInChildren<WeaponCustomizationCard>();

        if(preview == null || preview.Cards == null)
            return;

        WeaponAttachmentDataSO equippedAttachment = tempAttachments.Find(existing => existing != null && existing.attachmentType == attachmentType);

        foreach(CustomizationAttachmentCard card in preview.Cards) {
            if(card == null || card.Type != attachmentType)
                continue;

            if(equippedAttachment != null)
                card.ChangeAttachmentCardIcon(equippedAttachment);
            else
                card.ClearAttachmentCardIcon();

            break;
        }
    }

    private void RefreshAttachmentBuyCardStates()
    {
        if(attachmentBuyCardSpawn == null)
            return;

        foreach(Transform child in attachmentBuyCardSpawn) {
            AttachmentBuyCard card = child.GetComponent<AttachmentBuyCard>();

            if(card == null)
                continue;

            card.UpdateEquipText(IsAttachmentSelected(card));
        }
    }

    private bool IsAttachmentSelected(AttachmentBuyCard card)
    {
        return card != null &&
               card.Attachment != null &&
               tempAttachments.Contains(card.Attachment);
    }

    #endregion

    #region Navigation

    public void OnBackButtonClick()
    {
        // Discard unconfirmed draft changes.
        tempAttachments.Clear();

        ClearAttachmentBuyCards();
        ClearWeaponPreview();

        selectedWeapon = null;

        if(attachmentName != null)
            attachmentName.text = string.Empty;

        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if(uiManager == null) {
            Debug.LogError("UIManager could not be found.", this);
            return;
        }

        if(uiManager.CustomizationPanel == null ||
            uiManager.InventoryPanel == null) {
            Debug.LogError("Customization or inventory panel reference is missing.", this);
            return;
        }

        // Return without rebuilding inventory, preserving its filters.
        uiManager.CustomizationPanel.SetActive(false);
        uiManager.InventoryPanel.SetActive(true);
    }

    #endregion

    #region Confirm Customization

    public void OnConfirmWeaponCustomization()
    {
        if(selectedWeapon is not WeaponDataSO weaponData) {
            Debug.LogWarning("Cannot confirm customization because no weapon is selected.", this);
            return;
        }

        InventoryManager inventory = InventoryManager.Instance;

        if(inventory == null) {
            Debug.LogError("Cannot confirm customization: InventoryManager is missing.", this);
            return;
        }

        inventory.SetEquippedAttachments(weaponData, tempAttachments);

        if(inventory.EquippedItems != null && inventory.EquippedItems.Contains(selectedWeapon)) {
            if(MainPanelUIManager.Instance != null) {
                MainPanelUIManager.Instance.UpdateMenuCardIcons(selectedWeapon);
            }
        }
        else {
            inventory.CheckWeaponEquipStateAndEquip(selectedWeapon);
        }

        tempAttachments.Clear();
        ClearAttachmentBuyCards();
    }

    #endregion
}