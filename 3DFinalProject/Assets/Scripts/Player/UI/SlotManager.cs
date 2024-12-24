using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] 
    private RectTransform slotRect; // The RectTransform of the slot
    [SerializeField] 
    private GameObject countPrefab; // Prefab for the count text
    [SerializeField] 
    private int itemCount = 0; // Starting count of items

    private TextMeshProUGUI countText; // Reference to the instantiated count text

    void Start()
    {
        // Instantiate the count text and anchor it to the bottom-right of the slot
        GameObject countObject = Instantiate(countPrefab, slotRect.parent);
        countObject.transform.SetParent(countObject.transform.root);
        countText = countObject.GetComponent<TextMeshProUGUI>();

        // Position the count text relative to the slot's position
        RectTransform countRect = countObject.GetComponent<RectTransform>();
        Vector3 slotPosition = slotRect.position; // World-space position of the slot

        // Set the count text position to bottom-right corner of the slot
        countRect.position = slotPosition; // Start with the slot's position
        countRect.anchoredPosition += new Vector2(slotRect.rect.width / 2, -slotRect.rect.height / 2); // Adjust to bottom-right

        UpdateCountText();
    }

    // Decrease the count and update the display
    public void DecreaseItemCount()
    {
        if (itemCount > 0)
        {
            itemCount--;
            UpdateCountText();
        }
    }

    // Update the count text
    private void UpdateCountText()
    {
        countText.text = itemCount.ToString();
    }
}
