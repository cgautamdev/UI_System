using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomizationAttachmentCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AttachmentType attachmentType;
    public TextMeshProUGUI attachmentName;

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
}
