using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BuyCard buyCard;
    [SerializeField] private InventoryTabGroup tabGroup;

    [Header("Weapon Filters")]
    [SerializeField] private TMP_Dropdown primaryDropdown;
    [SerializeField] private TMP_Dropdown secondaryDropdown;
    [SerializeField] private TMP_Dropdown throwableDropdown;

    [Header("Inventory Content")]
    [SerializeField] private Transform primaryWeaponContent;
    [SerializeField] private Transform secondaryWeaponContent;
    [SerializeField] private Transform throwableContent;
    [SerializeField] private Transform armorContent;

    private void Start()
    {
        if(inventoryManager == null)
            inventoryManager = InventoryManager.Instance;

        if(uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();

        if(inventoryManager == null) {
            Debug.LogError("InventoryManager reference is missing.", this);
            return;
        }

        if(!ValidateFilterReferences())
            return;

        RegisterFilterListeners();
        RefreshInventory();
    }

    private bool ValidateFilterReferences()
    {
        if(primaryDropdown == null ||
            secondaryDropdown == null ||
            throwableDropdown == null) {
            Debug.LogError(
                "One or more inventory filter dropdowns are missing.",
                this);
            return false;
        }

        return true;
    }

    private void RegisterFilterListeners()
    {
        primaryDropdown.onValueChanged.RemoveListener(
            OnPrimaryDropdownChanged);
        primaryDropdown.onValueChanged.AddListener(
            OnPrimaryDropdownChanged);

        secondaryDropdown.onValueChanged.RemoveListener(
            OnSecondaryDropdownChanged);
        secondaryDropdown.onValueChanged.AddListener(
            OnSecondaryDropdownChanged);

        throwableDropdown.onValueChanged.RemoveListener(
            OnThrowableDropdownChanged);
        throwableDropdown.onValueChanged.AddListener(
            OnThrowableDropdownChanged);
    }

    #region Primary Weapon Filter

    public void ResetPrimaryWeaponFilter()
    {
        if(primaryDropdown == null)
            return;

        primaryDropdown.SetValueWithoutNotify(0);
        OnPrimaryDropdownChanged(0);
    }

    public void OnPrimaryDropdownChanged(int index)
    {
        if(primaryWeaponContent == null)
            return;

        foreach(Transform child in primaryWeaponContent) {
            BuyCard card = child.GetComponent<BuyCard>();

            if(card == null || card.ItemData == null)
                continue;

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.ItemData is not WeaponDataSO weaponData) {
                card.gameObject.SetActive(false);
                continue;
            }

            WeaponClass selectedClass = index switch {
                1 => WeaponClass.AR,
                2 => WeaponClass.SMG,
                3 => WeaponClass.Sniper,
                _ => (WeaponClass)(-1)
            };

            card.gameObject.SetActive(
                weaponData.weaponClass == selectedClass);
        }
    }

    #endregion

    #region Secondary Weapon Filter

    public void ResetSecondaryWeaponFilter()
    {
        if(secondaryDropdown == null)
            return;

        secondaryDropdown.SetValueWithoutNotify(0);
        OnSecondaryDropdownChanged(0);
    }

    public void OnSecondaryDropdownChanged(int index)
    {
        if(secondaryWeaponContent == null)
            return;

        foreach(Transform child in secondaryWeaponContent) {
            BuyCard card = child.GetComponent<BuyCard>();

            if(card == null || card.ItemData == null)
                continue;

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.ItemData is not WeaponDataSO weaponData) {
                card.gameObject.SetActive(false);
                continue;
            }

            WeaponClass selectedClass = index switch {
                1 => WeaponClass.Pistol,
                2 => WeaponClass.Melee,
                3 => WeaponClass.Launchers,
                _ => (WeaponClass)(-1)
            };

            card.gameObject.SetActive(
                weaponData.weaponClass == selectedClass);
        }
    }

    #endregion

    #region Throwable Filter

    public void ResetThrowableWeaponFilter()
    {
        if(throwableDropdown == null)
            return;

        throwableDropdown.SetValueWithoutNotify(0);
        OnThrowableDropdownChanged(0);
    }

    public void OnThrowableDropdownChanged(int index)
    {
        if(throwableContent == null)
            return;

        foreach(Transform child in throwableContent) {
            BuyCard card = child.GetComponent<BuyCard>();

            if(card == null || card.ItemData == null)
                continue;

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.ItemData is not ThrowableDataSO throwable) {
                card.gameObject.SetActive(false);
                continue;
            }

            ThrowableType selectedType = index switch {
                1 => ThrowableType.lethal,
                2 => ThrowableType.tactical,
                _ => (ThrowableType)(-1)
            };

            card.gameObject.SetActive(
                throwable.throwableType == selectedType);
        }
    }

    #endregion

    #region Inventory Population

    public void RefreshInventory()
    {
        if(inventoryManager == null) {
            Debug.LogError(
                "Cannot refresh inventory: InventoryManager is missing.",
                this);
            return;
        }

        ClearCards(primaryWeaponContent);
        ClearCards(secondaryWeaponContent);
        ClearCards(throwableContent);
        ClearCards(armorContent);

        inventoryManager.CategorizeInventoryItem();

        PopulateCategory(
            inventoryManager.PrimaryInventoryWeapons,
            primaryWeaponContent);

        PopulateCategory(
            inventoryManager.SecondaryInventoryWeapons,
            secondaryWeaponContent);

        PopulateCategory(
            inventoryManager.ThrowableInventoryItems,
            throwableContent);

        PopulateCategory(
            inventoryManager.ArmorInventoryItems,
            armorContent);

        ApplyCurrentFilters();
    }

    public void PopulateCategory(
        List<InventoryItemSO> items,
        Transform container)
    {
        if(items == null || container == null) {
            Debug.LogError(
                "Cannot populate inventory: items or container is missing.",
                this);
            return;
        }

        if(buyCard == null) {
            Debug.LogError("BuyCard prefab is not assigned.", this);
            return;
        }

        foreach(InventoryItemSO item in items) {
            if(item == null)
                continue;

            BuyCard card = Instantiate(buyCard, container);
            card.Initialize(item);
        }
    }

    public void ClearCards(Transform container)
    {
        if(container == null)
            return;

        for(int i = container.childCount - 1; i >= 0; i--) {
            Transform child = container.GetChild(i);

            if(child != null)
                Destroy(child.gameObject);
        }
    }

    private void ApplyCurrentFilters()
    {
        if(primaryDropdown != null)
            OnPrimaryDropdownChanged(primaryDropdown.value);

        if(secondaryDropdown != null)
            OnSecondaryDropdownChanged(secondaryDropdown.value);

        if(throwableDropdown != null)
            OnThrowableDropdownChanged(throwableDropdown.value);
    }

    #endregion

    #region Equip State

    public void RefreshEquipButtonStates()
    {
        BuyCard[] cards = FindObjectsByType<BuyCard>(
            FindObjectsSortMode.None);

        foreach(BuyCard card in cards) {
            if(card != null)
                card.RefreshEquipButtonText();
        }
    }

    #endregion

    #region Navigation

    public void OpenFullInventory()
    {
        if(tabGroup == null) {
            Debug.LogError("InventoryTabGroup reference is missing.", this);
            return;
        }

        if(uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();

        if(uiManager == null ||
            uiManager.MainPanel == null ||
            uiManager.InventoryPanel == null) {
            Debug.LogError(
                "UIManager or its main/inventory panels are missing.",
                this);
            return;
        }

        tabGroup.OpenFullInventoryTabs();

        ResetPrimaryWeaponFilter();
        ResetSecondaryWeaponFilter();
        ResetThrowableWeaponFilter();

        uiManager.MainPanel.SetActive(false);
        uiManager.InventoryPanel.SetActive(true);
    }

    public void OnCustomizeWeaponClick(InventoryItemSO item)
    {
        if(uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();

        if(uiManager == null) {
            Debug.LogError("UIManager reference is missing.", this);
            return;
        }

        if(item == null) {
            Debug.LogWarning(
                "Cannot customize a null inventory item.", this);
            return;
        }

        uiManager.OpenCustomizationPanel(item);
    }

    public void OnEquipItemClick()
    {
        if(uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();

        if(uiManager == null) {
            Debug.LogError("UIManager reference is missing.", this);
            return;
        }

        uiManager.OpenMainCards();
    }

    #endregion
}