using System.Collections.Generic;
using UnityEngine;

public class MainPanelUIManager : MonoBehaviour
{
    public InventoryTabGroup tabGroup;

    public List<MainCard> cards;

    public void OpenInventoryCategory(InventoryItemType type)
    {
        tabGroup.OpenCategory(type);
    }
}
