using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI itemName;
    public Image itemImage;
    public InventoryItemSO itemData;

    public UnityEvent OnCardEnter;
    public UnityEvent OnCardExit;

    public Button customizeButton;
    public Button equipButton;

    private void Start()
    {
        customizeButton.onClick.AddListener(OnCustomizeWeaponButtonPressed);
        equipButton.onClick.AddListener(OnEquipWeaponButtonPressed);

        // hide customize button for these
        if(itemData.itemType == InventoryItemType.Throwable || itemData.itemType == InventoryItemType.Armor) {
            customizeButton.gameObject.SetActive(false);
        }

        // hide customize button for launchers and melee as well
        WeaponDataSO weaponData = itemData as WeaponDataSO;
        if(weaponData != null && (weaponData.weaponClass == WeaponClass.Launchers || weaponData.weaponClass == WeaponClass.Melee)){
                customizeButton.gameObject.SetActive(false);
        }
    }

    public void OnEquipWeaponButtonPressed()
    {
        InventoryManager.Instance.CheckWeaponEquipStateAndEquip(itemData);
        RefreshEquipButtonText();
        InventoryUIManager manager = GetComponentInParent<InventoryUIManager>();
        if(manager != null) {
            manager.OnEquipItemClick();
        }
    }


    public void RefreshEquipButtonText()
    {
        bool isEquipped = InventoryManager.Instance.equippedItems.Contains(itemData);

        TextMeshProUGUI buttonText = equipButton.GetComponentInChildren<TextMeshProUGUI>();

        if(buttonText != null) {
            buttonText.text = isEquipped ? "Equipped" : "Equip";
        }
    }

    public void OnCustomizeWeaponButtonPressed()
    {
        InventoryUIManager manager = GetComponentInParent<InventoryUIManager>();
        if(manager != null)
            manager.OnCustomizeWeaponClick(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCardEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCardExit?.Invoke();
    }
}
