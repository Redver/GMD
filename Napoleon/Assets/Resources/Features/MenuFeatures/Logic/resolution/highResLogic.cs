using UnityEngine;

public class highResLogic : Button
{
    public override void activateButton()
    {
        setResolution();
    }

    public void setResolution()
    {
        Screen.SetResolution(2560,1440,true);
    }
}
