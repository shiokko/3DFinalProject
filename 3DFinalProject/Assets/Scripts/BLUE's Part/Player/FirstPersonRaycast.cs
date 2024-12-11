using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                if (hit.collider.gameObject.tag == "CanTakeItem")
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
                if (hit.collider.gameObject.tag == "CanTakeItem")
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
        switch (hit.name)
        {
            case "Charm(Clone)":
                ItemHolder.GetComponent<ItemController>().IncrementItemCount((int)Items.CHARM);
                break;
            case "Incense(Clone)":
                ItemHolder.GetComponent<ItemController>().IncrementItemCount((int)Items.INCENSE);
                break;
            case "DivinationBlock(Clone)":
                ItemHolder.GetComponent<ItemController>().IncrementItemCount((int)Items.DIVINATION_BLOCK);
                break;

            case "comb(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.COMB);
                break;
            case "fan(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.FAN);
                break;
            case "toy(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.TOY);
                break;
            case "jade(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.JADE);
                break;
            case "crutches(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.CRUTCH);
                break;
            case "gold(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.GOLD);
                break;
            case "hat(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.HAT);
                break;
            case "bowl(Clone)":
                Backpack.GetComponent<BackpackController>().IncrementRemnantCount((int)Remnants.BOWL);
                break;
        }

        Destroy(hit);
    }
}
