using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [Header("Self")]
    [SerializeField]
    private Image _image;

    [Header("Dependencies")]
    [SerializeField]
    private TextMeshProUGUI CountText;
    [SerializeField]
    private Sprite[] SpriteChoice = new Sprite[2];


    private int itemCount = 0;   // Starting count of items

    private int currentSpriteID = 0;  // starting by selecting nothing


    void Start()
    {
        //countText = countTextGameObj.GetComponent<TextMeshPro>();

        UpdateCountText();
    }

    // Update the count text
    private void UpdateCountText()
    {
        if(itemCount == 0)
        {
            CountText.color = Color.red;
        }
        else
        {
            CountText.color = Color.white;
        }
        CountText.text = itemCount.ToString();
    }

    // public functions here
    // Decrease the count and update the display
    public void IncreaseItemCount()
    {
        itemCount++;
        UpdateCountText();
    }

    public void DecreaseItemCount()
    {
        if(itemCount > 0)
        {
            itemCount--;
            UpdateCountText();
        }
    }

    // toggle select indicator
    public void ToggleSprite()
    {
        if (currentSpriteID == 0)
        {
            currentSpriteID = 1;
            _image.sprite = SpriteChoice[currentSpriteID];
        }
        else
        {
            currentSpriteID = 0;
            _image.sprite = SpriteChoice[currentSpriteID];
        }
    }

}
