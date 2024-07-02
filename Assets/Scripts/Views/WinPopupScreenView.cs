using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopupScreenView : ViewBase
{
    [SerializeField] private ItemView item;
    
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text weightText;

    [SerializeField] private GameObject healIcon;
    [SerializeField] private GameObject armorIcon;
    [SerializeField] private GameObject bulletIcon;
    
    [SerializeField] private Button takeRewardButton;
    
    public Action OnClickTakeRewardButton;

    public void InitView(ItemData itemData, ItemDescription itemDescription)
    {
        labelText.text = itemDescription.name;
        item.InitItem(itemData, itemDescription);
        weightText.text = itemDescription.weight + "кг";

        healIcon.SetActive(false);
        armorIcon.SetActive(false);
        bulletIcon.SetActive(false);
        switch (itemDescription.type)
        {
            case EItemType.Heal:
                valueText.text = itemDescription.healValue.ToString();
                healIcon.SetActive(true);
                break;
            case EItemType.Armor:
                valueText.text = "+" + itemDescription.armorValue;
                armorIcon.SetActive(true);
                break;
            case EItemType.Bullet:
                valueText.text = itemDescription.damage.ToString();
                bulletIcon.SetActive(true);
                break;
        }
    }
    
    private void OnEnable()
    {
        takeRewardButton.onClick.AddListener(() => OnClickTakeRewardButton?.Invoke());
    }

    private void OnDisable()
    {
        takeRewardButton.onClick.RemoveAllListeners();
    }
}