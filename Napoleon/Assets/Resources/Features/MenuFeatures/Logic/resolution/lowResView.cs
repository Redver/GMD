using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class lowResView : ButtonUi
{
    private IButton lowResButton;
    void Awake()
    {
        lowResButton = new lowResLogic();
    }

    public override void activateButton()
    {
        lowResButton.activateButton();
    }
}
