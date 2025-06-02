using Resources.Features.MenuFeatures.Logic;
using UnityEngine;

public class fullScreenOffView : ButtonUi
{
private IButton _fullScreenOffLogic;
void Start()
{
    _fullScreenOffLogic = new fullScreenOffLogic();
}
public override void activateButton()
{
    _fullScreenOffLogic.activateButton();
}
}