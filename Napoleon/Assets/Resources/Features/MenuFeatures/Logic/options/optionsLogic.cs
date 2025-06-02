using UnityEngine;

public class optionsLogic : Button
{
    public override void activateButton()
    {
        openOptionsMenu();
    }

    public void openOptionsMenu()
    {
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        optionsMenu.transform.position = Vector3.zero;
    }
}
