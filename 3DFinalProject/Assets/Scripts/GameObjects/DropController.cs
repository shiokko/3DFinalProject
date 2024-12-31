using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    [SerializeField]
    private bool landed;
    // Start is called before the first frame update
    void Start()
    {
        landed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!landed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!landed && other.tag == "env") 
        {
            landed = true;
        }
    }
}
