using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelect : MonoBehaviour
{
    [Header("Self")]
    [SerializeField]
    private Image _image;

    [Header("Dependencies")]
    [SerializeField]
    private Sprite[] SpriteChoice = new Sprite[2];

    private int currentSpriteID = 0;

    public void ToggleSprite()
    {
        if(currentSpriteID == 0)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleSprite();
        }
    }
}
