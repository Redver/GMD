using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class midResView : ButtonUi
{
    private IButton midResButton;
    void Awake()
    {
        midResButton = new midResLogic();
    }

    public override void activateButton()
    {
        midResButton.activateButton();
    }
}
