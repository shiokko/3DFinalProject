using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    [SerializeField]
    private bool landed;
    [SerializeField]
    private float DropSpeed = 0.5f;
    [SerializeField]
    private float Height;
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
            transform.position = new Vector3(transform.position.x, transform.position.y - DropSpeed, transform.position.z);
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!landed && other.tag == "env") 
    //    {
    //        landed = true;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (!landed && collision.gameObject.tag == "env")
        {
            landed = true;
        }
    }
}
