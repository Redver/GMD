using UnityEngine;

public class midResLogic : Button
{
    public override void activateButton()
    {
        setResolution();
    }

    public void setResolution()
    {
        Screen.SetResolution(1280,720,true);
    }
}
