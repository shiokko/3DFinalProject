using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CloseWarning : MonoBehaviour
{
    public GhostController Ghost; // ghost reference
    public TextMeshProUGUI WarningText; // TextMeshPro

    [SerializeField]
    private float warningDistance = 10f; // warning distance

    void Update()
    {
        
        if (Ghost.GetDistanceToPlayer() <= warningDistance)
        {
            ShowWarning(true);
        }
        else
        {
            ShowWarning(false);
        }
    }

    private void ShowWarning(bool isActive)
    {
        if (isActive)
        {
            // visible
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 1f); // 
        }
        else
        {
            // 
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0f); // invisible but exist
        }
    }
}
