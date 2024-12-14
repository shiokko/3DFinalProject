using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Throw_bue : MonoBehaviour
{
    public GameObject bue;
    public GameObject SP_point;

    public Vector3 angularVelocityRange = new Vector3(2, 2, 2);
    void Start()
    {
        Throw(true);
    }


    void AddRandomAngularVelocity(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(
            Random.Range(-angularVelocityRange.x, angularVelocityRange.x) * 4,
            Random.Range(-angularVelocityRange.y, angularVelocityRange.y) * 10,
            Random.Range(-angularVelocityRange.z, angularVelocityRange.z) * 4
        );
    }

    public void Throw(bool ans)
    {
        GameObject bueA = Instantiate(bue, SP_point.transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
        GameObject bueB = Instantiate(bue, SP_point.transform.position + new Vector3(0.5f, 0 ,0), Quaternion.identity);

        if(ans)
            bueA.GetComponent<bue_controll>().face = 1;
        else
            bueA.GetComponent<bue_controll>().face = 0;
        bueB.GetComponent<bue_controll>().face = 0;

        AddRandomAngularVelocity(bueA);
        AddRandomAngularVelocity(bueB);
    }
}
