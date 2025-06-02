using UnityEngine;

public class lowResLogic : Button
{
    public override void activateButton()
    {
        setResolution();
    }

    public void setResolution()
    {
        Screen.SetResolution(800,600,true);
    }
}
