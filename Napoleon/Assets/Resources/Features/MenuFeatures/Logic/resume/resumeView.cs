using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class resumeView : ButtonUi
{
    private IButton resumeButton;
    void Awake()
    {
        resumeButton = new resumeLogic();
    }

    public override void activateButton()
    {
        resumeButton.activateButton();
    }
}
