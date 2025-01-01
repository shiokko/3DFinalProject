using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControll : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Reference")]
    [SerializeField]
    private GameObject StartPosition;

    [Header("Parameters")]
    [SerializeField]
    public float RemainTime;
    public float Range = 0.01f;

    public void Init() 
    { 
        this.transform.position = StartPosition.transform.position;
    }

    public void Ignite(float time)
    {
        this.transform.position = StartPosition.transform.position + new Vector3(0, time * Range, 0);
    }
}
