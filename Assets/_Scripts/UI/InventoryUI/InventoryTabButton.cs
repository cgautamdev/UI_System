using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public InventoryTabGroup tabGroup;
    public InventoryItemType type;
    public Image backgroundImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        tabGroup.OnSubscribe(this);
    }
}
