using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationAttachmentCard : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Attachment Configuration")]
    [SerializeField] private AttachmentType attachmentType;

    [Header("Attachment Display")]
    [SerializeField] private TextMeshProUGUI attachmentName;
    [SerializeField] private Image attachmentIcon;
    [SerializeField] private Sprite tempSprite;

    [Header("Pointer Events")]
    [SerializeField] private UnityEvent PointerEnter;
    [SerializeField] private UnityEvent PointerExit;

    public AttachmentType Type => attachmentType;

    private void Start()
    {
        if(attachmentName != null)
            attachmentName.text = attachmentType.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke();
    }

    public void OnAttachmentCardClick()
    {
        CustomizationPanelUIManager manager = GetComponentInParent<CustomizationPanelUIManager>();

        if(manager == null) {
            Debug.LogWarning("CustomizationPanelUIManager was not found in the parent hierarchy.", this);
            return;
        }
        Debug.Log("Click");
        manager.OpenAttachmentBuyPanel(attachmentType);
    }

    public void ChangeAttachmentCardIcon(
        WeaponAttachmentDataSO weaponAttachmentDataSO)
    {
        if(weaponAttachmentDataSO == null) {
            Debug.LogWarning("Cannot update attachment icon: attachment data is null.", this);
            return;
        }

        if(attachmentIcon == null) {
            Debug.LogWarning("Attachment icon Image reference is missing.", this);
            return;
        }

        attachmentIcon.sprite = weaponAttachmentDataSO.attachmentIcon;
    }

    public void ClearAttachmentCardIcon()
    {
        if(attachmentIcon != null)
            attachmentIcon.sprite = tempSprite;
    }
}