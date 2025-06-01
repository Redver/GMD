using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class quitUiScript : ButtonUi
{
    private IButton quitLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quitLogic = new quitLogic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void activateButton()
    {
        quitLogic.activateButton();
    }
}
