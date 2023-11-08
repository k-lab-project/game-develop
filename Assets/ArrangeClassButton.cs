using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrangeClassButton : MonoBehaviour
{
    public static ArrangeClassButton instance;

    [SerializeField] private TextMeshProUGUI AreaText;
    [SerializeField] private TextMeshProUGUI SortText;
    [SerializeField] private TextMeshProUGUI CreditText;
    [SerializeField] private GameObject Area;
    [SerializeField] private GameObject Sort;
    [SerializeField] private GameObject Credit;
    public string SortByMajor="ALL";//ALL,M,GE
    public string SortByMiddle="STAR";//star,popularity,name
    public int SortByCredit=1;//all,2,3

    void Awake()
    {
        SortByMajor = "ALL";//ALL,M,GE
        SortByMiddle = "STAR";//star,popularity,name
        SortByCredit = 1;//all,2,3
        instance = this;
    }
    // Start is called before the first frame update
    public void ClickArea()
    {
        if (SortByMajor == "ALL")
        {
            AreaText.text = "����/����   ����";
            SortByMajor = "M";
        }
        else if (SortByMajor == "M")
        {
            AreaText.text = "����/����   ����";
            SortByMajor = "GE";
        }
        else if (SortByMajor == "GE")
        {
            AreaText.text = "����/����   ��ü";
            SortByMajor = "ALL";
        }
        
        UIRecycleViewControllerSample.instance.LoadData();
    }
    public void ClickSort()
    {
        if (SortByMiddle == "STAR")
        {
            SortByMiddle = "POPULARITY";
            SortText.text = "����     �α⵵";
        }
        else if (SortByMiddle == "POPULARITY")
        {
            SortByMiddle = "NAME";
            SortText.text = "����     �̸���";
        }
        else if (SortByMiddle == "NAME")
        {
            SortByMiddle = "STAR";
            SortText.text = "����      ����";
        }
        UIRecycleViewControllerSample.instance.LoadData();
    }
    public void ClickCredit()
    {
        if (SortByCredit == 1)
        {
            CreditText.text = "����    2����";
            SortByCredit = 2;
        }
        else if (SortByCredit == 2)
        {
            CreditText.text = "����    3����";
            SortByCredit = 3;
        }
        else if (SortByCredit == 3)
        {
            CreditText.text = "����     ��ü";
            SortByCredit = 1;
        }
        UIRecycleViewControllerSample.instance.LoadData();
    }
}