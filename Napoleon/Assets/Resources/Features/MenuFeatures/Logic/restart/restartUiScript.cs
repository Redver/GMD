using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class restartUiScript : ButtonUi
{
    private IButton restartLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartLogic = new restartLogic();
    }
    
    public override void activateButton()
    {
        restartLogic.activateButton();
    }
}
