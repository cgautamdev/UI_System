using TMPro;
using UnityEngine;

public class CustomizationPanelUIManager : MonoBehaviour
{
    public Transform weaponPreviewContainer;
    public TextMeshProUGUI attachmentName;

    public GameObject weaponCustomizationPreviewContainer;

    public void OnCustomizeWeapon(InventoryItemSO weapon)
    {
        GameObject customizationCard = Instantiate(weaponCustomizationPreviewContainer, weaponPreviewContainer);
        WeaponCustomizationCard cardScript = customizationCard.GetComponent<WeaponCustomizationCard>();

        cardScript.SetWeaponPreviewCard(weapon);
    }
}
