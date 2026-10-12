using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject inventoryPanel;
    public GameObject customizationPanel;
    public InventoryUIManager inventoryUIManager;

    private void Start()
    {
        OpenMainCards();
        inventoryUIManager = inventoryPanel.GetComponent<InventoryUIManager>();
    }

    public void OpenCustomizationPanel(InventoryItemSO itemData)
    {
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        customizationPanel.SetActive(true);

        customizationPanel.GetComponent<CustomizationPanelUIManager>().OnCustomizeWeapon(itemData);
    }

    public void OpenMainCards()
    {
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        customizationPanel.SetActive(false);
    }
}
