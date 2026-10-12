using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTabButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Tab Configuration")]
    [SerializeField] private InventoryTabGroup tabGroup;
    [SerializeField] private InventoryItemType type;

    [Header("Visuals")]
    [SerializeField] private Image backgroundImage;

    public InventoryItemType Type => type;
    public Image BackgroundImage => backgroundImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(tabGroup == null) {
            Debug.LogWarning(
                "Cannot select tab: InventoryTabGroup is missing.",
                this);
            return;
        }

        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tabGroup != null)
            tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tabGroup != null)
            tabGroup.OnTabExit(this);
    }

    public void SetTabGroup(InventoryTabGroup group)
    {
        tabGroup = group;
    }
}