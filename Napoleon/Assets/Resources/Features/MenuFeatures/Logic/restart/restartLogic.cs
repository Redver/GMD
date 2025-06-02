using UnityEngine.SceneManagement;

public class restartLogic : Button
{
    public override void activateButton()
    {
        restartGame();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
