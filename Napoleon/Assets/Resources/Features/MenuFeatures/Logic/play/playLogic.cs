using UnityEngine;
using UnityEngine.SceneManagement;

public class playLogic : Button
{
    public override void activateButton()
    {
        startApplication();
    }

    public void startApplication()
    {
        SceneManager.LoadScene("MainScene");
    }
}
