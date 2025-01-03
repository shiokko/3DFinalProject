using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Flowchart _flowchart;
    [SerializeField]
    private GameObject[] GhostTemples;
    [SerializeField]
    private GameObject AbortUI;
    [SerializeField]
    private GameObject HelpUI;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private List<GhostController> ghost;

    [Header("Parameters")]
    [SerializeField]
    private int[] remnantsInSex;
    [SerializeField]
    private int[] remnantsInAge;
    [SerializeField]
    private int[] remnantsInHierarchy;

    
    
    private bool gameOver;

    private int[] correctRemnantID = new int[(int)GlobalVar.NUM_REMNANT_CATEGORY];
    private int correctGhostID;

    private int correctGhostTempleIndex;


    // All Public Static variables here for scene passing
    public static int BonusScore;
    public static bool Win;
    public static bool[] CorrectAns = new bool[(int)GlobalVar.NUM_REMNANT_CATEGORY];

    private void Awake()
    {
        BonusScore = 0;
        gameOver = false;
        Win = false;
        for (int i = 0; i < CorrectAns.Length; i++)
        {
            CorrectAns[i] = false;
        }

        AbortUI.SetActive(false);

        CreateCorrectAns();
        DecideCorrectGhostTemple();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (correctRemnantID[0] == 0) // female
        {
            ActivateGhost(0); 
        }
        else 
        {
            ActivateGhost(1); // male
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // player wants to leave mid game
            if (AbortUI.activeSelf)
            {
                AbortUI.SetActive(false);
                Player.GetComponent<PlayerController>().SetCanMove();
            }
            else
            {
                AbortUI.SetActive(true);
                Player.GetComponent<PlayerController>().ResetCanMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            // player want to see Help Manual
            //HelpUI.SetActive(!HelpUI.activeSelf);
            if (HelpUI.activeSelf)
            {
                HelpUI.SetActive(false);
                Player.GetComponent<PlayerController>().SetCanMove();
            }
            else
            {
                HelpUI.SetActive(true);
                Player.GetComponent<PlayerController>().ResetCanMove();
            }
        }
    }
    private void ActivateGhost(int index)
    {
        for (int i = 0; i < ghost.Count; i++)
        {
            ghost[i].gameObject.SetActive(i == index); // for correct ghost sex
        }
    }
    private void LetGhostSleep()
    {
        for (int i = 0; i < ghost.Count; i++)
        {
            ghost[i].gameObject.SetActive(false);
        }
    }

    private void CreateCorrectAns()
    {
        // for correct remnants
        correctRemnantID[0] = remnantsInSex[(int)Random.Range(0, remnantsInSex.Length)];
        correctRemnantID[1] = remnantsInAge[(int)Random.Range(0, remnantsInAge.Length)];
        correctRemnantID[2] = remnantsInHierarchy[(int)Random.Range(0, remnantsInHierarchy.Length)];

        // for correct ghost
        correctGhostID = (int)Random.Range(1, (int)GlobalVar.NUM_GHOST_TYPE + 1);

        // for demo usage
        Debug.Log("Correct ghost ID: " + correctGhostID);
        Debug.Log("Correct Remnant Id: " + correctRemnantID[0] + ", " + correctRemnantID[1] + ", " + correctRemnantID[2]);
    }

    private void DecideCorrectGhostTemple()
    {
        correctGhostTempleIndex = Random.Range(0, GhostTemples.Length);

        for(int i = 0; i < GhostTemples.Length; i++)
        {
            if(i != correctGhostTempleIndex)
            {
                // disable this temple
                GhostTemples[i].SetActive(false);
            }
        }
    }


    // public functios here

    // for everyone
    public bool GameOver()
    {
        LetGhostSleep();
        return gameOver;
    }

    // for obj distributer to distribute the correct remnants
    public int GetCorrectGhostTempleIndex()
    {
        return correctGhostTempleIndex;
    }

    public int[] GetCorrectRemnants()
    {
        return correctRemnantID;        
    }

    // for obj distributer to distribute the correct deadbody
    public int GetCorrectDeadbody()
    {
        return correctGhostID;
    }

    // for player controller to call after using the wood sword
    public void EndGame(int purifiedGhostID)
    {
        BonusScore = 0;
        if (purifiedGhostID == correctGhostID)
        {
            Win = true;
            Flowchart.BroadcastFungusMessage("win");
        }
        else
        {
            Win = false;
            Flowchart.BroadcastFungusMessage("lose");
        }

        if (correctRemnantID[0] == _flowchart.GetIntegerVariable("sex"))
        {
            Debug.Log("sex");
            CorrectAns[0] = true;
            BonusScore++;
        }

        if (correctRemnantID[1] == _flowchart.GetIntegerVariable("age"))
        {
            Debug.Log("age");
            CorrectAns[1] = true;
            BonusScore++;
        }

        if (correctRemnantID[2] == _flowchart.GetIntegerVariable("state"))
        {
            Debug.Log("state");
            CorrectAns[2] = true;
            BonusScore++;
        }

        Debug.Log("Bonus Score : " + BonusScore);
        gameOver = true;
    }

    // for scene switching
    public void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void ResetAbortScreen()
    {
        AbortUI.SetActive(false);
        Player.GetComponent<PlayerController>().SetCanMove();
    }

    // for player asking god
    public bool IsCorrect(int remnantID)
    {
        foreach (int i in correctRemnantID)
        {
            if (i == remnantID)
            {
                return true;
            }
        }
        return false;
    }
}
