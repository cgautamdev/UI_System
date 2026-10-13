using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Display")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private InventoryItemSO itemData;

    [Header("Card Events")]
    [SerializeField] private UnityEvent onCardEnter;
    [SerializeField] private UnityEvent onCardExit;

    [Header("Action Buttons")]
    [SerializeField] private Button customizeButton;
    [SerializeField] private Button equipButton;

    public InventoryItemSO ItemData => itemData;

    private void Start()
    {
        if(itemData == null) {
            Debug.LogWarning("BuyCard has no itemData assigned.", this);
            return;
        }

        ConfigureCustomizeButton();
        RefreshEquipButtonText();
    }

    public void Initialize(InventoryItemSO item)
    {
        if(item == null) {
            Debug.LogWarning("Cannot initialize BuyCard with a null item.", this);
            return;
        }

        itemData = item;

        if(itemName != null)
            itemName.text = item.itemName;

        if(itemImage != null)
            itemImage.sprite = item.itemSprite;

        ConfigureCustomizeButton();
        RefreshEquipButtonText();
    }

    private void ConfigureCustomizeButton()
    {
        if(customizeButton == null || itemData == null)
            return;

        bool canCustomize =
            itemData.itemType != InventoryItemType.Throwable &&
            itemData.itemType != InventoryItemType.Armor;

        if(itemData is WeaponDataSO weaponData &&
            (weaponData.weaponClass == WeaponClass.Launchers ||
             weaponData.weaponClass == WeaponClass.Melee)) {
            canCustomize = false;
        }

        customizeButton.gameObject.SetActive(canCustomize);
    }

    public void OnEquipWeaponButtonPressed()
    {
        InventoryManager inventory = InventoryManager.Instance;

        if(itemData == null) {
            Debug.LogWarning("Cannot equip an item because itemData is null.", this);
            return;
        }

        if(inventory == null) {
            Debug.LogError("InventoryManager.Instance is missing.", this);
            return;
        }

        if(inventory.EquippedItems == null) {
            Debug.LogWarning("Equipped items list is not initialized.", this);
            return;
        }

        if(!inventory.EquippedItems.Contains(itemData)) {
            inventory.CheckWeaponEquipStateAndEquip(itemData);
        }

        RefreshEquipButtonText();

        InventoryUIManager manager = GetComponentInParent<InventoryUIManager>();

        if(manager != null) {
            manager.RefreshEquipButtonStates();
            manager.OnEquipItemClick();
        }
    }

    public void RefreshEquipButtonText()
    {
        if(itemData == null || equipButton == null)
            return;

        InventoryManager inventory = InventoryManager.Instance;

        if(inventory == null || inventory.EquippedItems == null)
            return;

        bool isEquipped = inventory.EquippedItems.Contains(itemData);

        TextMeshProUGUI buttonText = equipButton.GetComponentInChildren<TextMeshProUGUI>(true);

        if(buttonText != null)
            buttonText.text = isEquipped ? "Equipped" : "Equip";

        equipButton.interactable = true;
    }

    public void OnCustomizeWeaponButtonPressed()
    {
        if(itemData == null)
            return;

        InventoryUIManager manager = GetComponentInParent<InventoryUIManager>();

        if(manager != null) {
            manager.OnCustomizeWeaponClick(itemData);
        }
        else {
            Debug.LogWarning("InventoryUIManager could not be found in the parent hierarchy.", this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onCardEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onCardExit?.Invoke();
    }
}