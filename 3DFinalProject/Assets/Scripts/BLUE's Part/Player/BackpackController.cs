using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackpackController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private TextMeshProUGUI CurrentMessage;

    private int curRemnantIndex;

    private int[] remnantCount = new int[(int) GlobalVar.NUM_REMNANT_TYPE];  // get currently how many remnant there are
    private string[] remnantNames = new string[(int)GlobalVar.NUM_REMNANT_TYPE];  // just a temp UI, will use other implementation in the future 

    [SerializeField]
    private bool backpackMode;

    private StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();

        backpackMode = false;
        CurrentMessage.enabled = true;
        CurrentMessage.text = "[Backpack mode] Want to Take: COMB";

        curRemnantIndex = 0;

        // temp UI
        remnantNames[(int)Remnants.FAN] = "Fan";
        remnantNames[(int)Remnants.TOY] = "Toy";
        remnantNames[(int)Remnants.GOLD] = "Gold";
        remnantNames[(int)Remnants.JADE] = "Jade";
        remnantNames[(int)Remnants.BOWL] = "Bowl";
        remnantNames[(int)Remnants.HAT] = "Hat";
        remnantNames[(int)Remnants.COMB] = "Comb";
        remnantNames[(int)Remnants.CRUTCH] = "Crutch";
    }

    // Update is called once per frame
    void Update()
    {
        int prevRemnantIndex = curRemnantIndex;

        GetInputs();

        if (prevRemnantIndex != curRemnantIndex)
        {
            SelectItem();
        }
    }

    private void GetInputs()
    {
        // for future backpack usage
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    _input.cursorInputForLook = !_input.cursorInputForLook;
        //    _input.look = Vector2.zero;
        //    //  canvas call backpack to drag drop
        //}


        // CAUTIOUS!! below implements are just temporary, I will use back drag drop afterward
        // toggle backpackMode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            backpackMode = !backpackMode;
        }

        if (!backpackMode)
        {
            CurrentMessage.enabled = false;
            return;
        }
        CurrentMessage.enabled = true;

        // for item switching
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (curRemnantIndex >= (int)GlobalVar.NUM_REMNANT_TYPE - 1)
            {
                curRemnantIndex = (int)GlobalVar.NUM_REMNANT_TYPE - 1;
            }
            else
            {
                curRemnantIndex++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (curRemnantIndex <= 0)
            {
                curRemnantIndex = 0;
            }
            else
            {
                curRemnantIndex--;
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
        for(int i = 0; i < (int)GlobalVar.NUM_REMNANT_TYPE; i++)
        {
            if (i == curRemnantIndex)
            {
                if (remnantCount[i] > 0)
                {
                    CurrentMessage.text = "[Backpack mode] Current Taking: " + remnantNames[i] + ", Remaining:" + remnantCount[i];
                }
                else
                {
                    CurrentMessage.text = "[Backpack mode] Want to Take: " + remnantNames[i];
                }
            }
        }
    }

    private void UseItem()
    {
        if (remnantCount[curRemnantIndex] == 0)  // nothing to use
        {
            return;
        }

        for(int i = 0; i < (int)GlobalVar.NUM_REMNANT_TYPE; i++)
        {
            if(i == curRemnantIndex)
            {
                // call method in god interaction HERE!

                remnantCount[i] --;
                CurrentMessage.text = "[Backpack mode] Want to Take: " + remnantNames[i];
                backpackMode = false;
            }
        }
    }

    // Public function here

    // for item controller to see if is using backpack
    public bool IsBackpackMode()
    {
        return backpackMode;
    }

    // for First Person Raycast
    public void IncrementRemnantCount(int index)
    {
        if (index < 0 || index >= remnantCount.Length)
        {
            Debug.Log("index out of range in IncrementRemnantCount, passed wrong params");
            return;
        }

        remnantCount[index]++;
        if (curRemnantIndex == index)
        {
            CurrentMessage.text = "[Backpack mode] Current Taking: " + remnantNames[index] + ", Remaining:" + remnantCount[index];
        }
    }
}
