using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Configuration")]
    [SerializeField] private InventoryItemType cardType;
    [SerializeField] private MainPanelUIManager mainPanelUIManager;

    [Header("Item Display")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI cardCategoryText;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject emptyItem;

    [Header("Attachment Icon Settings")]
    [SerializeField] private Sprite transparentIcon;

    [Header("Attachment Icons")]
    [SerializeField] private Transform attachmentContainer;
    [SerializeField] private Image muzzleAttachmentIcon;
    [SerializeField] private Image barrelAttachmentIcon;
    [SerializeField] private Image opticAttachmentIcon;
    [SerializeField] private Image stockAttachmentIcon;
    [SerializeField] private Image magAttachmentIcon;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent PointerEnter;
    [SerializeField] private UnityEvent PointerExit;

    public InventoryItemType CardType => cardType;
    private InventoryItemSO itemData;

    private bool isHovered;
    private bool hasAttachments;

    private void Awake()
    {
        hasAttachments = false;

        if(attachmentContainer != null)
            attachmentContainer.gameObject.SetActive(false);
    }

    public void OpenInventoryCategory()
    {
        if(mainPanelUIManager == null) {
            Debug.LogError(
                "MainPanelUIManager is missing.",
                this
            );
            return;
        }

        mainPanelUIManager.OpenInventoryWithCategory(cardType);
    }

    public void SetMenuCard(InventoryItemSO item)
    {
        if(item == null)
            return;

        if(emptyItem != null)
            emptyItem.SetActive(false);

        if(itemName != null)
            itemName.text = item.itemName;

        if(itemImage != null)
            itemImage.sprite = item.itemSprite;

        if(cardCategoryText != null)
            cardCategoryText.text = item.itemType.ToString();

        itemData = item;
    }

    public void UpdateAttachmentIcons(
        List<WeaponAttachmentDataSO> attachments
    )
    {
        ResetAttachmentIcons();

        if(attachments != null) {
            foreach(WeaponAttachmentDataSO attachment in attachments) {
                if(attachment == null)
                    continue;

                Image targetIcon = GetAttachmentIcon(
                    attachment.attachmentType
                );

                if(targetIcon == null)
                    continue;

                hasAttachments = true;

                targetIcon.sprite = attachment.attachmentIcon != null
                    ? attachment.attachmentIcon
                    : transparentIcon;
            }
        }

        UpdateAttachmentContainerVisibility();
    }

    public void ResetAttachmentIcons()
    {
        ResetAttachmentIcon(muzzleAttachmentIcon);
        ResetAttachmentIcon(barrelAttachmentIcon);
        ResetAttachmentIcon(opticAttachmentIcon);
        ResetAttachmentIcon(stockAttachmentIcon);
        ResetAttachmentIcon(magAttachmentIcon);

        hasAttachments = false;

        UpdateAttachmentContainerVisibility();
    }

    // Call this from a Unity Event to show the attachment container.
    public void ShowAttachmentContainer()
    {
        isHovered = true;
        UpdateAttachmentContainerVisibility();
    }

    // Call this from a Unity Event to hide the attachment container.
    public void HideAttachmentContainer()
    {
        isHovered = false;
        UpdateAttachmentContainerVisibility();
    }

    public InventoryItemSO ItemData => itemData;

    private bool SupportsAttachments()
    {
        return cardType != InventoryItemType.Throwable &&
               cardType != InventoryItemType.Armor;
    }

    private void UpdateAttachmentContainerVisibility()
    {
        if(attachmentContainer == null)
            return;

        bool shouldShow =
            SupportsAttachments() &&
            isHovered &&
            hasAttachments;

        attachmentContainer.gameObject.SetActive(shouldShow);
    }

    private void ResetAttachmentIcon(Image icon)
    {
        if(icon != null)
            icon.sprite = transparentIcon;
    }

    private Image GetAttachmentIcon(AttachmentType attachmentType)
    {
        return attachmentType switch {
            AttachmentType.Muzzle => muzzleAttachmentIcon,
            AttachmentType.Barrel => barrelAttachmentIcon,
            AttachmentType.Optic => opticAttachmentIcon,
            AttachmentType.Stock => stockAttachmentIcon,
            AttachmentType.Magazine => magAttachmentIcon,
            _ => null
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowAttachmentContainer();
        PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAttachmentContainer();
        PointerExit?.Invoke();
    }
}