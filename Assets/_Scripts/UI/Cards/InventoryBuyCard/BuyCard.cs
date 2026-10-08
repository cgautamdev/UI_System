using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI itemName;
    public Image itemImage;

    public UnityEvent OnCardEnter;
    public UnityEvent OnCardExit;

    public Button customizeButton;
    public Button equipButton;

    private void Start()
    {
        customizeButton.onClick.AddListener(OnCustomizeWeaponButtonPressed);
        equipButton.onClick.AddListener(OnEquipWeaponButtonPressed);
    }

    public void OnEquipWeaponButtonPressed()
    {
        OnEquipWeapon();
    }
    public void OnCustomizeWeaponButtonPressed()
    {
        OnCustomizeWeapon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCardEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCardExit?.Invoke();
    }

    // functions
    public void OnEquipWeapon()
    {
        Debug.Log("Equip: " +  itemName.text);
    }

    public void OnCustomizeWeapon()
    {
        Debug.Log("Customize: " + itemName.text);
    }
}
