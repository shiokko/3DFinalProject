using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("Init inventory")]
    [SerializeField]
    private int InitCharmNum = 0;
    [SerializeField]
    private int InitDivinationBlockNum = 0;
    [SerializeField]
    private int initIncenseNum = 0;

    [SerializeField]
    private int curItemIndex = 0;  // default to not taking anything

    private int[] itemCount = new int[5];  // get currently how many items there are

    // Start is called before the first frame update
    void Start()
    {
        curItemIndex = 0;

        itemCount[(int)Items.CHARM] = InitCharmNum;
        itemCount[(int)Items.DIVINATION_BLOCK] = InitDivinationBlockNum;
        itemCount[(int)Items.INCENSE] = initIncenseNum;
        itemCount[(int)Items.WOOD_SWORD] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        int prevItemIndex = curItemIndex;

        GetInputs();

        if (prevItemIndex != curItemIndex)
        {
            SelectItem();
        }
    }

    private void GetInputs()
    {
        // for item switching
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (curItemIndex >= transform.childCount - 1)
            {
                curItemIndex = transform.childCount - 1;
            }
            else
            {
                curItemIndex++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (curItemIndex <= 0)
            {
                curItemIndex = 0;
            }
            else
            {
                curItemIndex--;
            }
        }

        // for item usage
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseItem();
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

            if (i == curItemIndex)
            {
                if (itemCount[i] > 0)  // skip lantern's index
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

    private void UseItem()
    {
        if(curItemIndex == 0)  // only holding lantern, nothing can be used
        {
            return;
        }

        bool successfullyUsed = false;

        if(curItemIndex == (int)Items.CHARM)
        {
            // make player invincible
            GameObject.Find("PlayerCapsule").GetComponent<PlayerController>().SetInvincible();
            successfullyUsed = true;
        }
        else if(curItemIndex == (int)Items.DIVINATION_BLOCK)
        {
            // call function in ask god
        }
        else if(curItemIndex == (int)Items.INCENSE)
        {
            // try to start praying at pray area
            bool startPraying = GameObject.Find("PlayerCapsule").GetComponent<PlayerController>().SetPraying();

            if (startPraying)
            {
                successfullyUsed = true;
            }
        }
        else if (curItemIndex == (int)Items.WOOD_SWORD)
        {
            // call function in kill ghost
        }

        if (successfullyUsed)
        {
            itemCount[curItemIndex] --;
            curItemIndex = 0;
        }
    }


    // Public function here

    // for First Person Raycast
    public void IncrementItemCount(int index)
    {
        if(index <= 0 || index >= itemCount.Length)
        {
            Debug.Log("index out of range in IncrementItemCount, passed wrong params");
            return;
        }

        itemCount[index] ++;
    }

    // for After 請神 get the sword
    public void GetWoodSword()
    {
        itemCount[(int)Items.WOOD_SWORD] = 1;
    }
}
