using UnityEngine;
using Random = UnityEngine.Random;

public class GameCore : MonoBehaviour
{
    [SerializeField] private WindowsState windowsState;
    [SerializeField] private ItemsContent itemsContent;
    
    private SaveData _data;
    private EArmorType _currentArmorType = EArmorType.HeadArmor;

    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        _data = SaveManager.LoadData();

        OpenMainScreen();
    }

    private void OpenMainScreen()
    {
        windowsState.SetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items);
    }

    public void OpenPopupScreen(int index, ItemDescription itemDescription)
    {
        windowsState.SetPopupWindow<PopupScreenVM>().InitWindow(index, itemDescription);
    }

    private void OpenGameOverScreen()
    {
        windowsState.SetPopupWindow<GameOverScreenVM>();
    }

    private void OpenWinScreen()
    {
        var reward = itemsContent.items[Random.Range(0, itemsContent.items.Count)];
        var rewardData = new ItemData {id = reward.id, count = reward.maxCount};
        windowsState.SetPopupWindow<WinPopupScreenVM>().InitWindow(rewardData, reward);
    }
    
    public void TryRemoveItems(int startIndex, int endIndex)
    {
        if (_data.characters[0].inventory[startIndex] == null) { return; }

        (_data.characters[0].inventory[startIndex], _data.characters[0].inventory[endIndex]) =
            (_data.characters[0].inventory[endIndex], _data.characters[0].inventory[startIndex]);
        
        windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items);

        SaveManager.SaveData(_data);
    }

    public void TryFire()
    {
        var currentCalibre = _data.characters[0].caliberType;
        var bullet = itemsContent.items.Find(x => x.type == EItemType.Bullet && x.caliberType == currentCalibre);
        var howManyBullets = currentCalibre == ECaliberType.Gun ? 1 : 3;
        bool playerShot = false;
        var countBullets = 0;
        
        for (int i = 0; i < _data.characters[0].inventory.Length && countBullets < howManyBullets; i++)
        {
            if (_data.characters[0].inventory[i]?.id == bullet.id)
            {
                playerShot = true;
                var count = _data.characters[0].inventory[i].count < howManyBullets - countBullets
                    ? _data.characters[0].inventory[i].count
                    : howManyBullets;
                DeleteItem(i, count);
                countBullets += count;
            }
        }

        DamageToCharacter(1, bullet.damage * countBullets, EArmorType.BodyArmor);


        if (playerShot)
        {
            if (_data.characters[1].health > 0f)
            {
                DamageToCharacter(0, 15f, _currentArmorType);
                if (_data.characters[0].health <= 0)
                {
                    OpenGameOverScreen();
                }
                _currentArmorType = _currentArmorType == EArmorType.HeadArmor
                    ? EArmorType.BodyArmor
                    : EArmorType.HeadArmor;
            }
            else
            {
                OpenWinScreen();
            }
        }
        else
        {
            OpenPopupScreen(0, bullet);
        }

        windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items);

        SaveManager.SaveData(_data);
    }

    private void DamageToCharacter(int character,float damage, EArmorType armorType)
    {
        var armor = 0f;
        if (armorType == EArmorType.HeadArmor && _data.characters[character].headArmor != null)
        {
            armor = itemsContent.items.Find(x => x.id == _data.characters[character].headArmor.id).armorValue;
        }
        if (armorType == EArmorType.BodyArmor && _data.characters[character].bodyArmor != null)
        {
            armor = itemsContent.items.Find(x => x.id == _data.characters[character].bodyArmor.id).armorValue;
        }

        var resultDamage = armor > damage ? 0 : damage - armor;

        _data.characters[character].health = _data.characters[character].health > resultDamage
            ? _data.characters[character].health - resultDamage
            : 0f;
    }
    
    public void TrySetSelectedGun(ECaliberType caliberType)
    {
        if (_data.characters[0].caliberType == caliberType) { return; }

        _data.characters[0].caliberType = caliberType;
        windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items);
        
        SaveManager.SaveData(_data);
    }

    public void HealPlayer(int index)
    {
        if (_data.characters[0].health >= _data.characters[0].maxHealth) { return; }

        var heal = itemsContent.items.Find(x => x.id == _data.characters[0].inventory[index].id);
        
        if (_data.characters[0].health + heal.healValue < _data.characters[0].maxHealth)
        {
            _data.characters[0].health += heal.healValue;
        }
        else
        {
            _data.characters[0].health = _data.characters[0].maxHealth;
        }
        
        DeleteItem(index, needUpdate: true, needSave: true);
    }
    
    public void EquipArmor(int index)
    {
        var armor = itemsContent.items.Find(x => x.id == _data.characters[0].inventory[index].id);

        DeleteItem(index);
        
        if (armor.armorType == EArmorType.HeadArmor)
        {
            if (_data.characters[0].headArmor != null) { AddItem(_data.characters[0].headArmor.id); }
            _data.characters[0].headArmor = new ItemData {id = armor.id, count = 1};
        }
        else
        {
            if (_data.characters[0].bodyArmor != null) { AddItem(_data.characters[0].bodyArmor.id); }
            _data.characters[0].bodyArmor = new ItemData {id = armor.id, count = 1};
        }
        
        windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items);
        
        SaveManager.SaveData(_data);
    }

    public void AddItem(uint id, bool needUpdate = false, bool needSave = false)
    {
        var newItem = itemsContent.items.Find(x => x.id == id);
        for (int i = 0; i < _data.characters[0].inventory.Length; i++)
        {
            if (_data.characters[0].inventory[i] == null)
            {
                _data.characters[0].inventory[i] = new ItemData {id = newItem.id, count = newItem.maxCount};
                break;
            }
        }

        if (needUpdate) { windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items); }

        if (needSave) { SaveManager.SaveData(_data); }
    }
    
    public void DeleteItem(int index, int count = 1, bool needUpdate = false, bool needSave = false)
    {
        if (_data.characters[0].inventory[index].count > count)
        {
            _data.characters[0].inventory[index].count -= count;
        }
        else
        {
            _data.characters[0].inventory[index] = null;
        }
        
        if (needUpdate) { windowsState.GetWindow<MainScreenVM>().InitWindow(_data.characters, itemsContent.items); }

        if (needSave) { SaveManager.SaveData(_data); }
    }

    public void NewEnemy()
    {
        _data.characters[1].health = _data.characters[0].maxHealth;
    }

    public void ReloadGame()
    {
        SaveManager.ResetData();
        NewEnemy();
        StartGame();
    }
}
