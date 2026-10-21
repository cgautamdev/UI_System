using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCard : MonoBehaviour, IPointerExitHandler
{
    private static readonly int ClosedHash = Animator.StringToHash("Closed");
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

        ResetAttachmentCardAnimation();

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


    public void OnAttachmentButtonClick(AttachmentType attachmentType)
    {
        if(uiManager == null) {
            Debug.LogError("UIManager is missing.", this);
            return;
        }

        if(itemData == null) {
            Debug.LogWarning("No weapon is assigned to this card.", this);
            return;
        }

        uiManager.OpenCustomizationAttachmentButtonClick(
            itemData,
            attachmentType);
    }


    // Connect this function to your Button's OnClick event.
    public void OnCustomizeAttachmentsButtonClick()
    {
        if(animator == null) {
            Debug.LogError("Attachments Animator is not assigned.", this);
            return;
        }

        // Read the Animator's actual state instead of trusting a separate bool.
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        bool isOpen =
            state.IsName("Opened") ||
            (state.IsName("Open") && state.normalizedTime >= 0.9f);

        // Clear both triggers so they cannot conflict.
        animator.ResetTrigger(OpenHash);
        animator.ResetTrigger(CloseHash);

        if(isOpen)
            animator.SetTrigger(CloseHash);
        else if(state.IsName("Closed"))
            animator.SetTrigger(OpenHash);
        else
            Debug.Log("Animation is transitioning. Try again when it finishes.", this);
    }

    public void ResetAttachmentCardAnimation()
    {
        if(animator == null)
            return;

        animator.ResetTrigger(OpenHash);
        animator.ResetTrigger(CloseHash);

        isAttachmentCardOpen = false;

        if(animator.isActiveAndEnabled) {
            animator.Play("Closed", 0, 0f);
            animator.Update(0f);
        }
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

    public void OnPointerExit(PointerEventData eventData)
    {
        if(animator == null)
            return;

        animator.ResetTrigger(OpenHash);
        animator.ResetTrigger(CloseHash);
    }
}