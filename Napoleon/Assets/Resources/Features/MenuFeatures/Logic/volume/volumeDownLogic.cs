using UnityEngine;

public class volumeDownLogic : Button
{
    public override void activateButton()
    {
        volumeDown();
    }

    public void volumeDown()
    {
        AudioListener.volume -= 0.1f;
    }
}
