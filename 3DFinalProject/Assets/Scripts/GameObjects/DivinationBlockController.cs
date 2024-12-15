using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivinationBlockController: MonoBehaviour
{
    // Start is called before the first frame update

    public int face;
    public Rigidbody rb;
    private bool dropped = false;

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Ground")) 
        if(!dropped)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;

            if (face == 1)
                currentRotation.z = 180;
            else
                currentRotation.z = 0;

            currentRotation.x = 0;
            transform.rotation = Quaternion.Euler(currentRotation);
            transform.position = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            dropped = true;
        }
    }
}
