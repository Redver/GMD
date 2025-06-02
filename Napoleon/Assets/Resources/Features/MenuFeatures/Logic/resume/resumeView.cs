using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class resumeView : ButtonUi
{
    private IButton resumeButton;
    [SerializeField] private GameObject optionsButton;
    void Awake()
    {
        resumeButton = new resumeLogic(optionsButton);
    }

    public override void activateButton()
    {
        resumeButton.activateButton();
    }
}
