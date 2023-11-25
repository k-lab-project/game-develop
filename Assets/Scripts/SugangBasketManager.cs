using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SugangBasketManager : MonoBehaviour
{
    [System.Serializable]
    public class Subject
    {
        public int Number;
        public string ClassMGE;
        public string ClassName_KR;
        public string ClassName_EN;
        public string Schedule;
        public int Popularity;
        public int Credit;
        public float Star;
        public string Professor;
        public GameObject OneSubject;
        public GameObject TwoSubject;
    }
    
    
    [SerializeField] private GameObject prefab;
    public static SugangBasketManager instance;
    public List<Subject> SubjectManager;
    public List<Subject> FailedSubjectManager;
    public Transform classesTransform;
    public void AddManager(UI.UIRecycleviewsample.UIGiveData m_GiveDataclass)
    {
        if (!CheckDuplicate(m_GiveDataclass.GiveSchedule))
        {
            PopUpManager.instance.ShowMessageCannotApply();
        }
        else
        {
            
            SubjectManager.Add(new Subject()
            {
                Number = m_GiveDataclass.GiveNumber,
                ClassMGE = m_GiveDataclass.GiveClassMGE,
                ClassName_KR = m_GiveDataclass.GiveClassName_KR,
                ClassName_EN = m_GiveDataclass.GiveClassName_EN,
                Schedule = m_GiveDataclass.GiveSchedule,
                Popularity = m_GiveDataclass.GivePopularity,
                Credit = m_GiveDataclass.GiveCredit,
                Star = m_GiveDataclass.GiveStar,
                Professor = m_GiveDataclass.GiveProfessor
            });
            UIRecycleViewControllerSample.instance.LoadData();
            DisplayClass();

        }
    }
    public void RemoveSubject(string name)
    {
        for(int i = 0; i < SubjectManager.Count; i++)
        {
            if (SubjectManager[i].ClassName_KR == name)
            {
                SubjectManager.RemoveAt(i);
                break;
            }
        }
            
    }
    public void FailedSubject(string name)
    {
        for (int i = 0; i < SubjectManager.Count; i++)
        {
            if (SubjectManager[i].ClassName_KR == name)
            {
                FailedSubjectManager.Add(new Subject()
                {
                    Number = SubjectManager[i].Number,
                    ClassMGE = SubjectManager[i].ClassMGE,
                    ClassName_KR = SubjectManager[i].ClassName_KR,
                    ClassName_EN = SubjectManager[i].ClassName_EN,
                    Schedule = SubjectManager[i].Schedule,
                    Popularity = SubjectManager[i].Popularity,
                    Credit = SubjectManager[i].Credit,
                    Star = SubjectManager[i].Star,
                    Professor = SubjectManager[i].Professor
                });
                SubjectManager.RemoveAt(i);
                break;
            }
        }
    }
    private bool CheckDuplicate(string checkSchedule)
    {//하루 넣어야함
        string checkfirstday = checkSchedule.Substring(0, 1);
        string checksecondday = checkSchedule.Substring(13, 1);
        int checkfirstdate1 = int.Parse(checkSchedule.Substring(2, 4));
        int checkfirstdate2 = int.Parse(checkSchedule.Substring(7, 4));
        int checkseconddate1 = int.Parse(checkSchedule.Substring(15, 4));
        int checkseconddate2 = int.Parse(checkSchedule.Substring(20, 4));
        for (int i = 0; i < SubjectManager.Count; i++)
        {
            if (SubjectManager[i].Schedule.Length > 20)
            {
                string firstday = SubjectManager[i].Schedule.Substring(0, 1);
                string secondday = SubjectManager[i].Schedule.Substring(13, 1);
                int firstdate1 = int.Parse(SubjectManager[i].Schedule.Substring(2, 4));
                int firstdate2 = int.Parse(SubjectManager[i].Schedule.Substring(7, 4));
                int seconddate1 = int.Parse(SubjectManager[i].Schedule.Substring(15, 4));
                int seconddate2 = int.Parse(SubjectManager[i].Schedule.Substring(20, 4));
                if (firstday == checkfirstday)
                {
                    
                    if (checkfirstdate1 >= firstdate2 && checkfirstdate2 >= firstdate2)
                    {

                    }
                    else if (checkfirstdate1<= firstdate1 && checkfirstdate2 <= firstdate1)
                    {

                    }
                    else
                    {

                        return false;
                    }
                }
                if (firstday == checksecondday)
                {
                    if (checkfirstdate1 >= seconddate2 && checkfirstdate2 >= seconddate2)
                    {

                    }
                    else if (checkfirstdate1 <= seconddate1 && checkfirstdate2 <= seconddate1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                if (secondday == checkfirstday)
                {
                    if (checkseconddate1 >= firstdate2 && checkseconddate2 >= firstdate2)
                    {

                    }
                    else if (checkseconddate1 <= firstdate1 && checkseconddate2 <= firstdate1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                if (secondday == checksecondday)
                {
                    if (checkseconddate1 >= seconddate2 && checkseconddate2 >= seconddate2)
                    {

                    }
                    else if (checkseconddate1 <= seconddate1 && checkseconddate2 <= seconddate1)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {//하나짜리

            }
        }
        return true;
    }
    public void DisplayClass()
    {
        //위치 계싼
        RemoveAllChildrenFromClasses();
        for (int i = 0; i < SubjectManager.Count; i++)
        {

            if (SubjectManager[i].Schedule.Length > 20)
            {
                float x1, x2, y1, y2;
                //이틀 이상 있는 날
                string firstday = SubjectManager[i].Schedule.Substring(0, 1);
                int firstdate1 = int.Parse(SubjectManager[i].Schedule.Substring(2, 4));
                int firstdate2 = int.Parse(SubjectManager[i].Schedule.Substring(7, 4));
                int firstdistance = (firstdate2 - firstdate1) / 100 * 40;
                if ((firstdate2 - firstdate1) % 100 != 0)
                    firstdistance += 20;
                string secondday = SubjectManager[i].Schedule.Substring(13, 1);
                int seconddate1 = int.Parse(SubjectManager[i].Schedule.Substring(15, 4));
                int seconddate2 = int.Parse(SubjectManager[i].Schedule.Substring(20, 4));
                int seconddistance = (seconddate2 - seconddate1) / 100 * 40;
                if ((seconddate2 - seconddate1) % 100 != 0)
                    seconddistance += 20;
                x1 = checkxlocation(firstday);
                x2 = checkxlocation(secondday);
                y1 = -firstdistance / 2 + ((firstdate1 - 1000) / 100 * (-40));
                y2 = -seconddistance / 2 + ((seconddate1 - 1000) / 100 * (-40));
                if ((firstdate1 - 1000) % 100!=0)
                    y1 -= firstdistance / 2;
                if ((seconddate1 - 1000) % 100 != 0)
                    y2 -= seconddistance / 2;
                GameObject newOne = Instantiate(prefab);
                newOne.transform.SetParent(GameObject.Find("Classes").transform, false);
                RectTransform rectTransform1 = newOne.GetComponent<RectTransform>();
                rectTransform1.anchoredPosition = new Vector3(x1, y1, 0);
                rectTransform1.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, firstdistance);
                newOne.GetComponent<ChangeClass>().SubjectName.text = SubjectManager[i].ClassName_KR;
                newOne.GetComponent<ChangeClass>().Popularity.text ="인기  "+ SubjectManager[i].Popularity.ToString();
                CheckStarImage(newOne.GetComponent<ChangeClass>().Star, SubjectManager[i].Star);
                DecideBackgroundImage(i, firstdistance, newOne.GetComponent<ChangeClass>().ColorChange);
                newOne.name = i.ToString()+"1";

                SubjectManager[i].OneSubject = newOne;

                GameObject newTwo = Instantiate(prefab);
                newTwo.transform.SetParent(GameObject.Find("Classes").transform, false);
                RectTransform rectTransform2 = newTwo.GetComponent<RectTransform>();
                rectTransform2.anchoredPosition = new Vector3(x2, y2, 0);
                rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, seconddistance);
                newTwo.GetComponent<ChangeClass>().SubjectName.text = SubjectManager[i].ClassName_KR;
                newTwo.GetComponent<ChangeClass>().Popularity.text = "";
                newTwo.GetComponent<ChangeClass>().Star.enabled = false;
                DecideBackgroundImage(i, firstdistance, newTwo.GetComponent<ChangeClass>().ColorChange);
                SubjectManager[i].TwoSubject = newTwo;
                newTwo.name = i.ToString()+"2";
            }
            else
            {
                //하루만 있는 날(이건 아직 엑셀 파일에 없어서 못하겠음)
            }

        }
    }
    private float checkxlocation(string day)
    {
        //케이스문 쓰기
        float x1=0f;
        switch (day)
        {
            case "월":
                x1 = 27f;
                break;
            case "화":
                x1 = 27f+54f*1;
                break;
            case "수":
                x1 = 27f+ 54f * 2;
                break;
            case "목":
                x1 = 27f + 54f * 3;
                break;
            case "금":
                x1 = 27f + 54f * 4;
                break;
        }
        return x1;
    }
    
    private void RemoveAllChildrenFromClasses()
    {
        foreach (Transform child in classesTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DecideBackgroundImage(int num,int distance, Image colorimage)
    {
        int decide_num = num % 4;
        switch (decide_num)
        {
            case 0:
                if (distance > 50)
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/pink_long");
                }
                else
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/pink_short");
                }
                break;
            case 1:
                if (distance > 50)
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/blue_long");
                }
                else
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/blue_short");
                }
                break;
            case 2:
                if (distance > 50)
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/green_long");
                }
                else
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/green_short");
                }
                break;
            case 3:
                if (distance > 50)
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/purple_long");
                }
                else
                {
                    colorimage.sprite = Resources.Load<Sprite>("Sprite/ClassDiffer/purple_short");
                }
                break;
        }
    }
    private void CheckStarImage(Image Star,float StarNum)
    {
        string sprite_path;
        switch (StarNum)
        {
            case 0:
                sprite_path = "Sprite/addClassSearchUi/star/star00";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 0.5f:
                sprite_path = "Sprite/addClassSearchUi/star/star05";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 1:
                sprite_path = "Sprite/addClassSearchUi/star/star10";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 1.5f:
                sprite_path = "Sprite/addClassSearchUi/star/star15";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 2:
                sprite_path = "Sprite/addClassSearchUi/star/star20";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 2.5f:
                sprite_path = "Sprite/addClassSearchUi/star/star25";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 3:

                sprite_path = "Sprite/addClassSearchUi/star/star30";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 3.5f:
                sprite_path = "Sprite/addClassSearchUi/star/star35";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 4:
                sprite_path = "Sprite/addClassSearchUi/star/star40";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 4.5f:
                sprite_path = "Sprite/addClassSearchUi/star/star45";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            case 5:
                sprite_path = "Sprite/addClassSearchUi/star/star50";
                Star.sprite = Resources.Load<Sprite>(sprite_path);
                break;
            default:
                break;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated SugangBasketManager", gameObject);
        }
    }
}
