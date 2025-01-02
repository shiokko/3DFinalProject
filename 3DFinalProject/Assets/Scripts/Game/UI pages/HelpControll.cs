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
    [SerializeField] private GameObject GodPage;

    [Header("bottom")]
    [SerializeField] private Button Item;  // 改为private，但依然在Inspector中可见
    [SerializeField] private Button Renmanat;
    [SerializeField] private Button HowToPlay;
    [SerializeField] private Button Control;
    [SerializeField] private Button God;
    [SerializeField] private Button Back1;
    [SerializeField] private Button Back2;
    [SerializeField] private Button Back3;
    [SerializeField] private Button Back4;
    [SerializeField] private Button Back5;


    private GameObject currentPage;

    void Start()
    {
        // 可以在这里为按钮添加监听器
        Item.onClick.AddListener(OnItemClick);
        Renmanat.onClick.AddListener(OnRenmanatClick);
        HowToPlay.onClick.AddListener(OnHowToPlayClick);
        Control.onClick.AddListener(OnControlClick);
        God.onClick.AddListener(OnGodClick);
        Back1.onClick.AddListener(OnBackClick);
        Back2.onClick.AddListener(OnBackClick);
        Back3.onClick.AddListener(OnBackClick);
        Back4.onClick.AddListener(OnBackClick);
        Back5.onClick.AddListener(OnBackClick);

        currentPage = CatalogPage;
        CatalogPage.SetActive(true);
        ItemPage.SetActive(false);
        RemnantPage.SetActive(false);
        HowToPlayPage.SetActive(false);
        ControlPage.SetActive(false);
        GodPage.SetActive(false);
    }

    private void OnItemClick()
    {
        Debug.Log("Item Button Clicked");
        currentPage.SetActive(false);
        ItemPage.SetActive(true);
        currentPage = ItemPage;
    }

    private void OnRenmanatClick()
    {
        Debug.Log("Renmanat Button Clicked");
        currentPage.SetActive(false);
        RemnantPage.SetActive(true);
        currentPage = RemnantPage;
    }

    private void OnHowToPlayClick()
    {
        Debug.Log("HowToPlay Button Clicked");
        currentPage.SetActive(false);
        HowToPlayPage.SetActive(true);
        currentPage = HowToPlayPage;
    }

    private void OnControlClick()
    {
        Debug.Log("Control Button Clicked");
        currentPage.SetActive(false);
        ControlPage.SetActive(true);
        currentPage = ControlPage;
    }
    private void OnGodClick()
    {
        Debug.Log("Back Button Clicked");
        currentPage.SetActive(false);
        GodPage.SetActive(true);
        currentPage = GodPage;
    }
    private void OnBackClick()
    {
        Debug.Log("Back Button Clicked");
        currentPage.SetActive(false);
        CatalogPage.SetActive(true);
        currentPage = CatalogPage;
    }
}
