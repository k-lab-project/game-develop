using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EndBasketManager : MonoBehaviour
{
    public static EndBasketManager instance;
    public bool isTrue=false;
    [SerializeField] private GameObject SuccessPreafb;
    [SerializeField] private GameObject FailedPrefab;
    [SerializeField] private TextMeshProUGUI CurrentCreditTMP;
    [SerializeField] private TextMeshProUGUI LossSubjectTMP;
    [SerializeField] private TextMeshProUGUI TotalScoreTMP;
    [SerializeField] private GameObject CurrentCreditObj;
    [SerializeField] private GameObject LossSubjectObj;
    [SerializeField] private GameObject TotalScoreObj;
    [SerializeField] private GameObject NextButtonObj;
    private bool oneTime = true;
    private int cnt;
    private int creditcnt=0;
    private int currentcredit;
    private int subjectcnt = 0;
    private int losscnt=0;
    private int losscredit = 0;
    private int totalscorecnt = 0;
    private float totalscorebasket;
    private float currentscorebasket;
    private void Awake()
    {
        instance = this;   
    }

    private void Update()
    {
        if (isTrue && oneTime)
        {
            cnt = SugangBasketManager.instance.SubjectManager.Count - 1;
            totalscorebasket = Mathf.Ceil((ScoreManager.instance.totalscore));
            oneTime = false;
            CurrentCreditTMP.text = ScoreManager.instance.creditcnt.ToString();
            TotalScoreTMP.text= (Mathf.Ceil(ScoreManager.instance.totalscore)).ToString();
            WhatSubjectSuccess();
        }
    }
    private void WhatSubjectSuccess()
    {
        System.Random rand = new System.Random();
        
        currentcredit += SugangBasketManager.instance.SubjectManager[cnt].Credit;
            int rand_num = rand.Next(0, 90);
        
        if (ScoreManager.instance.RiskPopularity < rand_num)
        {
            //¼º°ø
            GameObject SuccessText = Instantiate(SuccessPreafb);
            SuccessText.transform.SetParent(GameObject.Find("SuccessContent").transform, false);
            SuccessText.GetComponent<SuccessFailClass>().text.text = SugangBasketManager.instance.SubjectManager[cnt].ClassName_KR + " (" + SugangBasketManager.instance.SubjectManager[cnt].Credit + ")";
        }
        else
        {
            //½ÇÆÐ
            GameObject FailedText = Instantiate(FailedPrefab);
            FailedText.transform.SetParent(GameObject.Find("FailedContent").transform, false);
            FailedText.GetComponent<SuccessFailClass>().text.text = SugangBasketManager.instance.SubjectManager[cnt].ClassName_KR + " (" + SugangBasketManager.instance.SubjectManager[cnt].Credit + ")";
            SugangBasketManager.instance.SubjectManager[cnt].OneSubject.GetComponent<ChangeClass>().Star.enabled = false;
            SugangBasketManager.instance.SubjectManager[cnt].OneSubject.GetComponent<ChangeClass>().Popularity.enabled = false;
            SugangBasketManager.instance.SubjectManager[cnt].OneSubject.GetComponent<ChangeClass>().TombImage.SetActive(true);
            SugangBasketManager.instance.SubjectManager[cnt].OneSubject.GetComponent<ChangeClass>().TombImageBackground.SetActive(true);    
            if (SugangBasketManager.instance.SubjectManager[cnt].TwoSubject != null)
            {
                SugangBasketManager.instance.SubjectManager[cnt].TwoSubject.GetComponent<ChangeClass>().Star.enabled = false;
                SugangBasketManager.instance.SubjectManager[cnt].TwoSubject.GetComponent<ChangeClass>().Popularity.enabled = false;
                SugangBasketManager.instance.SubjectManager[cnt].TwoSubject.GetComponent<ChangeClass>().TombImage.SetActive(true);
                SugangBasketManager.instance.SubjectManager[cnt].TwoSubject.GetComponent<ChangeClass>().TombImageBackground.SetActive(true);
            }
            
            losscnt++;
            losscredit += SugangBasketManager.instance.SubjectManager[cnt].Credit;
            SugangBasketManager.instance.FailedSubject(SugangBasketManager.instance.SubjectManager[cnt].ClassName_KR);
        }
        if (cnt - 1 >= 0)
        {
            
            StartCoroutine(DisplayWhatSubjectSuccess());
        }
        else
        {
            
            StartCoroutine(DisplayLossSubject());
        }
    }
    private void ShowLossSubject()
    {
        if (subjectcnt > losscnt)
        {
            CurrentCreditObj.SetActive(true);
            StartCoroutine(DisplayCurrentCredit());
        }
        else
        {
            LossSubjectTMP.text = (-subjectcnt).ToString() +"°ú¸ñ";
            subjectcnt++;
            StartCoroutine(DisplayLossSubject());
        }
    }
    private void ShowCurrentCredit()
    {
        if (creditcnt > losscredit)
        {
            TotalScoreObj.SetActive(true);
            ScoreManager.instance.UpdateScore();
            currentscorebasket= Mathf.Ceil((ScoreManager.instance.totalscore));
            StartCoroutine(DisplayTotalScore());
        }
        else {
            
            CurrentCreditTMP.text = (currentcredit - creditcnt).ToString();
            creditcnt++;
            StartCoroutine(DisplayCurrentCredit());
          }
    }

    private void ShowTotalScore()
    {
        if (totalscorebasket - totalscorecnt < currentscorebasket)
        {
            
            NextButtonObj.SetActive(true);
        }
        else
        {
            TotalScoreTMP.text = (totalscorebasket - totalscorecnt).ToString();
            totalscorecnt++;
            StartCoroutine(DisplayTotalScore());
        }
    }
    IEnumerator DisplayWhatSubjectSuccess()
    {//°ú¸ñ µýµýµý
        yield return new WaitForSecondsRealtime(1f);
        {
            cnt--;
            WhatSubjectSuccess();
            
        }

    }
    IEnumerator DisplayCurrentCredit()
    {//ÇÐÁ¡ µýµýµý
        yield return new WaitForSecondsRealtime(0.2f);
        {

            ShowCurrentCredit();

        }
    }
    IEnumerator DisplayLossSubject()
    {
        SoundManager.instance.tombCome();
        yield return new WaitForSecondsRealtime(1.5f);
        {
            if (LossSubjectObj.activeSelf==false){

                LossSubjectObj.SetActive(true);
            }   
            ShowLossSubject();

        }
    }
    IEnumerator DisplayTotalScore()
    {
        SoundManager.instance.scoreGoDown();
        yield return new WaitForSecondsRealtime(0.05f);
        {

            ShowTotalScore();

        }
    }
}
