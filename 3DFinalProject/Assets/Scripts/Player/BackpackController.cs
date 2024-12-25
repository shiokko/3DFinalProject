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
    private GameObject Backpack;
    [SerializeField]
    private GameObject[] UIgrids = new GameObject[(int)GlobalVar.NUM_REMNANT_CATEGORY];
    [SerializeField]
    private GameObject[] PrefabsUIremnants = new GameObject[(int)GlobalVar.NUM_REMNANT_TYPE];

    private bool backpackMode;

    // Start is called before the first frame update
    void Start()
    {
        backpackMode = false;
    }

    // Update is called once per frame
    void Update()
    {
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
                Backpack.SetActive(true);
                Player.GetComponent<PlayerController>().ResetCanMove();
            }
            else
            {
                Backpack.SetActive(false);
                Player.GetComponent<PlayerController>().SetCanMove();
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
            Instantiate(PrefabsUIremnants[index], UIgrids[0].transform);
        }
        else if (index <= (int)Remnants.CRUTCH)
        {
            // category age:
            Instantiate(PrefabsUIremnants[index], UIgrids[1].transform);
        }
        else if (index <= (int)Remnants.BOWL)
        {
            // category Hierarchy:
            Instantiate(PrefabsUIremnants[index], UIgrids[2].transform);
        }
    }
}
