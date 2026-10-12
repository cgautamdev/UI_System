using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCustomizationCard : MonoBehaviour
{
    public Image weaponImage;
    public List<CustomizationAttachmentCard> cards;

    public void SetWeaponPreviewCard(InventoryItemSO item)
    {
        weaponImage.sprite = item.itemSprite;
    }
}
