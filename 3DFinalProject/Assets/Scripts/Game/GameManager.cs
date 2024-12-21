using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Flowchart _flowchart;

    [SerializeField]
    private int[] remnantsInSex;
    [SerializeField]
    private int[] remnantsInAge;
    [SerializeField]
    private int[] remnantsInHierarchy;

    private bool gameOver;

    private int[] correctRemnantID = new int[3];
    private int correctGhostID;

    private int bonusScore;
    // Start is called before the first frame update
    void Start()
    {
        bonusScore = 0;
        gameOver = false;

        CreateCorrectAns();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            // terminate current game, preparing for the next one
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


    // public functios here

    // for player controller to call after using the wood sword
    public void EndGame(int purifiedGhostID)
    {
        bonusScore = 0;
        if (purifiedGhostID == correctGhostID)
        {
            Flowchart.BroadcastFungusMessage("win");
        }
        else
        {
            Flowchart.BroadcastFungusMessage("lose");
        }

        if (correctRemnantID[0] == _flowchart.GetIntegerVariable("sex"))
        {
            Debug.Log("sex");
            bonusScore++;
        }

        if (correctRemnantID[1] == _flowchart.GetIntegerVariable("age"))
        {
            Debug.Log("age");
            bonusScore++;
        }

        if (correctRemnantID[2] == _flowchart.GetIntegerVariable("state"))
        {
            Debug.Log("state");
            bonusScore++;
        }

        Debug.Log("Bonus Score : " + bonusScore);
        gameOver = true;
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