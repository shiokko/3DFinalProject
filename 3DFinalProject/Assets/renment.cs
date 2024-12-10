using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renment : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] renmant_IDs = new int[3];
    void Start()
    {
        renmant_IDs[0] = Random.Range(11, 13);
        renmant_IDs[1] = Random.Range(21, 24);
        renmant_IDs[2] = Random.Range(31, 34);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
