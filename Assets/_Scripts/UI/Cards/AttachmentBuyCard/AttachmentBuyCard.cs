using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentBuyCard : MonoBehaviour
{
    public Image attachmentImage;
    public TextMeshProUGUI attachmentName;
    public Button attachmentEquipButton;
    public WeaponAttachmentDataSO attachment;
    public TextMeshProUGUI equipText;

    public void OnAttachmentEquip()
    {
        CustomizationPanelUIManager manager = GetComponentInParent<CustomizationPanelUIManager>();
        manager.OnAttachmentEquipOnWeapon(attachment);


    }

    public void UpdateEquipText(bool isEquipped)
    {
        equipText.text = isEquipped ? "Equipped" : "Equip";
        attachmentEquipButton.interactable = !isEquipped;
    }

    public void SetAttachmentBuyCard(WeaponAttachmentDataSO attachmentDataSO)
    {
        attachmentImage.sprite = attachmentDataSO.attachmentIcon;
        attachmentName.text = attachmentDataSO.attachmentName;
        attachment = attachmentDataSO;
    }

}
