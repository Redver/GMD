using System;
using Resources.Features.MenuFeatures.Logic;


public class playUiScript : ButtonUi
{
    private IButton playLogic;

    private void Start()
    {
        playLogic = new playLogic();
    }

    public override void activateButton()
    {
        playLogic.activateButton();
    }
}
