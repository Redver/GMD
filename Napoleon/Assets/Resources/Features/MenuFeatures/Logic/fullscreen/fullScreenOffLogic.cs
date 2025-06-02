using UnityEngine;

public class fullScreenOffLogic : Button
{
    public override void activateButton()
    {
        fullScreenOn();
    }

    public void fullScreenOn()
    {
        Screen.fullScreen = false;
    }
    
}
