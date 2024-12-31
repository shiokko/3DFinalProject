using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class FungusTrigger : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Flowchart _flowchart;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Backpack;

    private int remnantID;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // show cursor and disable playe movement
            Player.GetComponent<PlayerController>().ResetCanMove();

            Flowchart.BroadcastFungusMessage("write");
        }
    }

    private void AskQuestion() {
        int Q1 = _flowchart.GetIntegerVariable("Q1");
        int Q2 = _flowchart.GetIntegerVariable("Q2");

        if (Q1 == 1)
            FindDeadBody(Q2);
        else if (Q1 == 2)
            FindGhostTemple(Q2);
        else if (Q1 == 3)
            FindRenmant(Q2);
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

    private void positive() {
        Player.GetComponent<PlayerController>().Throw(true);
        Debug.Log("positive");
    }

    private void negative() {
        Player.GetComponent<PlayerController>().Throw(false);
        Debug.Log("negative");
    }

    private void FindDeadBody(int dir) {
        GameObject deadbody = GameObject.FindGameObjectWithTag("Deadbody");
        GameObject temple = GameObject.Find("Temple");
        Vector3 direction = deadbody.transform.position - temple.transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        int cardinalDirection = GetCardinalDirection(angle);
        if (dir == cardinalDirection)
            positive();
        else
            negative();
    }

    private void FindGhostTemple(int dir)
    {
        GameObject ghostTemple = GameObject.FindGameObjectWithTag("GhostTemple");
        GameObject temple = GameObject.Find("Temple");
        Vector3 direction = ghostTemple.transform.position - temple.transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        int cardinalDirection = GetCardinalDirection(angle);
        if (dir == cardinalDirection)
            positive();
        else
            negative();


    }

    private void FindRenmant(int dir)
    {
        GameObject[] renmant = GameObject.FindGameObjectsWithTag("Remnant");
        GameObject temple = GameObject.Find("Temple");
        foreach (GameObject R in renmant) {
            Vector3 direction = R.transform.position - temple.transform.position;
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

    private void RemnantCorrectEntry()
    {
        // before telling whether the remnant is correct, force user to choose one remnant to ask
        Backpack.GetComponent<BackpackController>().SetBackpackMode();
        Backpack.GetComponent<BackpackController>().SetIsChoosing();
    }

    private void IsRenmantCorrect()
    {
        remnantID = Backpack.GetComponent<BackpackController>().GetSelectedRemnantID();

        bool isCorrect = GameObject.Find("GameManager").GetComponent<GameManager>().IsCorrect(remnantID);
        if (isCorrect)
            positive();
        else 
            negative();
    }


    // public function here

    // for broacasting
    public void BroadCastAsk()
    {
        Flowchart.BroadcastFungusMessage("Ask");
    }

    public void BroadCastRemnantSelected()
    {
        Flowchart.BroadcastFungusMessage("AskCorrectRemnant");
    }

    public void BroadCastPunishment()
    {
        Flowchart.BroadcastFungusMessage("Punishment");
    }

    // for controlling inputs, lock the mouse, enable screen rotation
    public void FungusModeOver()
    {
        // hide mouse and enable screen rotation
        Player.GetComponent<PlayerController>().SetCanMove();
    }

    public void SetRenmant_ID(int R) { 
        remnantID = R;
    }
}