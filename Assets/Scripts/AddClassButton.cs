using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddClassButton : MonoBehaviour
{
    private  float Speed = 750.0f;
    
    private Vector3 AddClassUIStart = new Vector3(0,-177.5f,0);
    private Vector3 AddClassUIEnd = new Vector3(0, 177.5f, 0);
    private Vector3 TimeScheduleStart = new Vector3(9, -218f, 0);
    private Vector3 TimeScheduleEnd = new Vector3(9, -49f, 0);
    bool isCameraMovingT = false;
    bool isCameraMovingA = false;
    [SerializeField] private GameObject AddClassBut;
    [SerializeField] private GameObject Else;
    [SerializeField] private GameObject AddClassUI;
    [SerializeField] private GameObject TimeSchedule;
    [SerializeField] private GameObject Xbutton;
    // Start is called before the first frame update
    public void OnMouseAdcClassButClick()
    {
        AddClassBut.SetActive(false);
        Else.SetActive(false);
        AddClassUI.SetActive(true);
        Xbutton.SetActive(true);
        StartCoroutine(MoveUIDownToUpForT(TimeSchedule, TimeScheduleStart, TimeScheduleEnd));
        StartCoroutine(MoveUIDownToUpForA(AddClassUI,AddClassUIStart,AddClassUIEnd));
    }

    public void OnMouseXButtonClick()
    {
        AddClassBut.SetActive(true);
        Else.SetActive(true);
        AddClassUI.SetActive(false);
        Xbutton.SetActive(false);
        StartCoroutine(MoveUIDownToUpForT(TimeSchedule, TimeScheduleEnd, TimeScheduleStart));
        StartCoroutine(MoveUIDownToUpForA(AddClassUI, AddClassUIEnd, AddClassUIStart));
    }
    IEnumerator MoveUIDownToUpForT(GameObject obj, Vector3 Start, Vector3 End)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        isCameraMovingT = true;

        float journeyLength = Vector3.Distance(Start, End);
        float startTime = Time.time;

        while (isCameraMovingT)
        {
            float distanceCovered = (Time.time - startTime) * Speed;
            float journeyFraction = distanceCovered / journeyLength;
            Vector3 newPosition = Vector3.Lerp(Start, End, journeyFraction);
            rectTransform.anchoredPosition = new Vector3(newPosition.x, newPosition.y, newPosition.z); // 2D 위치 업데이트
            if (journeyFraction >= 1.0f)
            {
                isCameraMovingT = false;
            }

            yield return null;
        }
    }
    IEnumerator MoveUIDownToUpForA(GameObject obj, Vector3 Start, Vector3 End)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        isCameraMovingA = true;

        float journeyLength = Vector3.Distance(Start, End);
        float startTime = Time.time;

        while (isCameraMovingA)
        {
            float distanceCovered = (Time.time - startTime) * Speed*2.08f;
            float journeyFraction = distanceCovered / journeyLength;
            Vector3 newPosition = Vector3.Lerp(Start, End, journeyFraction);
            rectTransform.anchoredPosition = new Vector3(newPosition.x, newPosition.y, newPosition.z); // 2D 위치 업데이트
            
            if (journeyFraction >= 1.0f)
            {
                isCameraMovingA = false;
            }

            yield return null;
        }
    }
}
