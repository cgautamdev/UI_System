using UnityEngine;
using System.Collections.Generic;
public enum WeaponClass
{
    AR,
    SMG,
    Sniper,
    Pistol,
    Melee,
    Launchers
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/Items/Weapon")]
public class WeaponDataSO : InventoryItemSO
{
    public WeaponClass weaponClass;

    public List<WeaponAttachmentDataSO> equippedAttachments;
    // other weaopn details later
}
