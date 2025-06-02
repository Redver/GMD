using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class optionsUiScript : ButtonUi
{
    private IButton optionsLogic;
    [SerializeField] private GameObject optionsPanel;
    void Awake()
    {
        optionsLogic = new optionsLogic(optionsPanel);
    }

    public override void activateButton()
    {
        optionsLogic.activateButton();
    }
}
