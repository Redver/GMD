using UnityEngine;

public class gameResumeLogic : Button
{
    public override void activateButton()
    {
        closeMenu();
    }

    public void closeMenu()
    {
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        optionsMenu.transform.position = new Vector3(16,0 ,0);
    }
}
