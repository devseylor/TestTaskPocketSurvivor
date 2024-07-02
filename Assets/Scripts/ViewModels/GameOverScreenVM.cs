using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenVM : ViewModelBase<GameOverScreenView>
{
    public GameOverScreenVM(Canvas canvas, GameCore gameCore) : base(canvas, gameCore) { }

    private void ReloadGame()
    {
        gameCore.ReloadGame();
        Disengage();
    }
    
    public override void Engage()
    {
        base.Engage();

        view.OnClickReloadButton += ReloadGame;
    }

    public override void Disengage()
    {
        view.OnClickReloadButton = null;
        
        base.Disengage();
    }
}
