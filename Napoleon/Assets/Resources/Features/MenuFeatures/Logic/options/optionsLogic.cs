using UnityEngine;

public class optionsLogic : Button
{
    private GameObject optionsMenu;

    public optionsLogic(GameObject optionsMenu)
    {
        this.optionsMenu = optionsMenu;
    }

    public override void activateButton()
    {
        openOptionsMenu();
    }

    public void openOptionsMenu()
    {
        optionsMenu.transform.position = new Vector3(29.5f,-1.29f,0f);
    }
}
