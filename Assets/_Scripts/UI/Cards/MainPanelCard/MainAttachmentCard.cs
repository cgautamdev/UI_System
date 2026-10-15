using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainAttachmentCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Panel References")]
    [SerializeField] private CustomizationPanelUIManager customizationPanelManager;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private AttachmentType attachmentType;
    [SerializeField] private UnityEvent PointerEnter;
    [SerializeField] private UnityEvent PointerExit;

    // to be called by the main card attachment icons
    public void OnAttachmentCardClick()
    {
        // CustomizationPanelUIManager manager = FindFirstObjectByType<CustomizationPanelUIManager>();

        if(customizationPanelManager == null) {
            Debug.LogWarning("CustomizationPanelUIManager was not found in the parent hierarchy.",this);
            return;
        }

        Debug.Log("Clicked");

        uiManager.OpenCustomizationPanel(GetComponentInParent<MainCard>().ItemData);
        customizationPanelManager.OpenAttachmentBuyPanel(attachmentType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"ENTER: {name}");
        PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"EXIT: {name}");
        PointerExit?.Invoke();
    }
}
