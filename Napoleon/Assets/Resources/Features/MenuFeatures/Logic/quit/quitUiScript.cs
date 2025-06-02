using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class quitUiScript : ButtonUi
{
    private IButton quitLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        quitLogic = new quitLogic();
    }

    public override void activateButton()
    {
        quitLogic.activateButton();
    }
}
