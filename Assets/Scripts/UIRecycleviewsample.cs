using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

namespace UI
{
    [System.Serializable]
    public class UICellSampleData
    {
        public int Number;
        public string ClassMGE;
        public string ClassName_KR;
        public string ClassName_EN;
        public string Schedule_Day;
        public string Schedule_Date;
        public int Popularity;
        public int Credit;
        public float Star;
        public string Professor;
    }

    public class UIRecycleviewsample : UIRecycleViewCell<UICellSampleData>
    {

        public class UIGiveData
        {
            public int GiveNumber;
            public string GiveClassMGE;
            public string GiveClassName_KR;
            public string GiveClassName_EN;
            public string GiveSchedule;
            public int GivePopularity;
            public int GiveCredit;
            public float GiveStar;
            public string GiveProfessor;
        }
        [SerializeField] private TextMeshProUGUI className;
        [SerializeField] private TextMeshProUGUI Schedule;
        [SerializeField] private TextMeshProUGUI Popularity;
        [SerializeField] private TextMeshProUGUI Professor;
        [SerializeField] private Image Star;
        [SerializeField] private int Credit;
        [SerializeField] private int Number;
        [SerializeField] private GameObject RegisterButton;
        [SerializeField] private GameObject NotRegisterButton;
        [SerializeField] private GameObject ClosedRegisterButton;
        [SerializeField] private GameObject RegisterComplete;

        UIGiveData m_GiveData = new UIGiveData();

