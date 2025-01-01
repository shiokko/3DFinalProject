using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class HelpControll : MonoBehaviour
{
    [Header("Page")]
    [SerializeField] private GameObject CatalogPage;
    [SerializeField] private GameObject ItemPage;
    [SerializeField] private GameObject RemnantPage;
    [SerializeField] private GameObject HowToPlayPage;
    [SerializeField] private GameObject ControlPage;

    [Header("Catalog")]
    [SerializeField] private Button Item;  // 改为private，但依然在Inspector中可见
    [SerializeField] private Button Renmanat;
    [SerializeField] private Button HowToPlay;
    [SerializeField] private Button Control;

    void Start()
    {
        // 可以在这里为按钮添加监听器
        Item.onClick.AddListener(OnItemClick);
        Renmanat.onClick.AddListener(OnRenmanatClick);
        HowToPlay.onClick.AddListener(OnHowToPlayClick);
        Control.onClick.AddListener(OnControlClick);
    }

    private void OnItemClick()
    {
        Debug.Log("Item Button Clicked");
    }

    private void OnRenmanatClick()
    {
        Debug.Log("Renmanat Button Clicked");
    }

    private void OnHowToPlayClick()
    {
        Debug.Log("HowToPlay Button Clicked");
    }

    private void OnControlClick()
    {
        Debug.Log("Control Button Clicked");
    }
}
