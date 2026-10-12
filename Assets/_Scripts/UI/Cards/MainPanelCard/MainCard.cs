using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCard : MonoBehaviour
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
    [SerializeField] private Image muzzleAttachmentIcon;
    [SerializeField] private Image barrelAttachmentIcon;
    [SerializeField] private Image opticAttachmentIcon;
    [SerializeField] private Image stockAttachmentIcon;
    [SerializeField] private Image magAttachmentIcon;

    public InventoryItemType CardType => cardType;

    public void OpenInventoryCategory()
    {
        if(mainPanelUIManager == null) {
            Debug.LogError(
                "Cannot open inventory category: MainPanelUIManager is missing.",
                this);
            return;
        }

        mainPanelUIManager.OpenInventoryWithCategory(cardType);
    }

    public void SetMenuCard(InventoryItemSO item)
    {
        if(item == null) {
            Debug.LogWarning(
                "Cannot set main card: inventory item is null.",
                this);
            return;
        }

        if(emptyItem != null)
            emptyItem.SetActive(false);

        if(itemName != null)
            itemName.text = item.itemName;

        if(itemImage != null)
            itemImage.sprite = item.itemSprite;

        if(cardCategoryText != null)
            cardCategoryText.text = item.itemType.ToString();
    }

    public void UpdateAttachmentIcons(
        List<WeaponAttachmentDataSO> attachments)
    {
        ResetAttachmentIcons();

        if(attachments == null)
            return;

        foreach(WeaponAttachmentDataSO attachment in attachments) {
            if(attachment == null)
                continue;

            Image targetIcon = GetAttachmentIcon(
                attachment.attachmentType);

            if(targetIcon == null)
                continue;

            targetIcon.sprite = attachment.attachmentIcon != null
                ? attachment.attachmentIcon
                : transparentIcon;
        }
    }

    public void ResetAttachmentIcons()
    {
        ResetAttachmentIcon(muzzleAttachmentIcon);
        ResetAttachmentIcon(barrelAttachmentIcon);
        ResetAttachmentIcon(opticAttachmentIcon);
        ResetAttachmentIcon(stockAttachmentIcon);
        ResetAttachmentIcon(magAttachmentIcon);
    }

    private void ResetAttachmentIcon(Image icon)
    {
        if(icon != null)
            icon.sprite = transparentIcon;
    }

    private Image GetAttachmentIcon(AttachmentType attachmentType)
    {
        switch(attachmentType) {
            case AttachmentType.Muzzle:
                return muzzleAttachmentIcon;

            case AttachmentType.Barrel:
                return barrelAttachmentIcon;

            case AttachmentType.Optic:
                return opticAttachmentIcon;

            case AttachmentType.Stock:
                return stockAttachmentIcon;

            case AttachmentType.Magazine:
                return magAttachmentIcon;

            default:
                return null;
        }
    }
}