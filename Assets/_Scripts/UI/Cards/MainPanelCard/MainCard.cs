using UnityEngine;

public class MainCard : MonoBehaviour
{
    public InventoryItemType cardType;
    public MainPanelUIManager mainPanelUIManager;

    public void OpenInventory()
    {
        mainPanelUIManager.OpenInventoryCategory(cardType);
    }
}
