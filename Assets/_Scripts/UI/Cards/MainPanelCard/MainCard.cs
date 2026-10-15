using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCard : MonoBehaviour
{
    private static readonly int CloseHash = Animator.StringToHash("Close");
    private static readonly int OpenHash = Animator.StringToHash("Open");
    [Header("Card Configuration")]
    [SerializeField] private InventoryItemType cardType;
    [SerializeField] private MainPanelUIManager mainPanelUIManager;
    [SerializeField] private UIManager uiManager;

    [Header("Item Display")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI cardCategoryText;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject emptyItem;

    [Header("Attachment Icons")]
    [SerializeField] private Image muzzleAttachmentIcon;
    [SerializeField] private Image barrelAttachmentIcon;
    [SerializeField] private Image opticAttachmentIcon;
    [SerializeField] private Image stockAttachmentIcon;
    [SerializeField] private Image magAttachmentIcon;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private bool isAttachmentCardOpen = false;

    public InventoryItemType CardType => cardType;
    public InventoryItemSO ItemData => itemData;

    private InventoryItemSO itemData;

    public void OpenInventoryCategory()
    {
        if(mainPanelUIManager == null) {
            Debug.LogError("MainPanelUIManager is missing.", this);
            return;
        }

        mainPanelUIManager.OpenInventoryWithCategory(cardType);
    }

    public void SetMenuCard(InventoryItemSO item)
    {
        if(item == null)
            return;

        itemData = item;

        if(emptyItem != null)
            emptyItem.SetActive(false);

        if(itemName != null)
            itemName.text = item.itemName;

        if(itemImage != null)
            itemImage.sprite = item.itemSprite;

        if(cardCategoryText != null)
            cardCategoryText.text = item.itemType.ToString();
    }

    public void UpdateAttachmentIcons(List<WeaponAttachmentDataSO> attachments)
    {
        ResetAttachmentIcons();

        if(attachments == null)
            return;

        foreach(WeaponAttachmentDataSO attachment in attachments) {
            if(attachment == null)
                continue;

            Image targetIcon = GetAttachmentIcon(attachment.attachmentType);

            if(targetIcon == null)
                continue;

            targetIcon.sprite = attachment.attachmentIcon;
            targetIcon.enabled = attachment.attachmentIcon != null;
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

    // Connect this function to your Button's OnClick event.
    public void OnCustomizeAttachmentsButtonClick()
    {
        if(isAttachmentCardOpen) {
            animator.SetTrigger(CloseHash);
            Debug.Log("Close");
        }
        else {
            animator.SetTrigger(OpenHash);
            Debug.Log("Open");
        }
        isAttachmentCardOpen = !isAttachmentCardOpen;
        Debug.Log(isAttachmentCardOpen);
    }

    private void ResetAttachmentIcon(Image icon)
    {
        if(icon == null)
            return;

        icon.sprite = null;
        icon.enabled = false;
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
}