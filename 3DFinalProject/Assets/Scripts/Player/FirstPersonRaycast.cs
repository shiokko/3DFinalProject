using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class FirstPersonRaycast : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [Header("Detection")]
    [SerializeField]
    private LayerMask LayerToDetect;
    [SerializeField]
    private float handLength = 3f;

    [Header("hit points")]
    [SerializeField]
    private GameObject RayIndicatorOn;
    [SerializeField]
    private GameObject RayIndicatorOff;

    private GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        indicator = null;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, handLength, LayerToDetect))
        {
            //if (hit.collider.gameObject.tag != "")
            //{
            //    RayIndicatorOn.SetActive(false);
            //    RayIndicatorOff.SetActive(true);
            //}
            //else
            //{
            //    RayIndicatorOn.SetActive(true);
            //    RayIndicatorOff.SetActive(false);
            //}

            if (indicator == null)
            {
                indicator = Instantiate(RayIndicatorOff);
            }

            indicator.transform.position = hit.point;
        }
        else
        {
            if(indicator != null)
            {
                Destroy(indicator);
                indicator = null;
            }
        }
    }
}
