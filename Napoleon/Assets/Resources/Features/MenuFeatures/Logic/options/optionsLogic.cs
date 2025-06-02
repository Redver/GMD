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
        optionsMenu.SetActive(true);
    }
}
