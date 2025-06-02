using UnityEngine;

public class volumeDownLogic : Button
{
    public override void activateButton()
    {
        volumeDown();
    }

    public void volumeDown()
    {
        float temp = AudioListener.volume;
        temp -= 0.1f;
        float rounded = Mathf.Round(temp * 10f) / 10f;
        rounded = Mathf.Clamp(temp, 0f, 1f);
        AudioListener.volume = rounded;
    }
}
