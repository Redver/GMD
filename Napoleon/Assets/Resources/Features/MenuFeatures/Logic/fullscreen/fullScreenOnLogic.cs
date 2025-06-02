using UnityEngine;

public class fullScreenOnLogic : Button
{
    public override void activateButton()
    {
        fullScreenOn();
    }

    public void fullScreenOn()
    {
        Screen.fullScreen = true;
    }
    
}
