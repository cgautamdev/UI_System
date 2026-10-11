using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryTabGroup : MonoBehaviour
{
    public List<InventoryTabButton> tabButtons;
    public List<GameObject> panels;
    [SerializeField] private Color _tabIdleColor;
    [SerializeField] private Color _tabHoverColor;
    [SerializeField] private Color _tabActiveColor;

    public InventoryTabButton selectedTab;

    public void OnTabEnter(InventoryTabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.backgroundImage.color = _tabHoverColor;
        }
    }

    public void OnTabExit(InventoryTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(InventoryTabButton button)
    {
        selectedTab = button;

        ResetTabs();
        button.backgroundImage.color = _tabActiveColor;

        foreach(GameObject panel in panels) {
            if(!panel.TryGetComponent<InventoryCategoryPanel>(out var panelCategory))
                continue;

            panel.SetActive(panelCategory.category == button.type);
        }
    }

    public void OnTabSelectedFromCard(InventoryItemType category)
    {
        foreach(InventoryTabButton button in tabButtons) {
            bool isTarget = button.type == category;
            button.gameObject.SetActive(isTarget);
            if(isTarget) {
                OnTabSelected(button);
            }
        }
    }

    public void OpenFullInventoryTabs()
    {
        foreach (InventoryTabButton button in tabButtons) {
            button.gameObject.SetActive(true);
        }
        if(tabButtons.Count > 0){
            OnTabSelected(tabButtons[0]);
        }
    }

    public void ResetTabs()
    {
        foreach(InventoryTabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) {
                continue;
            }
            button.backgroundImage.color = _tabIdleColor;
        }
    }
}
