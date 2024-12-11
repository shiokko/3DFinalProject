using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renmant_ID : MonoBehaviour
{
    public int ID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "renmant_area")
           GameObject.Find("player").GetComponent<Ask_God>().setRenmant_ID(ID);
    }
}
