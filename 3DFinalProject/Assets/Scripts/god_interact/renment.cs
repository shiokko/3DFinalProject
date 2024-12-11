using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class renment : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] renmant_IDs = new int[3];
    private int woodswordgod;
    private int ghost_ID;
    public Flowchart flowchart;
    void Start()
    {
        renmant_IDs[0] = Random.Range(11, 13);
        Debug.Log("renmant_ID : " + renmant_IDs[0]);
        renmant_IDs[1] = Random.Range(21, 24);
        Debug.Log("renmant_ID : " + renmant_IDs[1]);
        renmant_IDs[2] = Random.Range(31, 34);
        Debug.Log("renmant_ID : " + renmant_IDs[2]);
        ghost_ID = Random.Range(1, 3);
        Debug.Log("ghost ID:" + ghost_ID);
        woodswordgod = 0;
    }

    public void blessing(int ID) 
    {
        woodswordgod = ID;
        Flowchart.BroadcastFungusMessage("blessing");
    }
    public bool isCorrect(int ID) 
    {
        foreach (int i in renmant_IDs) 
        {
            if (i == ID) 
                return true;
        }
        return false;
    }

    public void finish() {
        if (woodswordgod == ghost_ID)
        {
            Flowchart.BroadcastFungusMessage("win");
        }
        else 
        { 
            Flowchart.BroadcastFungusMessage("lose");
        }
        int score = 0;
        if (renmant_IDs[0] == flowchart.GetIntegerVariable("sex"))
            score++;
        if (renmant_IDs[1] == flowchart.GetIntegerVariable("age"))
            score++;
        if (renmant_IDs[1] == flowchart.GetIntegerVariable("state"))
            score++;
        Debug.Log("score : " + score);
        //flowchart.SetIntegerVariable("score", score);
        //Flowchart.BroadcastFungusMessage("score");
    }
}
