
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentBuyCard : MonoBehaviour
{
    [Header("Attachment Display")]
    [SerializeField] private Image attachmentImage;
    [SerializeField] private TextMeshProUGUI attachmentName;

    [Header("Equip Button")]
    [SerializeField] private Button attachmentEquipButton;
    [SerializeField] private TextMeshProUGUI equipText;

    [Header("Attachment Data")]
    [SerializeField] private WeaponAttachmentDataSO attachment;

    public void OnAttachmentEquip()
    {
        CustomizationPanelUIManager manager =
            GetComponentInParent<CustomizationPanelUIManager>();

        if(manager == null || attachment == null) {
            Debug.LogWarning(
                "Cannot toggle attachment: manager or attachment is missing.",
                this);
            return;
        }

        manager.OnAttachmentEquipOnWeapon(attachment);
    }

    public void UpdateEquipText(bool isEquipped)
    {
        if(equipText != null)
            equipText.text = isEquipped ? "Unequip" : "Equip";

        if(attachmentEquipButton != null)
            attachmentEquipButton.interactable = true;
    }

    public void SetAttachmentBuyCard(
        WeaponAttachmentDataSO attachmentDataSO)
    {
        if(attachmentDataSO == null) {
            Debug.LogWarning(
                "Cannot initialize AttachmentBuyCard: attachment data is missing.",
                this);
            return;
        }

        attachment = attachmentDataSO;

        if(attachmentImage != null)
            attachmentImage.sprite = attachmentDataSO.attachmentIcon;

        if(attachmentName != null)
            attachmentName.text = attachmentDataSO.attachmentName;
    }

    public WeaponAttachmentDataSO Attachment => attachment;
}
