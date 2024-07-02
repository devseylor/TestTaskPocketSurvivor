using System.Collections.Generic;
using UnityEngine;

public class MainScreenVM : ViewModelBase<MainScreenView>
{
    public MainScreenVM(Canvas canvas, GameCore gameCore) : base(canvas, gameCore) { }

    public void InitWindow(Character[] characters, List<ItemDescription> items)
    {
        view.InitView(characters, items);
    }

    private void ShowItemPopup(int index,ItemDescription itemDescription)
    {
        gameCore.OpenPopupScreen(index, itemDescription);
    }

    private void RemoveItems(int startIndex, int endIndex)
    {
        gameCore.TryRemoveItems(startIndex, endIndex);
    }
    
    private void SelectedGun(ECaliberType caliberType)
    {
        gameCore.TrySetSelectedGun(caliberType);
    }
    
    private void Fire()
    {
        gameCore.TryFire();
    }
    
    public override void Engage()
    {
        base.Engage();
        
        view.OnClickItemButton += ShowItemPopup;
        view.OnRemoveItems += RemoveItems;
        view.OnClickSelectedGunButton += SelectedGun;
        view.OnClickFireButton += Fire;
    }

    public override void Disengage()
    {
        view.OnClickItemButton = null;
        view.OnRemoveItems = null;
        view.OnClickSelectedGunButton = null;
        view.OnClickFireButton = null;
        
        base.Disengage();
    }
}
