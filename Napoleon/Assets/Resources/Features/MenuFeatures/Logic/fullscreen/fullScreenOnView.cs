using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class fullScreenOnView : ButtonUi
{
    private IButton _fullScreenOnLogic;
    
    void Awake()
    {
        _fullScreenOnLogic = new fullScreenOnLogic();
    }

    public override void activateButton()
    {
        _fullScreenOnLogic.activateButton();
    }
}
