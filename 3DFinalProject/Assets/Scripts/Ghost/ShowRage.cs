using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空間

public class ShowRage : MonoBehaviour
{
    public GameObject Ghost; // 鬼的遊戲物件
    public Text rageText;    // 用於顯示怒氣的 UI Text

    private GhostController ghostController; // 鬼腳本的引用

    void Start()
    {
        // 從 Ghost 遊戲物件獲取 GhostController 腳本
        ghostController = Ghost.GetComponent<GhostController>();
    }

    void Update()
    {
        // 獲取鬼的怒氣值
        float currentRage = ghostController.GetRage();

        // 更新 Text 顯示
        rageText.text = "Rage: " + currentRage.ToString("F0"); // F0 格式化為無小數點
    }
}