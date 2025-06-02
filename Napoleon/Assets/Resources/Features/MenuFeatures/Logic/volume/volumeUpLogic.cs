using UnityEngine;

public class volumeUpLogic : Button
{
    public override void activateButton()
    {
        volumeUp();
    }

    public void volumeUp()
    {
        float temp = AudioListener.volume;
        temp += 0.1f;
        float rounded = Mathf.Round(temp * 10f) / 10f;
        rounded = Mathf.Clamp(temp, 0f, 1f);
        AudioListener.volume = rounded;    
    }
}