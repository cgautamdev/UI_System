using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainCard : MonoBehaviour
{
    public InventoryItemType cardType;
    public MainPanelUIManager mainPanelUIManager;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI cardCategoryText;
    public Image itemImage;
    public GameObject emptyItem;

    public Sprite transparentIcon;
    public Image muzzleAttachmentIcon;
    public Image barrelAttachmentIcon;
    public Image opticAttachmentIcon;
    public Image stockAttachmentIcon;
    public Image magAttachmentIcon;

    public void OpenInventoryCategory()
    {
        InventoryManager.Instance.primaryInventoryWeapons.Clear();
        InventoryManager.Instance.secondaryInventoryWeapons.Clear();

        mainPanelUIManager.OpenInventoryWithCategory(cardType);
    }

    public void SetMenuCard(InventoryItemSO item)
    {
        emptyItem.SetActive(false);
        itemName.text = item.itemName;
        itemImage.sprite = item.itemSprite;
    }

    public void UpdateAttachmentIcons(
        List<WeaponAttachmentDataSO> attachments)
    {
        // Reset all icons to their default sprites.
        muzzleAttachmentIcon.sprite = transparentIcon;
        barrelAttachmentIcon.sprite = transparentIcon;
        opticAttachmentIcon.sprite = transparentIcon;
        stockAttachmentIcon.sprite = transparentIcon;
        magAttachmentIcon.sprite = transparentIcon;

        foreach(WeaponAttachmentDataSO attachment in attachments) {
            switch(attachment.attachmentType) {
                case AttachmentType.Muzzle:
                    muzzleAttachmentIcon.sprite = attachment.attachmentIcon;
                    break;

                case AttachmentType.Barrel:
                    barrelAttachmentIcon.sprite = attachment.attachmentIcon;
                    break;

                case AttachmentType.Optic:
                    opticAttachmentIcon.sprite = attachment.attachmentIcon;
                    break;

                case AttachmentType.Stock:
                    stockAttachmentIcon.sprite = attachment.attachmentIcon;
                    break;

                case AttachmentType.Magazine:
                    magAttachmentIcon.sprite = attachment.attachmentIcon;
                    break;
            }
        }
    }

}
