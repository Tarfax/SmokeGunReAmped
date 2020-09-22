using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ButtonFunctions : MonoBehaviour {

    public void MainMenu_StartGame() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu_ExitGame() {
        Application.Quit();
    }

    public void InLevel_TryAgain() {
        SceneManager.LoadScene(1);
    }

    public void InLevel_MainMenu() {
        SceneManager.LoadScene(0);
    }

}