        private int realtime = 0;
        private bool realtimefirst = false;
        public override void UpdateContent(UICellSampleData itemData)
        {
            Number = itemData.Number;
            Credit = itemData.Credit;
            className.text = itemData.ClassName_KR;
            int timea, timeb, timec, timed;
            if (itemData.Schedule_Date.Length < 5)
            {
                timea = 1000 + (int.Parse(itemData.Schedule_Date.Substring(0, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(0, 2)) - 1) % 2 * 30;
                timeb = 1000 + (int.Parse(itemData.Schedule_Date.Substring(2, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(2, 2)) - 1) % 2 * 30;
                Schedule.text = itemData.Schedule_Day.Substring(0, 1) + " " + timea + "-" + timeb;
                m_GiveData.GiveSchedule = itemData.Schedule_Day.Substring(0, 1) + " " + timea + "-" + timeb;
            }
            else
            {
                timea = 1000 + (int.Parse(itemData.Schedule_Date.Substring(0, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(0, 2)) - 1) % 2 * 30;
                timeb = 1000 + (int.Parse(itemData.Schedule_Date.Substring(2, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(2, 2)) - 1) % 2 * 30;
                timec = 1000 + (int.Parse(itemData.Schedule_Date.Substring(4, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(4, 2)) - 1) % 2 * 30;
                timed = 1000 + (int.Parse(itemData.Schedule_Date.Substring(6, 2)) - 1) / 2 * 100 + (int.Parse(itemData.Schedule_Date.Substring(6, 2)) - 1) % 2 * 30;
                Schedule.text = itemData.Schedule_Day.Substring(0, 1) + " " + timea + "-" + timeb + ", " + itemData.Schedule_Day.Substring(1, 1) + " " + timec + "-" + timed;
                m_GiveData.GiveSchedule = itemData.Schedule_Day.Substring(0, 1) + " " + timea + "-" + timeb + ", " + itemData.Schedule_Day.Substring(1, 1) + " " + timec + "-" + timed;
            }

            Popularity.text = "�α⵵     " + itemData.Popularity.ToString();
            Professor.text = itemData.Professor;
            CheckStarImage(itemData.Star);

            if (RealSugangSystem.instance.TimerFinished)
            {
                if (!realtimefirst)
                {
                    realtimefirst = true;
                    StartRealTIme();
                }
                if (SugangBasketManager.instance.SubjectManager.Count == 0)
                {
                    RegisterButton.SetActive(true);
                    NotRegisterButton.SetActive(false);
                    StartCoroutine(SubjectTimer(itemData));
                }
                else
                {
                    for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
                    {
                        if (itemData.ClassName_KR == SugangBasketManager.instance.SubjectManager[i].ClassName_KR)
                        {
                            RegisterButton.SetActive(false);
                            NotRegisterButton.SetActive(false);
                            RegisterComplete.SetActive(true);
                            ClosedRegisterButton.SetActive(false);
                            break;
                        }
                        else
                        {
                            RegisterButton.SetActive(true);
                            NotRegisterButton.SetActive(false);
                            RegisterComplete.SetActive(false);
                            StartCoroutine(SubjectTimer(itemData));


                        }
                    }

                }
                float popular = itemData.Popularity;
                float time = 20f - popular / 100f * 19f;
                time = time - realtime;
                if (time > 0)
                {
                    ClosedRegisterButton.SetActive(false);
                }
            }
            else if (!RealSugangSystem.instance.RealSugangStarted)
            {
                if (SugangBasketManager.instance.SubjectManager.Count == 0)
                {
                    RegisterButton.SetActive(true);
                    NotRegisterButton.SetActive(false);
                }
                else
                {
                    for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
                    {
                        if (itemData.ClassName_KR == SugangBasketManager.instance.SubjectManager[i].ClassName_KR)
                        {
                            RegisterButton.SetActive(false);
                            NotRegisterButton.SetActive(true);
                            break;
                        }
                        else
                        {
                            RegisterButton.SetActive(true);
                            NotRegisterButton.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                if (SugangBasketManager.instance.SubjectManager.Count == 0)
                {
                    RegisterButton.SetActive(false);
                    NotRegisterButton.SetActive(true);
                }
                else
                {
                    for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
                    {
                        if (itemData.ClassName_KR == SugangBasketManager.instance.SubjectManager[i].ClassName_KR)
                        {
                            RegisterButton.SetActive(false);
                            NotRegisterButton.SetActive(false);
                            RegisterComplete.SetActive(true);

                            break;
                        }
                        else
                        {
                            RegisterButton.SetActive(false);
                            NotRegisterButton.SetActive(true);
                            RegisterComplete.SetActive(false);
                        }
                    }
                }
            }

            m_GiveData.GiveNumber = itemData.Number;
            m_GiveData.GiveClassMGE = itemData.ClassMGE;
            m_GiveData.GiveClassName_KR = itemData.ClassName_KR;
            m_GiveData.GiveClassName_EN = itemData.ClassName_EN;


            m_GiveData.GivePopularity = itemData.Popularity;
            m_GiveData.GiveCredit = itemData.Credit;
            m_GiveData.GiveStar = itemData.Star;
            m_GiveData.GiveProfessor = itemData.Professor;


        }
        private void StartRealTIme()
        {
            if(realtime<30)
                StartCoroutine(GetRealTIme());
            else
            {
                
            }
        }
        IEnumerator GetRealTIme()
        {
            yield return new WaitForSecondsRealtime(1f);
            {
                
                realtime++;
                StartRealTIme();
            }
        }
        IEnumerator SubjectTimer(UICellSampleData data)
        {
            float popular = data.Popularity;
            float time = 20f- popular / 100f * 19f;
            time = time - realtime;
            if (time < 0)
                time = 0.01f;
            yield return new WaitForSecondsRealtime(time);
            {
                if (RegisterComplete.activeSelf == true || ClosedRegisterButton.activeSelf == true)
                {
                    ;
                }
                else
                {
                    RegisterButton.SetActive(false);
                    ClosedRegisterButton.SetActive(true);
                
                }
                    
            }
        }
        public void onClickButton()
        {
                
            SugangBasketManager.instance.AddManager(m_GiveData);

        }
        private void CheckStarImage(float StarNum)
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

}