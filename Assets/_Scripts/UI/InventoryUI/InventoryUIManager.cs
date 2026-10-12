using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public UIManager uiManager;

    public BuyCard buyCard;

    public InventoryTabGroup tabGroup;

    [Header("Weapon Filter")]
    public TMP_Dropdown primaryDropdown;

    [Header("Secondary Filter")]
    public TMP_Dropdown secondaryDropdown;

    [Header("Throwable Filter")]
    public TMP_Dropdown throwableDropdown;

    [Header("Panels Parents")]
    public Transform primaryWeaponContent;
    public Transform secondaryWeaponContent;
    public Transform throwableContent;
    public Transform armorContent;

    private void Start()
    {
        RefreshInventory();

        primaryDropdown.onValueChanged.AddListener(OnPrimaryDropdownChanged);
        OnPrimaryDropdownChanged(primaryDropdown.value);

        secondaryDropdown.onValueChanged.AddListener(OnSecondaryDropdownChanged);
        OnSecondaryDropdownChanged(secondaryDropdown.value);

        throwableDropdown.onValueChanged.AddListener(OnThrowableDropdownChanged);
        OnThrowableDropdownChanged(throwableDropdown.value);
    }

    #region Filters
    #region Primary Filter
    public void ResetPrimaryWeaponFilter()
    {
        primaryDropdown.SetValueWithoutNotify(0);
        OnPrimaryDropdownChanged(0);
    }

    public void OnPrimaryDropdownChanged(int index)
    {
        foreach(Transform child in primaryWeaponContent) {
            BuyCard card = child.GetComponent<BuyCard>();
            if(card == null || card.itemData == null) { continue; }

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.itemData is not WeaponDataSO weaponData) {
                card.gameObject.SetActive(false);
                continue;
            }

            WeaponClass selectedClass = index switch {
                1 => WeaponClass.AR,
                2 => WeaponClass.SMG,
                3 => WeaponClass.Sniper,
                _ => (WeaponClass)(-1),
            };

            card.gameObject.SetActive(weaponData.weaponClass == selectedClass);
        }
    }
    #endregion

    #region Secondary Filter
    public void ResetSecondaryWeaponFilter()
    {
        primaryDropdown.SetValueWithoutNotify(0);
        OnSecondaryDropdownChanged(0);
    }

    public void OnSecondaryDropdownChanged(int index)
    {
        foreach(Transform child in secondaryWeaponContent) {
            BuyCard card = child.GetComponent<BuyCard>();
            if(card == null || card.itemData == null) { continue; }

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.itemData is not WeaponDataSO weaponData) {
                card.gameObject.SetActive(false);
                continue;
            }

            WeaponClass selectedClass = index switch {
                1 => WeaponClass.Pistol,
                2 => WeaponClass.Melee,
                3 => WeaponClass.Launchers,
                _ => (WeaponClass)(-1),
            };

            card.gameObject.SetActive(weaponData.weaponClass == selectedClass);
        }
    }
    #endregion

    #region Throwable Filter
    public void ResetThrowableWeaponFilter()
    {
        throwableDropdown.SetValueWithoutNotify(0);
        OnThrowableDropdownChanged(0);
    }

    public void OnThrowableDropdownChanged(int index)
    {
        foreach(Transform child in throwableContent) {
            BuyCard card = child.GetComponent<BuyCard>();
            if(card == null || card.itemData == null) { continue; }

            if(index == 0) {
                card.gameObject.SetActive(true);
                continue;
            }

            if(card.itemData is not ThrowableDataSO throwable) {
                card.gameObject.SetActive(false);
                continue;
            }

            ThrowableType selectedClass = index switch {
                1 => ThrowableType.lethal,
                2 => ThrowableType.tactical,
                _ => (ThrowableType)(-1),
            };

            card.gameObject.SetActive(throwable.throwableType == selectedClass);
        }
    }
    #endregion
    #endregion

    public void RefreshInventory()
    {
        ClearCards(primaryWeaponContent);
        ClearCards(secondaryWeaponContent);
        ClearCards(throwableContent);
        ClearCards(armorContent);

        inventoryManager.CategorizeInventoryItem();

        ResetPrimaryWeaponFilter();

        PopulateCategory(inventoryManager.primaryInventoryWeapons, primaryWeaponContent);
        PopulateCategory(inventoryManager.secondaryInventoryWeapons, secondaryWeaponContent);
        PopulateCategory(inventoryManager.throwableInventoryItems, throwableContent);
        PopulateCategory(inventoryManager.armorInventoryItems, armorContent);
    }

    public void PopulateCategory(List<InventoryItemSO> items, Transform container)
    {
        foreach(InventoryItemSO item in items) {
            BuyCard card = Instantiate(buyCard, container);
            card.itemData = item;
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

    public void RefreshEquipButtonStates()
    {
        foreach(BuyCard card in FindObjectsByType<BuyCard>(
            FindObjectsSortMode.None)) {
            card.RefreshEquipButtonText();
        }
    }

    public void OpenFullInventory()
    {
        tabGroup.OpenFullInventoryTabs();

        ResetPrimaryWeaponFilter();

        uiManager.mainPanel.SetActive(false);
        uiManager.inventoryPanel.SetActive(true);
    }

    public void OnCustomizeWeaponClick(InventoryItemSO item)
    {
        uiManager.OpenCustomizationPanel(item);
    }

    // should open the main cards
    public void OnEquipItemClick()
    {
        uiManager.OpenMainCards();
    }
}
