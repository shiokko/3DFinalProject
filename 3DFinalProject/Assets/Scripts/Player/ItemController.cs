using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // declare some const for value passing
    private const int CHARM = 0;
    private const int DIVINATION_BLOCK = 1;
    private const int INCENSE = 2;

    [Header("Init inventory")]
    [SerializeField]
    private int InitCharmNum = 0;
    [SerializeField]
    private int InitDivinationBlockNum = 0;
    [SerializeField]
    private int initIncenseNum = 0;

    [SerializeField]
    private int curItem = 0;  // default to not taking anything

    private int[] itemCount = new int[3];  // get currently how many items there are

    // Start is called before the first frame update
    void Start()
    {
        curItem = 0;

        itemCount[CHARM] = InitCharmNum;
        itemCount[DIVINATION_BLOCK] = InitDivinationBlockNum;
        itemCount[INCENSE] = initIncenseNum;
    }

    // Update is called once per frame
    void Update()
    {
        int prevItem = curItem;

        GetInputs();

        if (prevItem != curItem)
        {
            SelectItem();
        }
    }

    private void GetInputs()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (curItem >= transform.childCount - 1)
            {
                curItem = transform.childCount - 1;
            }
            else
            {
                curItem++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (curItem <= 0)
            {
                curItem = 0;
            }
            else
            {
                curItem--;
            }
        }
    }

    private void SelectItem()
    {
        int i = 0;
        foreach (Transform item in transform)
        {
            if (i == 0)
            {
                // not holding anything except lantern
                i++;
                continue;
            }

            if (i == curItem)
            {
                if (itemCount[i - 1] > 0)  // skip lantern's index
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }

            i++;
        }
    }


    // Public function here

    // for First Person Raycast
    public void IncrementItemCount(int index)
    {
        if(index < 0 || index >= itemCount.Length)
        {
            Debug.Log("index out of range in IncrementItemCount");
            return;
        }

        itemCount[index] ++;
    }
}
