using System.Collections.Generic;
using UnityEngine;

public class InventoryTabGroup : MonoBehaviour
{
    [Header("Tab References")]
    [SerializeField]
    private List<InventoryTabButton> tabButtons =
        new List<InventoryTabButton>();

    [SerializeField]
    private List<GameObject> panels =
        new List<GameObject>();

    [Header("Tab Colors")]
    [SerializeField] private Color tabIdleColor = Color.white;
    [SerializeField] private Color tabHoverColor = Color.gray;
    [SerializeField] private Color tabActiveColor = Color.white;

    [Header("Selection")]
    [SerializeField] private InventoryTabButton selectedTab;

    private void Awake()
    {
        AssignTabGroups();
    }

    private void AssignTabGroups()
    {
        if(tabButtons == null)
            return;

        foreach(InventoryTabButton button in tabButtons) {
            if(button != null)
                button.SetTabGroup(this);
        }
    }

    public void OnTabEnter(InventoryTabButton button)
    {
        if(button == null)
            return;

        ResetTabs();

        if(button != selectedTab && button.BackgroundImage != null)
            button.BackgroundImage.color = tabHoverColor;
    }

    public void OnTabExit(InventoryTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(InventoryTabButton button)
    {
        if(button == null)
            return;

        selectedTab = button;

        ResetTabs();

        if(button.BackgroundImage != null)
            button.BackgroundImage.color = tabActiveColor;

        SetCategoryPanelVisibility(button.Type);
    }

    private void SetCategoryPanelVisibility(InventoryItemType category)
    {
        if(panels == null)
            return;

        foreach(GameObject panel in panels) {
            if(panel == null)
                continue;

            InventoryCategoryPanel categoryPanel = panel.GetComponent<InventoryCategoryPanel>();

            if(categoryPanel == null)
                continue;

            panel.SetActive(categoryPanel.Category == category);
        }
    }

    public void OnTabSelectedFromCard(InventoryItemType category)
    {
        if(tabButtons == null)
            return;

        InventoryTabButton targetButton = null;

        foreach(InventoryTabButton button in tabButtons) {
            if(button != null && button.Type == category) {
                targetButton = button;
                break;
            }
        }

        if(targetButton == null) {
            Debug.LogWarning($"No inventory tab found for category '{category}'.", this);

            OpenFullInventoryTabs();
            return;
        }

        // Show only the requested category tab.
        foreach(InventoryTabButton button in tabButtons) {
            if(button != null)
                button.gameObject.SetActive(button == targetButton);
        }

        OnTabSelected(targetButton);
    }

    public void OpenFullInventoryTabs()
    {
        if(tabButtons == null)
            return;

        foreach(InventoryTabButton button in tabButtons) {
            if(button != null)
                button.gameObject.SetActive(true);
        }

        InventoryTabButton firstValidButton = null;

        foreach(InventoryTabButton button in tabButtons) {
            if(button == null)
                continue;

            firstValidButton = button;
            break;
        }

        if(firstValidButton != null) {
            OnTabSelected(firstValidButton);
            return;
        }

        selectedTab = null;
        ResetTabs();
        HideAllCategoryPanels();
    }

    public void ResetTabs()
    {
        if(tabButtons == null)
            return;

        foreach(InventoryTabButton button in tabButtons) {
            if(button == null || button.BackgroundImage == null)
                continue;

            button.BackgroundImage.color = button == selectedTab ? tabActiveColor : tabIdleColor;
        }
    }

    private void HideAllCategoryPanels()
    {
        if(panels == null)
            return;

        foreach(GameObject panel in panels) {
            if(panel != null)
                panel.SetActive(false);
        }
    }
}