using UnityEngine;

public class InventoryCategoryPanel : MonoBehaviour
{
    [SerializeField] private InventoryItemType category;

    public InventoryItemType Category => category;
}
