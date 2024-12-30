using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedSlotController : MonoBehaviour, IDropHandler
{
    [Header("Parameters")]
    [SerializeField]
    private GameObject[] PrefabsRemnantsUIsprite = new GameObject[(int)GlobalVar.NUM_REMNANT_TYPE];

    private int currentSelectRemnantID = -1;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject DroppedObj = eventData.pointerDrag;

        // for UI part
        if(currentSelectRemnantID != -1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        // update currentSelectRemnantID
        currentSelectRemnantID = DroppedObj.GetComponent<RemnantsUIController>().r_ID;
        Instantiate(PrefabsRemnantsUIsprite[currentSelectRemnantID], transform);
    }

    // public function for backpack to get the selected R_ID
    public int GetSelectedRemnantID()
    {
        return currentSelectRemnantID;
    }
}
