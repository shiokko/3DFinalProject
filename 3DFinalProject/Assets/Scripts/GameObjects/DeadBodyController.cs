using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyController : MonoBehaviour
{
    private bool firstTimeTouch;

    // Start is called before the first frame update
    void Start()
    {
        firstTimeTouch = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (firstTimeTouch)
        {
            if(collision.gameObject.tag == "Player")
            {
                GameObject.Find("Ghost").GetComponent<GhostController>().BeAngry();

                firstTimeTouch = false;
            }
        }

        Debug.Log(collision.gameObject.tag);
    }
}
