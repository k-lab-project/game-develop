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
    }

    [SerializeField] private GameObject prefab;
    public static SugangBasketManager instance;
    public List<Subject> SubjectManager;
    public Transform classesTransform;
    public void AddManager(UI.UIRecycleviewsample.UIGiveData m_GiveDataclass)
    {
    /*    string checkfirstday = SubjectManager[i].Schedule.Substring(0, 1);
        string checksecondday = SubjectManager[i].Schedule.Substring(13, 1);
        int checkfirstdate1 = int.Parse(SubjectManager[i].Schedule.Substring(2, 4));
        int checkfirstdate2 = int.Parse(SubjectManager[i].Schedule.Substring(7, 4));
        int checkseconddate1 = int.Parse(SubjectManager[i].Schedule.Substring(15, 4));
        int checkseconddate2 = int.Parse(SubjectManager[i].Schedule.Substring(20, 4));
        for (int i = 0; i < SubjectManager.Count; i++)
        {
            Debug.Log(i + "��° ������");
            //Debug.Log("�� 1000-1100,�� 1000-1100");
            if (SubjectManager[i].Schedule.Length > 20)
            {
                //��Ʋ �̻� �ִ� ��
                string firstday= SubjectManager[i].Schedule.Substring(0,1);
                string secondday= SubjectManager[i].Schedule.Substring(13,1);
                int firstdate1= int.Parse(SubjectManager[i].Schedule.Substring(2, 4));
                int firstdate2= int.Parse(SubjectManager[i].Schedule.Substring(7, 4)); 
                int seconddate1=int.Parse(SubjectManager[i].Schedule.Substring(15, 4)); 
                int seconddate2= int.Parse(SubjectManager[i].Schedule.Substring(20, 4));
                Debug.Log(firstday + firstdate1 + firstdate2);
                Debug.Log(secondday + seconddate1 + seconddate2);
            }
            else
            {
                //�Ϸ縸 �ִ� ��(�̰� ���� ���� ���Ͽ� ��� ���ϰ���)
            }
        }*/
        //SubjectManager.Insert(index,subject);
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
        DisplayClass();

    }
    private void DisplayClass()
    {
        //��ġ ���
        RemoveAllChildrenFromClasses();
        for (int i = 0; i < SubjectManager.Count; i++)
        {

            if (SubjectManager[i].Schedule.Length > 20)
            {
                float x1, x2, y1, y2;
                //��Ʋ �̻� �ִ� ��
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
                //rectTransform1.sizeDelta = new Vector2(rectTransform1.sizeDelta.x, y1);

                GameObject newTwo = Instantiate(prefab);
                newTwo.transform.SetParent(GameObject.Find("Classes").transform, false);
                RectTransform rectTransform2 = newTwo.GetComponent<RectTransform>();
                rectTransform2.anchoredPosition = new Vector3(x2, y2, 0);
                rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, seconddistance);
            }
            else
            {
                //�Ϸ縸 �ִ� ��(�̰� ���� ���� ���Ͽ� ��� ���ϰ���)
            }

        }
    }
    private float checkxlocation(string day)
    {
        //���̽��� ����
        float x1=0f;
        switch (day)
        {
            case "��":
                x1 = 27f;
                break;
            case "ȭ":
                x1 = 27f+54f*1;
                break;
            case "��":
                x1 = 27f+ 54f * 2;
                break;
            case "��":
                x1 = 27f + 54f * 3;
                break;
            case "��":
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
}