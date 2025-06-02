using UnityEngine;

public class volumeUpLogic : Button
{
    public override void activateButton()
    {
        volumeUp();
    }

    public void volumeUp()
    {
        AudioListener.volume += 0.1f;
    }
}