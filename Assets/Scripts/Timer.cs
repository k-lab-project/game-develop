using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject TimerObj;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject TimerRedObj;
    [SerializeField] private TextMeshProUGUI TimerRedText;
    private bool firsttime=false;
    private bool secondtime = false;
    private int timercnt = 0;
    private int endtimercnt = 0;
    private int untilRealStart = 10;//120초 국룰
    private int EndTimer = 30;//타이머 시간초 끝내기
    private void Update()
    {
        if (RealSugangSystem.instance.RealSugangStarted&&!firsttime)
        {
            firsttime = true;
            TimerObj.SetActive(true);
            TimerAction();
        }
        if (RealSugangSystem.instance.TimerFinished && !secondtime)
        {
            secondtime = true;
            TimerObj.SetActive(false);
            TimerRedObj.SetActive(true);
            EndTimerAction();
        }
    }

    private void EndTimerAction()
    {
        if (endtimercnt > EndTimer)
        {
            RealSugangSystem.instance.StartFinalStatus();
        }
        else
        {
            if (EndTimer - endtimercnt < 10)
            {
                TimerRedText.text = "0:0" + (EndTimer - endtimercnt).ToString();
            }
            else
            {
                TimerRedText.text = "0:" + (EndTimer - endtimercnt).ToString();
            }
            StartCoroutine(DisplayTimerEnd());
        }
    }
    private void TimerAction()
    {
        if (timercnt > untilRealStart)
        {

            RealSugangSystem.instance.TimerFinished = true;
            
            UIRecycleViewControllerSample.instance.LoadData();
            SugangBasketManager.instance.DisplayClass();

        }
        else
        {
            if (untilRealStart - timercnt >= 120)
            {
                TimerText.text = "2:00";
            }
            else if (untilRealStart - timercnt >= 60)
            {
                if (60 - timercnt < 10)
                {
                    TimerText.text = "1:0" + (untilRealStart -60 - timercnt).ToString();
                }
                else
                {
                    TimerText.text = "1:" + (untilRealStart -60 - timercnt).ToString();
                }

            }
            else
            {
                if (untilRealStart - timercnt < 10)
                {
                    TimerText.text = "0:0" + (untilRealStart - timercnt).ToString();
                }
                else
                {
                    TimerText.text = "0:" + (untilRealStart - timercnt).ToString();
                }
            }
            StartCoroutine(DisplayTimer());
        }
    }
    IEnumerator DisplayTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        {
            timercnt++;
            TimerAction();
        }
    }

    IEnumerator DisplayTimerEnd()
    {
        yield return new WaitForSecondsRealtime(1f);
        {
            
            endtimercnt++;
            EndTimerAction();
        }
    }
}
