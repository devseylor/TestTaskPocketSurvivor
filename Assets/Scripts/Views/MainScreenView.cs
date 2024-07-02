using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainScreenView : ViewBase
{
    [Header("Equipment")]
    [SerializeField] private ItemView headArmor;
    [SerializeField] private ItemView bodyArmor;
    [SerializeField] private GameObject headIcon;
    [SerializeField] private GameObject bodyIcon;
    [SerializeField] private TMP_Text headArmorValue;
    [SerializeField] private TMP_Text bodyArmorValue;
    
    [Header("Sliders")]
    [SerializeField] private HealthSlider playerHealthSlider;
    [SerializeField] private HealthSlider enemyHealthSlider;

    [Header("Weapon Buttons")]
    [SerializeField] private TMP_Text dmgGunText;
    [SerializeField] private TMP_Text dmgMachineGunText;
    [SerializeField] private Image borderGunImage;
    [SerializeField] private Image borderMachineGunImage;
    [SerializeField] private Color selectedWeaponColor;
    [SerializeField] private Color notSelectedWeaponColor;
    [SerializeField] private Button gunButton; 
    [SerializeField] private Button machineGunButton;
    [SerializeField] private Button fireButton;

    [Header("Inventory")]
    [SerializeField] private List<ItemSlot> slots;
    [SerializeField] private DragItem dragItem;
    private int _selectedItemIndex;

    private List<Character> _characters = new List<Character>();
    private List<ItemDescription> _itemDescriptions = new List<ItemDescription>();

    public Action<int, ItemDescription> OnClickItemButton;
    public Action<int, int> OnRemoveItems;
    public Action<ECaliberType> OnClickSelectedGunButton;
    public Action OnClickFireButton;
    
    public void InitView(Character[] characters, List<ItemDescription> itemDescriptions)
    {
        _characters.Clear();
        _itemDescriptions.Clear();
        _characters.AddRange(characters);
        _itemDescriptions.AddRange(itemDescriptions);

        InitEquipment(_characters[0].headArmor, _characters[0].bodyArmor);
        
        playerHealthSlider.InitSlider(_characters[0].name, _characters[0].health, _characters[0].maxHealth);
        enemyHealthSlider.InitSlider(_characters[1].name, _characters[1].health, _characters[1].maxHealth);

        FillInventory();
        
        InitButtons(_characters[0], GetItemDescriptionForID(6), GetItemDescriptionForID(7));
    }

    private void FillInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].OnClickSlot = null;
            slots[i].OnDragItem = null;
            slots[i].OnEndDragItem = null;
            slots[i].OnDropItem = null;
            var tmpI = i;
            if (_characters[0].inventory.Length > i && _characters[0].inventory[i] != null)
            {
                slots[i].OnClickSlot += () =>
                    OnClickItemButton?.Invoke(tmpI, GetItemDescriptionForID(_characters[0].inventory[tmpI].id));

                slots[i].OnDragItem += ShowDragItem;
                

                slots[i].InitItem(tmpI, _characters[0].inventory[i],
                    GetItemDescriptionForID(_characters[0].inventory[i].id));
            }
            else
            {
                slots[i].ClearItem();
                slots[i].SetIndex(tmpI);
            }
            slots[i].OnDropItem += RemoveItems;
            slots[i].OnEndDragItem += HideDragItem;
        }
    }

    private void ShowDragItem(PointerEventData eventData, int index)
    {
        _selectedItemIndex = index;
        dragItem.gameObject.SetActive(true);
        dragItem.InitItem(_characters[0].inventory[index],
            GetItemDescriptionForID(_characters[0].inventory[index].id), eventData);
    }

    private void HideDragItem()
    {
        dragItem.gameObject.SetActive(false);
    }

    private void RemoveItems(int endIndex)
    {
        OnRemoveItems?.Invoke(_selectedItemIndex, endIndex);
    }
    
    private void InitEquipment(ItemData headItem, ItemData bodyItem)
    {
        if (headItem != null)
        {
            var headDescription = GetItemDescriptionForID(headItem.id);
            headArmor.gameObject.SetActive(true);
            headArmor.InitItem(headItem, headDescription);
            headIcon.SetActive(false);
            headArmorValue.text = headDescription.armorValue > 0 ? "+" + headDescription.armorValue : "0";
        }
        else
        {
            headArmor.gameObject.SetActive(false);
            headIcon.SetActive(true);
            headArmorValue.text = "0";
        }
        
        if (bodyItem != null)
        {
            var bodyDescription = GetItemDescriptionForID(bodyItem.id);
            bodyArmor.gameObject.SetActive(true);
            bodyArmor.InitItem(bodyItem, bodyDescription);
            bodyIcon.SetActive(false);
            bodyArmorValue.text = bodyDescription.armorValue > 0 ? "+" + bodyDescription.armorValue : "0";
        }
        else
        {
            bodyArmor.gameObject.SetActive(false);
            bodyIcon.SetActive(true);
            bodyArmorValue.text = "0";
        }
    }

    private void InitButtons(Character character, ItemDescription firstBullets, ItemDescription secondBullets)
    {
        dmgGunText.text = firstBullets.damage.ToString();
        dmgMachineGunText.text = (secondBullets.damage * 3).ToString();
        
        borderGunImage.color = character.caliberType == ECaliberType.Gun ? selectedWeaponColor : notSelectedWeaponColor;
        borderMachineGunImage.color = character.caliberType == ECaliberType.MachineGun ? selectedWeaponColor : notSelectedWeaponColor;
    }

    private ItemDescription GetItemDescriptionForID(uint id)
    {
        foreach (var itemDescription in _itemDescriptions)
        {
            if (id == itemDescription.id)
            {
                return itemDescription;
            }
        }

        return null;
    }
    
    private void OnEnable()
    {
        gunButton.onClick.AddListener(() => OnClickSelectedGunButton?.Invoke(ECaliberType.Gun));
        machineGunButton.onClick.AddListener(() => OnClickSelectedGunButton?.Invoke(ECaliberType.MachineGun));
        fireButton.onClick.AddListener(() => OnClickFireButton?.Invoke());
    }

    private void OnDisable()
    {
        gunButton.onClick.RemoveAllListeners();
        machineGunButton.onClick.RemoveAllListeners();
        fireButton.onClick.RemoveAllListeners();
    }
}
