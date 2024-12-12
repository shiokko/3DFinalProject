using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Refernce")]
    [SerializeField]
    private GameObject Backpack;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private TextMeshProUGUI CurrentMessage;
    

    private int curItemIndex = 0;  // default to not taking anything

    private int[] itemCount = new int[(int)GlobalVar.NUM_ITEM_TYPE];  // get currently how many items there are
    private string[] itemNames = new string[(int)GlobalVar.NUM_ITEM_TYPE];  // just a temp UI, will use other implementation in the future

    // Start is called before the first frame update
    void Start()
    {
        curItemIndex = 0;
        CurrentMessage.enabled = true;
        CurrentMessage.text = "Current Taking: Nothing";

        itemCount[(int)Items.CHARM] = InitCharmNum;
        itemCount[(int)Items.DIVINATION_BLOCK] = InitDivinationBlockNum;
        itemCount[(int)Items.INCENSE] = initIncenseNum;
        itemCount[(int)Items.WOOD_SWORD] = 1;

        itemNames[0] = "Nothing";
        itemNames[(int)Items.CHARM] = "Charm";
        itemNames[(int)Items.DIVINATION_BLOCK] = "Divination Block";
        itemNames[(int)Items.INCENSE] = "Incense";
        itemNames[(int)Items.WOOD_SWORD] = "Wood Sword";
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
        // check if it's backpack mode
        if (Backpack.GetComponent<BackpackController>().IsBackpackMode())
        {
            CurrentMessage.enabled = false;
            return;
        }
        CurrentMessage.enabled = true;

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
            if(i == 0)
            {
                CurrentMessage.text = "Current Taking: Nothing";
                i++;
                continue;
            }

            if (i == curItemIndex)
            {
                if (itemCount[i] > 0)
                {
                    item.gameObject.SetActive(true);
                    CurrentMessage.text = "Current Taking: " + itemNames[i] + ", Remaining:" + itemCount[i];
                }
                else
                {
                    item.gameObject.SetActive(false);
                    CurrentMessage.text = "Want to Take: " + itemNames[i];
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
        if(curItemIndex == 0 || itemCount[curItemIndex] == 0)  // only holding lantern, nothing can be used
        {
            return;
        }

        bool successfullyUsed = false;

        if(curItemIndex == (int)Items.CHARM)
        {
            // make player invincible
            Player.GetComponent<PlayerController>().SetInvincible();
            successfullyUsed = true;
        }
        else if(curItemIndex == (int)Items.DIVINATION_BLOCK)
        {
            // call function in ask god
        }
        else if(curItemIndex == (int)Items.INCENSE)
        {
            // try to start praying at pray area
            bool startPraying = Player.GetComponent<PlayerController>().SetPraying();

            if (startPraying)
            {
                successfullyUsed = true;
            }
        }
        else if (curItemIndex == (int)Items.WOOD_SWORD)
        {
            // try to use wood sword to purify ghost
            bool startPurifying = Player.GetComponent<PlayerController>().SetPurifying();

            if (startPurifying)
            {
                successfullyUsed = true;
            }
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
        if(curItemIndex == index)
        {
            CurrentMessage.text = "Current Taking: " + itemNames[index] + ", Remaining:" + itemCount[index];
        }
    }

    // for After 請神 get the sword
    public void GetWoodSword()
    {
        itemCount[(int)Items.WOOD_SWORD] = 1;
    }
}
