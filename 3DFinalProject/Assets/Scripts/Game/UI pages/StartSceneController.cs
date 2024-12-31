using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject HelpScreen;

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        // will not work in unity editor tho
        Application.Quit();
    }

    public void GameHelp()
    {
        MainMenu.SetActive(false);
        HelpScreen.SetActive(true);
    }

    public void CloseHelp()
    {
        MainMenu.SetActive(true);
        HelpScreen.SetActive(false);
    }
}
