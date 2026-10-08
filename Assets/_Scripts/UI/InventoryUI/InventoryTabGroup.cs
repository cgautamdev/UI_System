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

    public void OnSubscribe(InventoryTabButton button)
    {
        if(button == null) {
            tabButtons = new List<InventoryTabButton>();
        }
        tabButtons.Add(button);
    }

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

        int index = tabButtons.IndexOf(button);
        for(int i = 0; i < panels.Count; i++) {
            if(i == index) {
                panels[i].SetActive(true);
            }
            else {
                panels[i].SetActive(false);
            }
        }
    }

    // for main card to call
    public void OpenCategory(InventoryItemType category)
    {
        for(int i = 0;i < tabButtons.Count; i++) {
            InventoryTabButton tab = tabButtons[i];
            bool isTargetTab = tab.type == category;

            tab.gameObject.SetActive(isTargetTab);
            if(isTargetTab) {
                OnTabEnter(tab);
            }
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
