using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;

public class BackpackController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject BackpackUI;
    [SerializeField]
    private GameObject SelectedSlot;
    [SerializeField]
    private GameObject FungusTrigger;
    [SerializeField]
    private GameObject[] UIGrids = new GameObject[(int)GlobalVar.NUM_REMNANT_CATEGORY];
    [SerializeField]
    private GameObject[] PrefabsUIRemnantSlots = new GameObject[(int)GlobalVar.NUM_REMNANT_TYPE];

    private bool backpackMode;
    private bool isChoosing;

    // Start is called before the first frame update
    void Start()
    {
        backpackMode = false;
        isChoosing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<PlayerController>().Iskilled())
        {
            if (backpackMode)
            {
                backpackMode = false;
                BackpackUI.SetActive(false);
            }
            return;
        }

        GetInputs();
    }

    private void GetInputs()
    {
        // toggle backpackMode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            backpackMode = !backpackMode;

            // show backpack UI
            if (backpackMode)
            {
                BackpackUI.SetActive(true);
                Player.GetComponent<PlayerController>().ResetCanMove();
            }
            else
            {
                BackpackUI.SetActive(false);
                Player.GetComponent<PlayerController>().SetCanMove();

                if (isChoosing)
                {
                    // FungusTrigger enable the backpack mode,
                    // and right now, player has chosen the desired remnant then tabbing out of backpack mode
                    if(SelectedSlot.GetComponent<SelectedSlotController>().GetSelectedRemnantID() == -1)
                    {
                        FungusTrigger.GetComponent<FungusTrigger>().BroadCastPunishment();
                    }
                    else
                    {
                        // to check if player really select a remnant to ask
                        FungusTrigger.GetComponent<FungusTrigger>().BroadCastRemnantSelected();
                    }

                    isChoosing = false;
                }
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
        if (index < 0 || index >= (int)GlobalVar.NUM_REMNANT_TYPE)
        {
            Debug.Log("index out of range in IncrementRemnantCount, passed wrong params");
            return;
        }

        if(index <= (int)Remnants.FAN)
        {
            // category sex:
            // add prefab to backpack, as a child of corresponding grid category
            Instantiate(PrefabsUIRemnantSlots[index], UIGrids[0].transform);
        }
        else if (index <= (int)Remnants.CRUTCH)
        {
            // category age:
            Instantiate(PrefabsUIRemnantSlots[index], UIGrids[1].transform);
        }
        else if (index <= (int)Remnants.BOWL)
        {
            // category Hierarchy:
            Instantiate(PrefabsUIRemnantSlots[index], UIGrids[2].transform);
        }
    }

    // for FungusTrigger to open backpack, forcing user to choose remnant before asking
    public void SetBackpackMode()
    {
        backpackMode = true;

        // UI part
        BackpackUI.SetActive(true);
        Player.GetComponent<PlayerController>().ResetCanMove();
    }

    // for FungusTrigger to set when player really wants to take remnant out of backpack to "ask"
    public void SetIsChoosing()
    {
        isChoosing = true;
    }

    // for FungusTrigger to get selected remnant id
    public int GetSelectedRemnantID()
    {
        return SelectedSlot.GetComponent<SelectedSlotController>().GetSelectedRemnantID();
    }
}
