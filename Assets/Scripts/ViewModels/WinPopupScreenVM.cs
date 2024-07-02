using UnityEngine;

public class WinPopupScreenVM : ViewModelBase<WinPopupScreenView>
{
    public WinPopupScreenVM(Canvas canvas, GameCore gameCore) : base(canvas, gameCore) { }

    private uint _itemId;
    
    public void InitWindow(ItemData itemData, ItemDescription itemDescription)
    {
        _itemId = itemDescription.id;

        view.InitView(itemData, itemDescription);
    }

    private void TakeReward()
    {
        gameCore.NewEnemy();
        gameCore.AddItem(_itemId, true, true);
        Disengage();
    }
    
    public override void Engage()
    {
        base.Engage();

        view.OnClickTakeRewardButton += TakeReward;
    }

    public override void Disengage()
    {
        view.OnClickTakeRewardButton = null;

        base.Disengage();
    }
}