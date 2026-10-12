using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCustomizationCard : MonoBehaviour
{
    [Header("Weapon Preview")]
    [SerializeField] private Image weaponImage;

    [Header("Attachment Slots")]
    [SerializeField]
    private List<CustomizationAttachmentCard> cards =
        new List<CustomizationAttachmentCard>();

    public List<CustomizationAttachmentCard> Cards => cards;

    public void SetWeaponPreviewCard(InventoryItemSO item)
    {
        if(item == null) {
            Debug.LogWarning(
                "Cannot set weapon preview: item is null.",
                this);
            return;
        }

        if(weaponImage == null) {
            Debug.LogError(
                "Weapon preview Image reference is missing.",
                this);
            return;
        }

        weaponImage.sprite = item.itemSprite;
    }
}