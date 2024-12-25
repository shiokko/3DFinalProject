using StarterAssets;
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

    [Header("Dependencies")]
    [SerializeField]
    private GameObject Backpack;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject[] ItemsUIslots = new GameObject[(int)GlobalVar.NUM_ITEM_TYPE];

    private StarterAssetsInputs _input;

    private int curItemIndex = 0;  // default to not taking anything
    private int prevItemIndex;

    private int[] itemCount = new int[(int)GlobalVar.NUM_ITEM_TYPE];  // get currently how many items there are

    // Start is called before the first frame update
    void Start()
    {
        _input = Player.GetComponent<StarterAssetsInputs>();

        curItemIndex = 0;
        prevItemIndex = 0;

        itemCount[(int)Items.CHARM] = InitCharmNum;
        itemCount[(int)Items.DIVINATION_BLOCK] = InitDivinationBlockNum;
        itemCount[(int)Items.INCENSE] = initIncenseNum;
        itemCount[(int)Items.WOOD_SWORD] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        prevItemIndex = curItemIndex;

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
            return;
        }

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
        // UI part
        if(curItemIndex == 0)
        {
            // switch to take nothing
            if (prevItemIndex != 0)   // in this case prevItemIndex should not be 0
            {
                ItemsUIslots[prevItemIndex].GetComponent<SlotManager>().ToggleSprite();
            }
            else
            {
                Debug.Log("something weong in SelectItem()");
            }
        }
        else
        {
            // switching to current taking
            ItemsUIslots[curItemIndex].GetComponent<SlotManager>().ToggleSprite();
            if (prevItemIndex != 0) 
            { 
                ItemsUIslots[prevItemIndex].GetComponent<SlotManager>().ToggleSprite();
            }
        }

        // player visiual part
        int i = 0;
        foreach (Transform item in transform)
        {
            if(i == 0)
            {
                // skip latern part
                i++;
                continue;
            }

            if (i == curItemIndex)
            {
                if (itemCount[i] > 0)
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
        if(curItemIndex == 0 || itemCount[curItemIndex] == 0)  // only holding lantern, or nothing can be used
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
            // call function to ask god, first we let player controller know we are using divination block
            // then, player controller activates funhus functions
            bool startAsking = Player.GetComponent<PlayerController>().SetAskGod();

            if (startAsking)
            {
                // show mouse and disable screen rotation
                _input.cursorInputForLook = false;
                _input.cursorLocked = false;
                _input.look = Vector2.zero;
                _input.move = Vector2.zero;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                successfullyUsed = true;
            }
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
            // for UI part
            ItemsUIslots[curItemIndex].GetComponent<SlotManager>().DecreaseItemCount();

            itemCount[curItemIndex] --;
            // refresh player visiual part
            // we will disable the current taking if count is 0
            // we will toggle UI sprite twice, since currentItemIndex == prevItemIndex
            SelectItem();
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

        // for UI part
        ItemsUIslots[index].GetComponent<SlotManager>().IncreaseItemCount();

        itemCount[index] ++;
        if(curItemIndex == index)
        {
            SelectItem();
        }
    }

    // for After 請神 get the sword
    public void GetWoodSword()
    {
        // for UI part
        ItemsUIslots[(int)Items.WOOD_SWORD].GetComponent<SlotManager>().IncreaseItemCount();

        itemCount[(int)Items.WOOD_SWORD] = 1;
        Debug.Log("Wood Sword Get!!!");

        if (curItemIndex == (int)Items.WOOD_SWORD)
        {
            SelectItem();
        }
    }
}
