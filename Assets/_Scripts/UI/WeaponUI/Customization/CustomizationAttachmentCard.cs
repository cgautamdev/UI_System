using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationAttachmentCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AttachmentType attachmentType;
    public TextMeshProUGUI attachmentName;
    public Image attachmentIcon;

    public UnityEvent PointerEnter;
    public UnityEvent PointerExit;

    private void Start()
    {
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
        if(manager != null) {
            manager.OpenAttachmentBuyPanel(attachmentType);
        }
    }
    public void ChangeAttachmentCardIcon(WeaponAttachmentDataSO weaponAttachmentDataSO)
    {
        attachmentIcon.sprite= weaponAttachmentDataSO.attachmentIcon;
    }
}
