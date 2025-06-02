using UnityEngine;

public class resumeLogic : Button
{
    private GameObject optionsButton;
    public override void activateButton()
    {
        closeMenu();
    }

    public resumeLogic(GameObject optionsButton)
    {
        setOptionsButton(optionsButton);
    }

    public void closeMenu()
    {
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        optionsMenu.transform.position = new Vector3(53,-1,0);
        optionsButton.SetActive(true);
    }

    public void setOptionsButton(GameObject optionsButton)
    {
        this.optionsButton = optionsButton;
    }
}
