using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class highResView : ButtonUi
{
    private IButton highResButton;
    void Awake()
    {
        highResButton = new highResLogic();
    }

    public override void activateButton()
    {
        highResButton.activateButton();
    }
}

