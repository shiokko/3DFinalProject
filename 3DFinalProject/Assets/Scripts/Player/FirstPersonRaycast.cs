using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonRaycast : MonoBehaviour
{
    // declare some const for value passing
    private const int CHARM = 0;
    private const int DIVINATION_BLOCK = 1;
    private const int INCENSE = 2;

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
                if (hit.collider.gameObject.tag == "CanTakeItem")
                {
                    indicator = Instantiate(RayIndicatorOn);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
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
                if (hit.collider.gameObject.tag == "CanTakeItem")
                {
                    indicator = Instantiate(RayIndicatorOn);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
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
        if (hit.name == "Charm(Clone)") 
        {
            // pass the result index of getting charm to item controller
            ItemHolder.GetComponent<ItemController>().IncrementItemCount(CHARM);
        }
        else if (hit.name == "Incense(Clone)")
        {
            // pass the result index of getting incense to item controller
            ItemHolder.GetComponent<ItemController>().IncrementItemCount(INCENSE);
        }
        else if (hit.name == "DivinationBlock(Clone)")
        {
            // pass the result index of getting divination block to item controller
            ItemHolder.GetComponent<ItemController>().IncrementItemCount(DIVINATION_BLOCK);
        }

        Destroy(hit);
    }
}
