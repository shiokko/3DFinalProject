using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Throw_bue : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject DivinationBlock;
    [SerializeField]
    private GameObject SpawnPoint;

    public Vector3 angularVelocityRange = new Vector3(2, 2, 2);
    //void Start()
    //{
    //    Throw(true);
    //}


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
        GameObject bueA = Instantiate(DivinationBlock, SpawnPoint.transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
        GameObject bueB = Instantiate(DivinationBlock, SpawnPoint.transform.position + new Vector3(0.5f, 0 ,0), Quaternion.identity);

        if(ans)
            bueA.GetComponent<DivinationBlockController>().face = 1;
        else
            bueA.GetComponent<DivinationBlockController>().face = 0;
        bueB.GetComponent<DivinationBlockController>().face = 0;

        AddRandomAngularVelocity(bueA);
        AddRandomAngularVelocity(bueB);
    }
}
