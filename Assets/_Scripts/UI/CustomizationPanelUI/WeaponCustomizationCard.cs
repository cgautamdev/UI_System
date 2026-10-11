using UnityEngine;
using UnityEngine.UI;

public class WeaponCustomizationCard : MonoBehaviour
{
    public Image weaponImage;

    public void SetWeaponPreviewCard(InventoryItemSO item)
    {
        weaponImage.sprite = item.itemSprite;
    }
}
