using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pray : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject PrayArea;
    bool InArea;
    void Start()
    {
        PrayArea = this.gameObject;
        InArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (PrayArea != this.gameObject)
            {
                PrayArea.GetComponent<Prayed>().PrayWoodSword();
            }
            if (InArea)
                GameObject.Find("GameManager").GetComponent<renment>().finish();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "prayarea")
            PrayArea = other.gameObject;
        if (other.transform.tag == "finish")
        {
            Debug.Log("in area");
            InArea = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "prayarea")
            PrayArea = this.gameObject;
        if (other.transform.tag == "finish")
        {
            Debug.Log("out area");
            InArea = false;
        }
    }

}
