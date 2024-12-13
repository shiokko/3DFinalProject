using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空間

public class CloseWarning : MonoBehaviour
{
    public GhostController Ghost; // 連結鬼的控制器
    public TextMeshProUGUI WarningText; // TextMeshPro 的文字物件

    [SerializeField]
    private float warningDistance = 10f; // 警告距離

    void Update()
    {
        // 判斷鬼是否接近玩家
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
            // 啟用警告文字並設置內容
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 1f); // 設為完全可見
        }
        else
        {
            // 設置文字為透明而非禁用
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0f); // 設為完全透明
        }
    }
}
