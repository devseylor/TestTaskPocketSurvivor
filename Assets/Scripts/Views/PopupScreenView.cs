using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScreenView : ViewBase
{
    [SerializeField] private Image itemImage;

    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text weightText;

    [SerializeField] private GameObject healIcon;
    [SerializeField] private GameObject armorIcon;
    [SerializeField] private GameObject bulletIcon;
    
    [SerializeField] private TMP_Text confirmText;
    
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button closeButton;

    public Action OnClickConfirmButton;
    public Action OnClickDeleteButton;
    public Action OnClickCloseButton;

    public void InitView(ItemDescription itemDescription)
    {
        labelText.text = itemDescription.name;
        itemImage.sprite = itemDescription.icon;
        weightText.text = itemDescription.weight + "кг";

        healIcon.SetActive(false);
        armorIcon.SetActive(false);
        bulletIcon.SetActive(false);
        switch (itemDescription.type)
        {
            case EItemType.Heal:
                valueText.text = itemDescription.healValue.ToString();
                healIcon.SetActive(true);
                confirmText.text = "Лечить";
                break;
            case EItemType.Armor:
                valueText.text = "+" + itemDescription.armorValue;
                armorIcon.SetActive(true);
                confirmText.text = "Экипировать";
                break;
            case EItemType.Bullet:
                valueText.text = itemDescription.damage.ToString();
                bulletIcon.SetActive(true);
                confirmText.text = "Купить";
                break;
        }
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(() => OnClickConfirmButton?.Invoke());
        deleteButton.onClick.AddListener(() => OnClickDeleteButton?.Invoke());
        closeButton.onClick.AddListener(() => OnClickCloseButton?.Invoke());
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }
}
