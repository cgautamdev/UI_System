using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Data")]
    [SerializeField] private InventoryItemSO[] inventory;
    [SerializeField]
    private List<InventoryItemSO> equippedItems =
        new List<InventoryItemSO>();

    [Header("Available Attachments")]
    [SerializeField]
    private List<WeaponAttachmentDataSO> availableAttachments =
        new List<WeaponAttachmentDataSO>();

    [Header("Legacy Attachment Lists")]
    [SerializeField]
    private List<WeaponAttachmentDataSO> primaryWeaponAttachments =
        new List<WeaponAttachmentDataSO>();

    [SerializeField]
    private List<WeaponAttachmentDataSO> secondaryWeaponAttachments =
        new List<WeaponAttachmentDataSO>();

    [Header("Categorized Inventory")]
    [SerializeField]
    private List<InventoryItemSO> primaryInventoryWeapons =
        new List<InventoryItemSO>();

    [SerializeField]
    private List<InventoryItemSO> secondaryInventoryWeapons =
        new List<InventoryItemSO>();

    [SerializeField]
    private List<InventoryItemSO> throwableInventoryItems =
        new List<InventoryItemSO>();

    [SerializeField]
    private List<InventoryItemSO> armorInventoryItems =
        new List<InventoryItemSO>();

    // Runtime-only attachment state. Never written to disk.
    private readonly Dictionary<WeaponDataSO, List<WeaponAttachmentDataSO>>
        runtimeWeaponAttachments =
            new Dictionary<WeaponDataSO, List<WeaponAttachmentDataSO>>();

    public InventoryItemSO[] Inventory => inventory;
    public List<InventoryItemSO> EquippedItems => equippedItems;
    public List<WeaponAttachmentDataSO> AvailableAttachments => availableAttachments;
    public List<InventoryItemSO> PrimaryInventoryWeapons => primaryInventoryWeapons;
    public List<InventoryItemSO> SecondaryInventoryWeapons => secondaryInventoryWeapons;
    public List<InventoryItemSO> ThrowableInventoryItems => throwableInventoryItems;
    public List<InventoryItemSO> ArmorInventoryItems => armorInventoryItems;

    private void Awake()
    {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Every new session starts with no runtime attachment changes.
        runtimeWeaponAttachments.Clear();

        CategorizeInventoryItem();
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }

    public List<WeaponAttachmentDataSO> GetEquippedAttachments(
        WeaponDataSO weapon)
    {
        if(weapon == null)
            return new List<WeaponAttachmentDataSO>();

        if(!runtimeWeaponAttachments.TryGetValue(
                weapon, out List<WeaponAttachmentDataSO> attachments)) {
            attachments = new List<WeaponAttachmentDataSO>();
            runtimeWeaponAttachments.Add(weapon, attachments);
        }

        return attachments;
    }

    public void SetEquippedAttachments(
        WeaponDataSO weapon,
        List<WeaponAttachmentDataSO> attachments)
    {
        if(weapon == null)
            return;

        List<WeaponAttachmentDataSO> runtimeAttachments =
            GetEquippedAttachments(weapon);

        runtimeAttachments.Clear();

        if(attachments == null)
            return;

        foreach(WeaponAttachmentDataSO attachment in attachments) {
            if(attachment != null)
                runtimeAttachments.Add(attachment);
        }
    }

    public void CategorizeInventoryItem()
    {
        primaryInventoryWeapons.Clear();
        secondaryInventoryWeapons.Clear();
        throwableInventoryItems.Clear();
        armorInventoryItems.Clear();

        if(inventory == null) {
            Debug.LogWarning(
                "InventoryManager: Inventory array is not assigned.",
                this);
            return;
        }

        foreach(InventoryItemSO item in inventory) {
            if(item == null)
                continue;

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

                default:
                    Debug.LogWarning(
                        $"Unsupported inventory item type: {item.itemType}.",
                        item);
                    break;
            }
        }
    }

    public void CheckWeaponEquipStateAndEquip(InventoryItemSO item)
    {
        if(item == null)
            return;

        if(equippedItems.Contains(item)) {
            RefreshMainCard(item);
            return;
        }

        InventoryItemSO existingItem =
            equippedItems.Find(x => x != null && x.itemType == item.itemType);

        if(existingItem != null)
            equippedItems.Remove(existingItem);

        equippedItems.Add(item);

        RefreshMainCard(item);
    }

    private void RefreshMainCard(InventoryItemSO item)
    {
        if(MainPanelUIManager.Instance != null) {
            MainPanelUIManager.Instance.UpdateMenuCard(item);
            MainPanelUIManager.Instance.UpdateMenuCardIcons(item);
        }

        FindFirstObjectByType<InventoryUIManager>()
            ?.RefreshEquipButtonStates();
    }

    public void ClearEquippedAttachments(InventoryItemType weaponType)
    {
        // Legacy lists are retained for compatibility, but are not
        // the source of attachment data in the runtime system.
        switch(weaponType) {
            case InventoryItemType.PrimaryWeapon:
                primaryWeaponAttachments.Clear();
                break;

            case InventoryItemType.SecondaryWeapon:
                secondaryWeaponAttachments.Clear();
                break;
        }
    }
}