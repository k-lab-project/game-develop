using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBasketButton : MonoBehaviour
{
    [SerializeField] private GameObject FinishClass_NotActive;
    [SerializeField] private GameObject FinishClass_Active;
    [SerializeField] private GameObject TopNavSearch;
    [SerializeField] private GameObject TopNavRefresh;
    public bool IsCredit12=false;
    public static EndBasketButton instance;

    private void Awake()
    {
        instance = this;
    }
    public void CheckBasket()
    {
        if (IsCredit12)
        {
            FinishClass_Active.SetActive(true);
            FinishClass_NotActive.SetActive(false);
        }
        else
        {
            FinishClass_Active.SetActive(false);
            FinishClass_NotActive.SetActive(true);
        }
            
    }
    public void StartRealSugang()
    {
        TopNavSearch.SetActive(false);
        TopNavRefresh.SetActive(true);
        RealSugangSystem.instance.RealSugangStarted = true;
        UIRecycleViewControllerSample.instance.LoadData();
        SugangBasketManager.instance.DisplayClass();
        AddClassButton.instance.StartSugangButClick();
    }
}
