using UnityEngine;

public class PopupScreenVM : ViewModelBase<PopupScreenView>
{
    public PopupScreenVM(Canvas canvas, GameCore gameCore) : base(canvas, gameCore) { }

    private int _selectedItemIndex;
    private ItemDescription _itemDescription;
    
    public void InitWindow(int index, ItemDescription itemDescription)
    {
        _selectedItemIndex = index;
        _itemDescription = itemDescription;
        view.InitView(itemDescription);
    }

    private void ConfirmItem()
    {
        switch (_itemDescription.type)
        {
            case EItemType.Heal:
                gameCore.HealPlayer(_selectedItemIndex);
                break;
            case EItemType.Armor:
                gameCore.EquipArmor(_selectedItemIndex);
                break;
            case EItemType.Bullet:
                gameCore.AddItem(_itemDescription.id, true, true);
                break;
        }
        
        ClosePopup();
    }

    private void DeleteItem()
    {
        gameCore.DeleteItem(_selectedItemIndex, needUpdate: true, needSave: true);
        ClosePopup();
    }

    private void ClosePopup()
    {
        Disengage();
    }
    
    public override void Engage()
    {
        base.Engage();

        view.OnClickConfirmButton += ConfirmItem;
        view.OnClickDeleteButton += DeleteItem;
        view.OnClickCloseButton += ClosePopup;
    }

    public override void Disengage()
    {
        view.OnClickConfirmButton = null;
        view.OnClickDeleteButton = null;
        view.OnClickCloseButton = null;
        
        base.Disengage();
    }
}
