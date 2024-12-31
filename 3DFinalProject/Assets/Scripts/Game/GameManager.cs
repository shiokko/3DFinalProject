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

        CreateCorrectAns();
        DecideCorrectGhostTemple();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
