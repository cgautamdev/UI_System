using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainCard : MonoBehaviour
{
    public InventoryItemType cardType;
    public MainPanelUIManager mainPanelUIManager;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI cardCategoryText;
    public Image itemImage;
    public GameObject emptyItem;

    public void OpenInventoryCategory()
    {
        mainPanelUIManager.OpenInventoryWithCategory(cardType);
    }

    public void SetMenuCard(InventoryItemSO item)
    {
        emptyItem.SetActive(false);
        itemName.text = item.itemName;
        itemImage.sprite = item.itemSprite;
    }
}
