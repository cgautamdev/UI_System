using TMPro;
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

        if(itemData.itemType == InventoryItemType.Throwable || itemData.itemType == InventoryItemType.Armor) {
            customizeButton.gameObject.SetActive(false);
        }
    }

    public void OnEquipWeaponButtonPressed()
    {
        InventoryManager.Instance.CheckWeaponEquipStateAndEquip(itemData);
        RefreshEquipButtonText();
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
