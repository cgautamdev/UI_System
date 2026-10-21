
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainAttachmentCard : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Panel References")]
    [SerializeField] private CustomizationPanelUIManager customizationPanelManager;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private AttachmentType attachmentType;

    [SerializeField] private UnityEvent PointerEnter;
    [SerializeField] private UnityEvent PointerExit;

    public UnityEvent OnCardClick;

    public void OnAttachmentCardClick()
    {
        if(uiManager == null) {
            Debug.LogWarning("UIManager reference is missing.", this);
            return;
        }

        MainCard mainCard = GetComponentInParent<MainCard>();

        if(mainCard == null) {
            Debug.LogWarning("MainCard was not found in the parent hierarchy.", this);
            return;
        }

        if(mainCard.ItemData == null) {
            Debug.LogWarning("No weapon is assigned to this card.", this);
            return;
        }

        Debug.Log("Click: " + mainCard.ItemData.name);
        Debug.Log("Calling OpenCustomizationAttachmentButtonClick...");

        uiManager.OpenCustomizationAttachmentButtonClick(
            mainCard.ItemData,
            attachmentType
        );

        Debug.Log("OpenCustomizationAttachmentButtonClick returned.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke();
    }
}
