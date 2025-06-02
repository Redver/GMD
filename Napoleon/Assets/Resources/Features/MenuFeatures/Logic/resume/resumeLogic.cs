using UnityEngine;

public class resumeLogic : Button
{
    [SerializeField] private GameObject optionsMenu;

    public override void activateButton()
    {
        closeMenu();
    }

    public void closeMenu()
    {
        optionsMenu.SetActive(false);
    }
}
