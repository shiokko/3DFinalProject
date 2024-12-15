using System.Collections;
using System.Collections.Generic;
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

    [Header("References")]
    [SerializeField]
    private GameObject ItemHolder;
    [SerializeField]
    private GameObject Backpack;

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
            // hit something
            if (indicator == null)
            {
                if (hit.collider.gameObject.tag == "Remnant" || hit.collider.gameObject.tag == "Item")
                {
                    indicator = Instantiate(RayIndicatorOn);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        TakeItem(hit.collider.gameObject);
                    }
                }
                else
                {
                    indicator = Instantiate(RayIndicatorOff);
                }
            }
            else
            {
                Destroy(indicator);
                if (hit.collider.gameObject.tag == "Remnant" || hit.collider.gameObject.tag == "Item")
                {
                    indicator = Instantiate(RayIndicatorOn);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        TakeItem(hit.collider.gameObject);
                    }
                }
                else
                {
                    indicator = Instantiate(RayIndicatorOff);
                }
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

    private void TakeItem(GameObject hit)
    {
        if(hit.tag == "Item")
        {
            ItemHolder.GetComponent<ItemController>().IncrementItemCount(hit.GetComponent<ItemID>().i_ID);
        }
        else if (hit.tag == "Remnant")
        {
            Backpack.GetComponent<BackpackController>().IncrementRemnantCount(hit.GetComponent<RemnantID>().r_ID);
        }
        
        Destroy(hit);
    }
}
