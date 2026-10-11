using System.Collections.Generic;
using UnityEngine;

public enum AttachmentType
{
    Muzzle,
    Barrel,
    Optic,
    Magazine,
    Stock
}

[CreateAssetMenu(fileName = "AttachmentData", menuName = "Inventory/Items/WeaponAttachments")]
public class WeaponAttachmentDataSO : ScriptableObject
{
    public AttachmentType attachmentType;
    public string attachmentName;
    public Sprite attachmentIcon;

    [Header("Compatibility")]
    public List<WeaponClass> compatibleWeapons;
}
