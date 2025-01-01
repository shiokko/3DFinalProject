using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _bonusPointText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.Win)
        {
            _titleText.text = "CONGRATS! YOU JUST PURIFIED THE GHOST";
            _bonusPointText.enabled = true;
            _bonusPointText.text = "The Bonus Score is: " + GameManager.BonusScore;
        }
        else
        {
            _titleText.text = "GGWP, TRY AGAIN!";
            _bonusPointText.enabled = false;
        }
    }

    public void HomeScreen()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        // will not work in unity editor tho
        Application.Quit();
    }
}
