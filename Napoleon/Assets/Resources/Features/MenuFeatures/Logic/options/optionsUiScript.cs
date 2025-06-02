using Resources.Features.MenuFeatures.Logic;

public class optionsUiScript : ButtonUi
{
    private IButton optionsLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionsLogic = new optionsLogic();
    }

    public override void activateButton()
    {
        optionsLogic.activateButton();
    }
}
