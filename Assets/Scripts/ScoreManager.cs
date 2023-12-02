using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private TextMeshProUGUI CurrentCredit;
    [SerializeField] private TextMeshProUGUI FinalCredit;
    [SerializeField] private TextMeshProUGUI AdmissionRisk;
    [SerializeField] private TextMeshProUGUI Cons;
    [SerializeField] private TextMeshProUGUI Pros;
    [SerializeField] private TextMeshProUGUI FinalCons;
    [SerializeField] private TextMeshProUGUI FinalPros;

    [SerializeField] private TextMeshProUGUI TotalScore;
    [SerializeField] private TextMeshProUGUI FinalTotalScore;

    [SerializeField] private GameObject createCharacterCanvas;
    [SerializeField] private GameObject Content;
    [SerializeField] private GameObject Pros_Prefab;
    [SerializeField] private GameObject Cons_Prefab;
    [SerializeField] private GameObject Diagnoal;
    [SerializeField] private Transform ConsProsTransform;
    [SerializeField] private Transform DiagnoalTransform;
    [System.Serializable]
    public class UserData
    {
        public int userId;
        public int semester;
        public string nickName;
        public string gender;
        public float memorization;
        public float concentration;
        public float patience;
        public float creativity;
        public float metacognition;
        public float understanding;
        public int[] subjectIds;
    }
    public int creditcnt;
    public int RiskPopularity;
    private float cons;
    private float pros;
    private int Contentcnt;
    public float totalscore;
    private string url = "http://3.35.187.239:8090";
    private void Awake()
    {
        instance = this;
    }
    public void UpdateChange()
    {
        Contentcnt = 0;

        TurnToFalse();
        UpdateCurrentCredit();
        UpdateConsPros();
        UpdateAdmissionRisk();
        UpdateTotalScore();

        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(Content.GetComponent<RectTransform>().sizeDelta.x, 24 * Contentcnt);

    }

    public void clickEndSugang()
    {
        if (creditcnt<12||creditcnt>21)
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif

        }
        else
        {
            StartCoroutine(goWeekScene());
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        //Scend받기
        }
    }
    private IEnumerator goWeekScene()
    {
        UserData userData = new UserData
        {
            userId = ButtonManager_start.instance.userId,
            semester = 1,
            nickName = ButtonManager_start.instance.characterName,
            gender = ButtonManager_start.instance.characterGender,
            memorization = totalscore / 6,
            concentration = totalscore / 6,
            patience = totalscore / 6,
            creativity = totalscore / 6,
            metacognition = totalscore / 6,
            understanding = totalscore / 6,
            subjectIds = new int[SugangBasketManager.instance.SubjectManager.Count]
        };
        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            userData.subjectIds[i] = SugangBasketManager.instance.SubjectManager[i].Number;
        }

        string jsonString = JsonUtility.ToJson(userData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(url + "/create-character", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string responseData = www.downloadHandler.text;
                

            }
        }
    }
    private void TurnToFalse()
    {
        foreach (Transform child in ConsProsTransform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in DiagnoalTransform)
        {
            Destroy(child.gameObject);
        }
    }
    private void UpdateCurrentCredit()
    {
        creditcnt = 0;
        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            creditcnt += SugangBasketManager.instance.SubjectManager[i].Credit;
        }
        if (creditcnt >= 10)
        {
            CurrentCredit.text = creditcnt.ToString();
            FinalCredit.text = creditcnt.ToString();
        }
        else
        {
            CurrentCredit.text = "0" + creditcnt.ToString();
            FinalCredit.text = "0" + creditcnt.ToString();
        }
        
    }

    private void UpdateConsPros()
    {
        cons = 0;
        pros = 0;
        int lunchtime = 0;
        int[][] scheduleArrays = new int[5][];
        scheduleArrays[0] = new int[20]; // 월요일
        scheduleArrays[1] = new int[20]; // 화요일
        scheduleArrays[2] = new int[20]; // 수요일
        scheduleArrays[3] = new int[20]; // 목요일
        scheduleArrays[4] = new int[20]; // 금요일

        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            if (SugangBasketManager.instance.SubjectManager[i].Schedule.Length < 15)
            {
                string[] days = new string[] {
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(0, 1)
                };

                int[][] dates = new int[][] {
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(2, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(7, 4))
                    }
                };
                for (int k = 0; k < 1; k++)
                {
                    int startDate = (dates[k][0] / 100 - 10) * 2 + (dates[k][0] % 100 != 0 ? 1 : 0);
                    int endDate = (dates[k][1] / 100 - 10) * 2 + (dates[k][1] % 100 != 0 ? 1 : 0);
                    switch (days[k])
                    {
                        case "월":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[0][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "화":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[1][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "수":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[2][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "목":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[3][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "금":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[4][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                    }
                }
            } 
            else
            {
                string[] days = new string[] {
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(0, 1),
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(13, 1)
                };

                int[][] dates = new int[][] {
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(2, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(7, 4))
                    },
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(15, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(20, 4))
                    }
                };
                for (int k = 0; k < 2; k++)
                {
                    int startDate = (dates[k][0] / 100 - 10) * 2 + (dates[k][0] % 100 != 0 ? 1 : 0);
                    int endDate = (dates[k][1] / 100 - 10) * 2 + (dates[k][1] % 100 != 0 ? 1 : 0);
                    switch (days[k])
                    {
                        case "월":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[0][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "화":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[1][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "수":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[2][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "목":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[3][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "금":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[4][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                    }
                }
            }


        }
        for(int i = 0; i < 5; i++)
        {
            float start = 100;
            float end = 0;
            int lunch_cnt = 0;
            int continuouscnt = 0;
            int prevnum = 0;
            int startnum = 0;
            int endnum = 0;
            for (int j = 0; j < 20; j++)
            {
                if (scheduleArrays[i][j] != 0)
                {
                    start = Mathf.Min(start, j);
                    end = Mathf.Max(end, j);
                }
                if ((j >= 2 && j <= 5)&&scheduleArrays[i][j]==0)
                {
                    lunch_cnt++;
                }
                if (scheduleArrays[i][j] != 0)
                {
                    if (prevnum == 0)
                    {
                        startnum = j;
                    }
                    if (scheduleArrays[i][j] != prevnum)
                    {
                        prevnum = scheduleArrays[i][j];
                        continuouscnt++;
                    }
                }
                else
                {
                    if (continuouscnt == 1)
                    {
                        continuouscnt = 0;
                        prevnum = 0;
                    }
                    else if (continuouscnt > 1)
                    {
                        endnum = j;
                        switch (i)
                        {
                            case 0:
                                GameObject Cont_Monday = Instantiate(Cons_Prefab);
                                if (RealSugangSystem.instance.TimerFinished)
                                {
                                    Cont_Monday.transform.SetParent(GameObject.Find("ConsContent").transform, false);
                                    Destroy(Cont_Monday.GetComponent<Image>());
                                    Cont_Monday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                                }
                                else
                                {
                                    Cont_Monday.transform.SetParent(GameObject.Find("Content2").transform, false);
                                }
                                Cont_Monday.GetComponent<ProsConsClass>().text.text = "월요일에 "+continuouscnt+"연강";
                                Contentcnt++;
                                break;
                            case 1:
                                GameObject Cont_Tuesday = Instantiate(Cons_Prefab);
                                if (RealSugangSystem.instance.TimerFinished)
                                {
                                    Cont_Tuesday.transform.SetParent(GameObject.Find("ConsContent").transform, false);
                                    Destroy(Cont_Tuesday.GetComponent<Image>());
                                    Cont_Tuesday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                                }
                                else
                                {
                                    Cont_Tuesday.transform.SetParent(GameObject.Find("Content2").transform, false);
                                }
                                
                                Cont_Tuesday.GetComponent<ProsConsClass>().text.text = "화요일에 " + continuouscnt + "연강";
                                Contentcnt++;
                                break;
                            case 2:
                                GameObject Cont_Wednesday = Instantiate(Cons_Prefab);
                                if (RealSugangSystem.instance.TimerFinished)
                                {
                                    Cont_Wednesday.transform.SetParent(GameObject.Find("ConsContent").transform, false);
                                    Destroy(Cont_Wednesday.GetComponent<Image>());
                                    Cont_Wednesday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                                }
                                else
                                {
                                    Cont_Wednesday.transform.SetParent(GameObject.Find("Content2").transform, false);
                                }
                                
                                Cont_Wednesday.GetComponent<ProsConsClass>().text.text = "수요일에 " + continuouscnt + "연강";
                                Contentcnt++;
                                break;
                            case 3:
                                GameObject Cont_Thursday = Instantiate(Cons_Prefab);
                                if (RealSugangSystem.instance.TimerFinished)
                                {
                                    Cont_Thursday.transform.SetParent(GameObject.Find("ConsContent").transform, false);
                                    Destroy(Cont_Thursday.GetComponent<Image>());
                                    Cont_Thursday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                                }
                                else
                                {
                                    Cont_Thursday.transform.SetParent(GameObject.Find("Content2").transform, false);
                                }
                                
                                Cont_Thursday.GetComponent<ProsConsClass>().text.text = "목요일에 " + continuouscnt + "연강";
                                Contentcnt++;
                                break;
                            case 4:
                                GameObject Cont_Friday = Instantiate(Cons_Prefab);
                                if (RealSugangSystem.instance.TimerFinished)
                                {
                                    Cont_Friday.transform.SetParent(GameObject.Find("ConsContent").transform, false);
                                    Destroy(Cont_Friday.GetComponent<Image>());
                                    Cont_Friday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                                }
                                else
                                {
                                    Cont_Friday.transform.SetParent(GameObject.Find("Content2").transform, false);
                                }
                                
                                Cont_Friday.GetComponent<ProsConsClass>().text.text = "금요일에 " + continuouscnt + "연강";
                                Contentcnt++;
                                break;
                        }
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            ;
                        }
                        else
                        {
                            GameObject Diagnoal_Monday = Instantiate(Diagnoal);
                            Diagnoal_Monday.transform.SetParent(GameObject.Find("Diagnoal_Parent").transform, false);
                            RectTransform rectTransform1 = Diagnoal_Monday.GetComponent<RectTransform>();
                            rectTransform1.anchoredPosition = new Vector3(checkxlocation(i), -(endnum - startnum) * 10 + startnum * 20, 0);
                            Debug.Log(endnum + " " + startnum + " ");
                            rectTransform1.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (endnum - startnum) * 20);

                        }

                        continuouscnt = 0;
                        prevnum = 0;
                        cons -= 10;
                    }
                }
            }
            if (lunch_cnt >= 2)
                lunchtime++;
            if (start == 100)
            {
                switch (i)
                {
                    case 0:
                        
                        GameObject Empty_Monday = Instantiate(Pros_Prefab);
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            Empty_Monday.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                            Destroy(Empty_Monday.GetComponent<Image>());
                            Empty_Monday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                        }
                        else
                        {
                            Empty_Monday.transform.SetParent(GameObject.Find("Content2").transform, false);
                        }
                        
                        Empty_Monday.GetComponent<ProsConsClass>().text.text = "공강(월요일)";
                        Contentcnt++;
                        break;
                    case 1:
                        GameObject Empty_Tuesday = Instantiate(Pros_Prefab);
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            Empty_Tuesday.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                            Destroy(Empty_Tuesday.GetComponent<Image>());
                            Empty_Tuesday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                        }
                        else
                        {
                            Empty_Tuesday.transform.SetParent(GameObject.Find("Content2").transform, false);
                        }
                        
                        Empty_Tuesday.GetComponent<ProsConsClass>().text.text = "공강(화요일)";
                        Contentcnt++;
                        break;
                    case 2:
                        GameObject Empty_Wednesday = Instantiate(Pros_Prefab);
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            Empty_Wednesday.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                            Destroy(Empty_Wednesday.GetComponent<Image>());
                            Empty_Wednesday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                        }
                        else
                        {
                            Empty_Wednesday.transform.SetParent(GameObject.Find("Content2").transform, false);
                        }
                        
                        Empty_Wednesday.GetComponent<ProsConsClass>().text.text = "공강(수요일)";
                        Contentcnt++;
                        break;
                    case 3:
                        GameObject Empty_Thursday = Instantiate(Pros_Prefab);
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            Empty_Thursday.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                            Destroy(Empty_Thursday.GetComponent<Image>());
                            Empty_Thursday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                        }
                        else
                        {
                            Empty_Thursday.transform.SetParent(GameObject.Find("Content2").transform, false);
                        }
                        
                        Empty_Thursday.GetComponent<ProsConsClass>().text.text = "공강(목요일)";
                        Contentcnt++;
                        break;
                    case 4:
                        GameObject Empty_Friday = Instantiate(Pros_Prefab);
                        if (RealSugangSystem.instance.TimerFinished)
                        {
                            Empty_Friday.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                            Destroy(Empty_Friday.GetComponent<Image>());
                            Empty_Friday.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
                        }
                        else
                        {
                            Empty_Friday.transform.SetParent(GameObject.Find("Content2").transform, false);
                        }
                        Empty_Friday.GetComponent<ProsConsClass>().text.text = "공강(금요일)";
                        Contentcnt++;
                        break;
                }
                pros += 20;
            }
            else
            {
                if (4 - (end + 1 - start)/2 > 0)
                {
                    pros += (4 - (end + 1 - start)/2) * 5;
                }
                else
                {
                    
                    cons += (4 - (end + 1 - start)/2) * 5;
                }
            }
        }
        if (lunchtime > 0)
        {
            GameObject LunchTime_Good = Instantiate(Pros_Prefab);
            if (RealSugangSystem.instance.TimerFinished)
            {
                LunchTime_Good.transform.SetParent(GameObject.Find("ProsContent").transform, false);
                Destroy(LunchTime_Good.GetComponent<Image>());
                LunchTime_Good.GetComponent<ProsConsClass>().text.color = new Color32(0, 0, 0, 255);
            }
            else
            {
                LunchTime_Good.transform.SetParent(GameObject.Find("Content2").transform, false);
            }
            
            LunchTime_Good.GetComponent<ProsConsClass>().text.text = "점심시간 확보(" + lunchtime + ")";
            pros += 5 * lunchtime;
            Contentcnt++;
        }
        if (RealSugangSystem.instance.TimerFinished)
        {
            if (cons < 100)
            {
                FinalCons.text ="0"+ (Mathf.CeilToInt(-cons)).ToString();
            }
            else
            {
                FinalCons.text = (Mathf.CeilToInt(-cons)).ToString();
            }
            if (pros < 100)
            {
                FinalPros.text = "0" + (Mathf.CeilToInt(pros)).ToString();
            }
            else
            {
                FinalPros.text = (Mathf.CeilToInt(pros)).ToString();
            }
            
        }
        else
        {
            Cons.text = (Mathf.CeilToInt(-cons)).ToString();
            Pros.text = (Mathf.CeilToInt(pros)).ToString();
        }
    }
    private float checkxlocation(int day)
    {
        //케이스문 쓰기
        float x1 = 0f;
        switch (day)
        {
            case 0:
                x1 = 27f;
                break;
            case 1:
                x1 = 27f + 54f * 1;
                break;
            case 2:
                x1 = 27f + 54f * 2;
                break;
            case 3:
                x1 = 27f + 54f * 3;
                break;
            case 4:
                x1 = 27f + 54f * 4;
                break;
        }
        return x1;
    }
    private void UpdateAdmissionRisk()
    {
        RiskPopularity = 0;
        if (SugangBasketManager.instance.SubjectManager.Count == 0)
        {
            ;
        }
        else
        {
            for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
            {
                RiskPopularity += SugangBasketManager.instance.SubjectManager[i].Popularity;
            }
            RiskPopularity /= SugangBasketManager.instance.SubjectManager.Count;
        }
        AdmissionRisk.text = RiskPopularity.ToString();
    }
    private void UpdateTotalScore()
    {
        totalscore = 0;
        
        float consprosfloat = (cons + pros)/100;
        
        float starsum = 0;
        float correctioncredit = 0;
        for(int i=0;i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            starsum += SugangBasketManager.instance.SubjectManager[i].Star;
        }
        if (creditcnt >= 12)
        {
            correctioncredit=(creditcnt - 12) * 15;
            EndBasketButton.instance.IsCredit12 = true;
        }
        else
        {
            EndBasketButton.instance.IsCredit12 = false;
        }
        EndBasketButton.instance.CheckBasket();
        if (SugangBasketManager.instance.SubjectManager.Count != 0)
            totalscore = (creditcnt * (1 + consprosfloat)+ correctioncredit + 30 + starsum / SugangBasketManager.instance.SubjectManager.Count * 4) * (100 + RiskPopularity) / 100;
        
        TotalScore.text = (Mathf.Ceil(totalscore)).ToString();
        FinalTotalScore.text = (Mathf.Ceil(totalscore)).ToString();
    }
    public void UpdateScore()
    {
        Contentcnt = 0;

        creditcnt = 0;
        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            creditcnt += SugangBasketManager.instance.SubjectManager[i].Credit;
        }
        cons = 0;
        pros = 0;
        int lunchtime = 0;
        int[][] scheduleArrays = new int[5][];
        scheduleArrays[0] = new int[20]; // 월요일
        scheduleArrays[1] = new int[20]; // 화요일
        scheduleArrays[2] = new int[20]; // 수요일
        scheduleArrays[3] = new int[20]; // 목요일
        scheduleArrays[4] = new int[20]; // 금요일

        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            if (SugangBasketManager.instance.SubjectManager[i].Schedule.Length < 15)
            {
                string[] days = new string[] {
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(0, 1)
                };

                int[][] dates = new int[][] {
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(2, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(7, 4))
                    }
                };
                for (int k = 0; k < 1; k++)
                {
                    int startDate = (dates[k][0] / 100 - 10) * 2 + (dates[k][0] % 100 != 0 ? 1 : 0);
                    int endDate = (dates[k][1] / 100 - 10) * 2 + (dates[k][1] % 100 != 0 ? 1 : 0);
                    switch (days[k])
                    {
                        case "월":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[0][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "화":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[1][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "수":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[2][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "목":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[3][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "금":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[4][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                    }
                }
            }
            else
            {
                string[] days = new string[] {
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(0, 1),
                SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(13, 1)
                };

                int[][] dates = new int[][] {
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(2, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(7, 4))
                    },
                    new int[] {
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(15, 4)),
                        int.Parse(SugangBasketManager.instance.SubjectManager[i].Schedule.Substring(20, 4))
                    }
                };
                for (int k = 0; k < 2; k++)
                {
                    int startDate = (dates[k][0] / 100 - 10) * 2 + (dates[k][0] % 100 != 0 ? 1 : 0);
                    int endDate = (dates[k][1] / 100 - 10) * 2 + (dates[k][1] % 100 != 0 ? 1 : 0);
                    switch (days[k])
                    {
                        case "월":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[0][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "화":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[1][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "수":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[2][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "목":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[3][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                        case "금":
                            for (int j = startDate; j < endDate; j++)
                            {
                                scheduleArrays[4][j] = SugangBasketManager.instance.SubjectManager[i].Number;
                            }
                            break;
                    }
                }
            }

        }
        for (int i = 0; i < 5; i++)
        {
            float start = 100;
            float end = 0;
            int lunch_cnt = 0;
            int continuouscnt = 0;
            int prevnum = 0;
            int startnum = 0;
            int endnum = 0;
            for (int j = 0; j < 18; j++)
            {
                if (scheduleArrays[i][j] != 0)
                {
                    start = Mathf.Min(start, j);
                    end = Mathf.Max(end, j);
                }
                if ((j >= 2 && j <= 5) && scheduleArrays[i][j] == 0)
                {
                    lunch_cnt++;
                }
                if (scheduleArrays[i][j] != 0)
                {
                    if (prevnum == 0)
                    {
                        startnum = j;
                    }
                    if (scheduleArrays[i][j] != prevnum)
                    {
                        prevnum = scheduleArrays[i][j];
                        continuouscnt++;
                    }
                }
                else
                {
                    if (continuouscnt == 1)
                    {
                        continuouscnt = 0;
                        prevnum = 0;
                    }
                    else if (continuouscnt > 1)
                    {
                        endnum = j;
                        Contentcnt++;

                        continuouscnt = 0;
                        prevnum = 0;
                        cons -= 10;
                    }
                }
            }
            if (lunch_cnt >= 2)
                lunchtime++;
            if (start == 100)
            {
                Contentcnt++;
                pros += 20;
            }
            else
            {
                if (4 - (end + 1 - start) / 2 > 0)
                {
                    pros += (4 - (end + 1 - start) / 2) * 5;
                }
                else
                {

                    cons += (4 - (end + 1 - start) / 2) * 5;
                }
            }
        }
        if (lunchtime > 0)
        {
            pros += 5 * lunchtime;
            Contentcnt++;
        }
        RiskPopularity = 0;
        if (SugangBasketManager.instance.SubjectManager.Count == 0)
        {
            ;
        }
        else
        {
            for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
            {
                RiskPopularity += SugangBasketManager.instance.SubjectManager[i].Popularity;
            }
            RiskPopularity /= SugangBasketManager.instance.SubjectManager.Count;
        }
        totalscore = 0;

        float consprosfloat = (cons + pros) / 100;

        float starsum = 0;
        float correctioncredit = 0;
        for (int i = 0; i < SugangBasketManager.instance.SubjectManager.Count; i++)
        {
            starsum += SugangBasketManager.instance.SubjectManager[i].Star;
        }
        if (creditcnt >= 12)
        {
            correctioncredit = (creditcnt - 12) * 15;

        }
        else
        {

        }

        if (SugangBasketManager.instance.SubjectManager.Count != 0)
            totalscore = (creditcnt * (1 + consprosfloat) + correctioncredit + 30 + starsum / SugangBasketManager.instance.SubjectManager.Count * 4) * (100 + RiskPopularity) / 100;


    }
}
