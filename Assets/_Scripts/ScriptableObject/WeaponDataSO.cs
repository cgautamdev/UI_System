using UnityEngine;

public enum WeaponClass
{
    AR,
    SMG,
    Sniper,
    Pistol
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/Items/Weapon")]
public class WeaponDataSO : InventoryItemSO
{
    public WeaponClass WeaponClass;

    // other weaopn details later
}
