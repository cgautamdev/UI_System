using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject customizationPanel;

    [Header("Inventory")]
    [SerializeField] private InventoryUIManager inventoryUIManager;

    public GameObject MainPanel => mainPanel;
    public GameObject InventoryPanel => inventoryPanel;
    public GameObject CustomizationPanel => customizationPanel;
    public InventoryUIManager InventoryUIManager => inventoryUIManager;

    private void Awake()
    {
        InitializeInventoryUIManager();
    }

    private void Start()
    {
        OpenMainCards();
    }

    private void InitializeInventoryUIManager()
    {
        if(inventoryUIManager != null)
            return;

        if(inventoryPanel == null) {
            Debug.LogError("Inventory panel reference is missing.", this);
            return;
        }

        inventoryUIManager = inventoryPanel.GetComponent<InventoryUIManager>();

        if(inventoryUIManager == null) {
            Debug.LogError("Inventory panel is missing InventoryUIManager.",inventoryPanel);
        }
    }

    public void OpenCustomizationPanel(InventoryItemSO itemData)
    {
        if(itemData == null) {
            Debug.LogWarning("Cannot open customization: no item was selected.", this);
            return;
        }

        if(!ValidatePanelReferences())
            return;

        CustomizationPanelUIManager customizationManager = customizationPanel.GetComponent<CustomizationPanelUIManager>();

        if(customizationManager == null) {
            Debug.LogError("Customization panel is missing CustomizationPanelUIManager.", customizationPanel);
            return;
        }

        // Prepare the selected weapon before displaying the panel.
        customizationManager.OnCustomizeWeapon(itemData);

        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        customizationPanel.SetActive(true);
    }

    public void OpenMainCards()
    {
        if(!ValidatePanelReferences())
            return;

        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        customizationPanel.SetActive(false);
    }

    private bool ValidatePanelReferences()
    {
        if(mainPanel == null ||
            inventoryPanel == null ||
            customizationPanel == null) {
            Debug.LogError("UIManager is missing a panel reference. " + "Assign Main, Inventory, and Customization panels in the Inspector.", this);
            return false;
        }

        return true;
    }
}