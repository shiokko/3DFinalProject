using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class Ask_God : MonoBehaviour
{
    // Start is called before the first frame update
    public Flowchart flowchart;
    private int renmant_id;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P)) {
            Flowchart.BroadcastFungusMessage("Ask");
        }
        if (Input.GetKey(KeyCode.O))
        {
            Flowchart.BroadcastFungusMessage("write");
        }
        if (Input.GetKey(KeyCode.G))
        {
            GameObject.Find("GameManager").GetComponent<renment>().finish();
        }
    }

    void AskQuestion() {
        int Q1 = flowchart.GetIntegerVariable("Q1");
        int Q2 = flowchart.GetIntegerVariable("Q2");
        if (Q1 == 1)
            FindDeadBody(Q2);
        else if (Q1 == 2)
            FindGhostTemple(Q2);
        else if (Q1 == 3)
            FindRenmant(Q2);
        else
            IsRenmantCorrect();
    }
    private int GetCardinalDirection(float angle)
    {
        if (angle >= 45 && angle < 135)
        {
            return 4; // 90° -> 北
        }
        else if (angle >= 135 && angle < 225)
        {
            return 2; // 180° -> 西
        }
        else if (angle >= 225 && angle < 315)
        {
            return 3; // 270° -> 南
        }
        else
        {
            return 1; // 0° 或 360° -> 东
        }
    }
    void positive() { 
    
    }
    void negative() { }

    void FindDeadBody(int dir) {
        GameObject deadbody = GameObject.Find("deadbody");
        Vector3 direction = deadbody.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        int cardinalDirection = GetCardinalDirection(angle);
        if (dir == cardinalDirection)
            positive();
        else
            negative();
    }
    void FindGhostTemple(int dir)
    {
        GameObject ghostTemple = GameObject.Find("GhostTemple");
        Vector3 direction = ghostTemple.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        int cardinalDirection = GetCardinalDirection(angle);
        if (dir == cardinalDirection)
            positive();
        else
            negative();
    }
    void FindRenmant(int dir)
    {
        GameObject[] renmant = GameObject.FindGameObjectsWithTag("renmant");
        foreach (GameObject R in renmant) {
            Vector3 direction = R.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            int cardinalDirection = GetCardinalDirection(angle);
            if (dir == cardinalDirection)
            {
                positive();
                return;
            }
        }
        negative();
    }
    void IsRenmantCorrect()
    {
        bool temp = GameObject.Find("GameManager").GetComponent<renment>().isCorrect(renmant_id);
        if (temp)
            positive();
        else 
            negative();
    }
    public void setRenmant_ID(int R) { 
        renmant_id = R;
    }
}