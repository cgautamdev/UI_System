using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CustomizationPanelUIManager : MonoBehaviour
{
    public Transform weaponPreviewContainer;
    public TextMeshProUGUI attachmentName;
    public InventoryItemSO selectedWeapon;

    public List<WeaponAttachmentDataSO> attachmentInventory;
    public GameObject weaponCustomizationPreviewContainer;
    public List<WeaponAttachmentDataSO> tempAttachments = new();

    [Header("Open attachments")]
    public AttachmentBuyCard attachmentBuyCard;
    public Transform attachmentBuyCardSpawn;

    private void Start()
    {
        attachmentInventory = InventoryManager.Instance.availableAttachments;
        
    }

    public void OnCustomizeWeapon(InventoryItemSO weapon)
    {
        selectedWeapon = weapon;
        tempAttachments.Clear();

        GameObject customizationCard = Instantiate(weaponCustomizationPreviewContainer, weaponPreviewContainer);
        WeaponCustomizationCard cardScript = customizationCard.GetComponent<WeaponCustomizationCard>();

        cardScript.SetWeaponPreviewCard(weapon);
    }

    public void OpenAttachmentBuyPanel(AttachmentType attachmentType)
    {
        ClearAttachmentBuyCards();

        // change panel name
        attachmentName.text = attachmentType.ToString();

        List<WeaponAttachmentDataSO> attachments = FindPossibleAttachments(attachmentType);
        // spawn these attachments inside the transform
        foreach (WeaponAttachmentDataSO attachment in attachments) {
            AttachmentBuyCard card = Instantiate(attachmentBuyCard, attachmentBuyCardSpawn);
            card.SetAttachmentBuyCard(attachment);
        }
    }

    public void ClearAttachmentBuyCards()
    {
        // clear all the previous cards
        foreach(Transform child in attachmentBuyCardSpawn) {
            Destroy(child.gameObject);
        }
    }

    public List<WeaponAttachmentDataSO> FindPossibleAttachments(AttachmentType attachmentType)
    {
        List<WeaponAttachmentDataSO> temp = new();

        foreach(WeaponAttachmentDataSO attachment in attachmentInventory) {
            if(attachment.attachmentType == attachmentType && attachment.compatibleWeapons.Contains((selectedWeapon as WeaponDataSO).weaponClass)) {
                    temp.Add(attachment);
            }
        }

        return temp;
    }

    public bool CanEquipAttachment(WeaponAttachmentDataSO attachment)
    {
        if(selectedWeapon == null || attachment == null)
            return false;

        if(tempAttachments.Contains(attachment))
            return false;

        tempAttachments.RemoveAll(
            x => x.attachmentType == attachment.attachmentType
        );

        return true;
    }

    public void OnAttachmentEquipOnWeapon(WeaponAttachmentDataSO attachment)
    {
        if(!CanEquipAttachment(attachment))
            return;

        tempAttachments.RemoveAll(
            x => x.attachmentType == attachment.attachmentType);

        tempAttachments.Add(attachment);

        foreach(CustomizationAttachmentCard card in GetComponentInChildren<WeaponCustomizationCard>().cards) {
            if(card.attachmentType == attachment.attachmentType) {
                card.ChangeAttachmentCardIcon(attachment);
            }
        }
    }

    public void OnBackButtonClick()
    {
        ClearAttachmentBuyCards();
        tempAttachments.Clear();
    }

    public void OnConfirmWeaponCustomization()
    {
        if(selectedWeapon == null)
            return;

        switch(selectedWeapon.itemType) {
            case InventoryItemType.PrimaryWeapon:
                InventoryManager.Instance.primaryWeaponAttachments.Clear();
                InventoryManager.Instance.primaryWeaponAttachments.AddRange(tempAttachments);
                break;

            case InventoryItemType.SecondaryWeapon:
                InventoryManager.Instance.secondaryWeaponAttachments.Clear();
                InventoryManager.Instance.secondaryWeaponAttachments.AddRange(tempAttachments);
                break;
        }

        // equip to main card
        tempAttachments.Clear();
        InventoryManager.Instance.CheckWeaponEquipStateAndEquip(selectedWeapon);
    }
}
