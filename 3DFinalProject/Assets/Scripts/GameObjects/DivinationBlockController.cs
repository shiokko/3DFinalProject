using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivinationBlockController: MonoBehaviour
{
    // Start is called before the first frame update

    public int face;
    public Rigidbody rb;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;

            if (face == 1)
                currentRotation.z = 180;
            else
                currentRotation.z = 0;

            currentRotation.x = 0;
            transform.rotation = Quaternion.Euler(currentRotation);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
