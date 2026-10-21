
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private GameObject loadingPanel;

    [Header("Inventory")]
    [SerializeField] private InventoryUIManager inventoryUIManager;

    [Header("Main Cards")]
    [SerializeField] private MainCard[] mainCards;

    [Header("Loading Durations")]
    [SerializeField]
    private float[] loadingDurations =
    {
        0.1f,
        0.05f,
        0.1f,
        0.05f,
        0.1f
    };

    public GameObject MainPanel => mainPanel;
    public GameObject InventoryPanel => inventoryPanel;
    public GameObject CustomizationPanel => customizationPanel;
    public InventoryUIManager InventoryUIManager => inventoryUIManager;

    private Coroutine loadingCoroutine;

    // Attachment menu to open after loading finishes.
    private AttachmentType pendingAttachmentType;
    private bool openAttachmentMenuAfterLoading;

    private void Awake()
    {
        InitializeInventoryUIManager();

        if(loadingPanel != null) {
            loadingPanel.SetActive(false);
        }
    }

    private void Start()
    {
        OpenMainCards();
    }

    private void InitializeInventoryUIManager()
    {
        if(inventoryUIManager != null) {
            return;
        }

        if(inventoryPanel == null) {
            Debug.LogError(
                "Inventory panel reference is missing.",
                this);
            return;
        }

        inventoryUIManager =
            inventoryPanel.GetComponent<InventoryUIManager>();

        if(inventoryUIManager == null) {
            Debug.LogError(
                "Inventory panel is missing InventoryUIManager.",
                inventoryPanel);
        }
    }

    // Opens customization without automatically opening an attachment menu.
    public void OpenCustomizationPanel(InventoryItemSO itemData)
    {
        if(itemData == null) {
            Debug.LogWarning(
                "Cannot open customization: no item was selected.",
                this);
            return;
        }

        if(!ValidatePanelReferences()) {
            return;
        }

        CustomizationPanelUIManager customizationManager =
            customizationPanel.GetComponent<CustomizationPanelUIManager>();

        if(customizationManager == null) {
            Debug.LogError(
                "Customization panel is missing CustomizationPanelUIManager.",
                customizationPanel);
            return;
        }

        // Cancel any previously queued attachment menu.
        openAttachmentMenuAfterLoading = false;

        ResetMainCardAnimations();

        // Prepare the selected weapon before displaying the panel.
        customizationManager.OnCustomizeWeapon(itemData);

        SwitchPanel(customizationPanel);
    }

    // Opens customization, then opens the selected attachment menu
    // after the loading sequence has completed.
    public void OpenCustomizationAttachmentButtonClick(
        InventoryItemSO itemData,
        AttachmentType attachmentType)
    {

        if(itemData == null) {
            Debug.LogWarning(
                "No item was selected.",
                this);
            return;
        }

        if(!ValidatePanelReferences()) {
            return;
        }

        CustomizationPanelUIManager customizationManager =
            customizationPanel.GetComponent<CustomizationPanelUIManager>();

        if(customizationManager == null) {
            Debug.LogError(
                "CustomizationPanelUIManager is missing.",
                customizationPanel);
            return;
        }

        ResetMainCardAnimations();

        // Prepare the selected weapon first.
        customizationManager.OnCustomizeWeapon(itemData);

        // Remember which attachment menu should open after loading.
        pendingAttachmentType = attachmentType;
        openAttachmentMenuAfterLoading = true;

        Debug.Log(
            "Starting panel transition. Attachment: " + attachmentType,
            this);

        SwitchPanel(customizationPanel);
    }

    public void OpenMainCards()
    {
        if(!ValidatePanelReferences()) {
            return;
        }

        openAttachmentMenuAfterLoading = false;

        ResetMainCardAnimations();

        SwitchPanel(mainPanel);
    }

    public void OpenInventoryPanel()
    {
        if(!ValidatePanelReferences()) {
            return;
        }

        openAttachmentMenuAfterLoading = false;

        SwitchPanel(inventoryPanel);
    }

    private void SwitchPanel(GameObject targetPanel)
    {
        if(targetPanel == null || loadingPanel == null) {
            return;
        }

        // Stop any transition already in progress.
        if(loadingCoroutine != null) {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        loadingCoroutine =
            StartCoroutine(SwitchPanelRoutine(targetPanel));
    }

    private IEnumerator SwitchPanelRoutine(GameObject targetPanel)
    {
        // Hide the regular panels.
        if(mainPanel != null) {
            mainPanel.SetActive(false);
        }

        if(inventoryPanel != null) {
            inventoryPanel.SetActive(false);
        }

        if(customizationPanel != null) {
            customizationPanel.SetActive(false);
        }

        // Display the loading panel.
        loadingPanel.SetActive(true);

        if(loadingDurations != null) {
            foreach(float duration in loadingDurations) {
                yield return new WaitForSeconds(
                    Mathf.Max(0f, duration));
            }
        }

        // Finish loading before showing the target panel.
        loadingPanel.SetActive(false);
        targetPanel.SetActive(true);

        // Open the pending attachment menu only after customization
        // has become active.
        if(targetPanel == customizationPanel &&
            openAttachmentMenuAfterLoading) {
            // Clear the flag first to prevent duplicate opening.
            openAttachmentMenuAfterLoading = false;

            CustomizationPanelUIManager customizationManager =
                customizationPanel.GetComponent<CustomizationPanelUIManager>();

            if(customizationManager != null) {
                customizationManager.OpenAttachmentBuyPanel(
                    pendingAttachmentType);
            }
            else {
                Debug.LogError(
                    "Cannot open attachment menu: " +
                    "CustomizationPanelUIManager is missing.",
                    customizationPanel);
            }
        }

        loadingCoroutine = null;
    }

    private bool ValidatePanelReferences()
    {
        if(mainPanel == null ||
            inventoryPanel == null ||
            customizationPanel == null ||
            loadingPanel == null) {
            return false;
        }

        return true;
    }

    private void ResetMainCardAnimations()
    {
        if(mainCards == null) {
            return;
        }

        foreach(MainCard card in mainCards) {
            if(card != null) {
                card.ResetAttachmentCardAnimation();
            }
        }
    }
}
