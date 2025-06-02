using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class gameResumeView : ButtonUi
{
    private IButton resumeButton;
    void Awake()
    {
        resumeButton = new gameResumeLogic();
    }

    public override void activateButton()
    {
        resumeButton.activateButton();
    }
}
