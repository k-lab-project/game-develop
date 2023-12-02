using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI;
using System.Text; // 인코딩을 사용하기 위해 추가
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Net;

public class ButtonManager_start : MonoBehaviour {
    public static ButtonManager_start instance;
    [System.Serializable]
    public class LoginData
    {
        public string login_id;
        public string login_password;
    }
    public class RegisterData
    {
        public string login_id;
        public string login_password;
        public string mail;
    }
    [SerializeField] private GameObject tabToStart;
    [SerializeField] private GameObject loginTabCanvas;
    [SerializeField] private GameObject registerTabCanvas;
    [SerializeField] private GameObject characterAbleCanvas;
    [SerializeField] private GameObject characterCreateCanvas;
    [SerializeField] private GameObject alarmCanvas;
    [SerializeField] private TextMeshProUGUI alarmText;
    [SerializeField] private GameObject characterMaleImage;
    [SerializeField] private GameObject characterFemaleImage;
    [SerializeField] private GameObject base_Canvas;
    [SerializeField] private GameObject sugangBasketCanvas;
    [SerializeField] private TMP_InputField loginIdValue;
    [SerializeField] private TMP_InputField loginPasswordValue;
    [SerializeField] private TMP_InputField registerIdValue;
    [SerializeField] private TMP_InputField registerPasswordValue;
    [SerializeField] private TMP_InputField registerEmailValue;
    [SerializeField] private TMP_InputField characterNickName;
    private string url = "http://3.35.187.239:8090";
    public int characterAble;
    public int userId;
    public string characterName;
    public int characterGender=0;
    private List<string> parsedData = new List<string>();

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (tabToStart.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tabToStart.SetActive(false);
                loginTabCanvas.SetActive(true);
            }
        }
    }

    public void clickContinueButton()
    {
        //성록이 파트
        alarmText.text = "준비중에 있습니다!\n다음번에 만나요~";
        alarmCanvas.SetActive(true);
    }
    public void clickDeleteButton()
    {
        StartCoroutine(deleteCharacter());
    }
    public void clickLoginButton()
    {
        if (loginIdValue.text.Length == 0)
        {
            alarmText.text = "아이디를 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;

        }
        if (loginPasswordValue.text.Length == 0)
        {
            alarmText.text = "비밀번호를 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;
        }
        StartCoroutine(loginCommunicate());
    }
    public void clickLeftCharacter()
    {
        if (characterGender - 1 < 0)
        {
            return;
        }
        else
        {
            characterGender -= 1;
            characterMaleImage.SetActive(false);
            characterFemaleImage.SetActive(true);
        }
    }
    public void clickRightCharacter()
    {
        if (characterGender + 1 > 1)
        {
            return;
        }
        else
        {
            characterGender += 1;
            characterMaleImage.SetActive(true);
            characterFemaleImage.SetActive(false);
        }
    }
    public void clickGoToRegisterTab()
    {
        loginIdValue.text = "";
        loginPasswordValue.text = "";
        loginTabCanvas.SetActive(false);
        registerTabCanvas.SetActive(true);
    }
    public void clickGoToLoginTab()
    {
        registerIdValue.text = "";
        registerPasswordValue.text = "";
        registerEmailValue.text = "";
        loginTabCanvas.SetActive(true);
        registerTabCanvas.SetActive(false);
    }
    public void clickRegisterButton()
    {
        if (registerIdValue.text.Length == 0)
        {
            alarmText.text = "아이디를 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;

        }
        if (registerPasswordValue.text.Length == 0)
        {
            alarmText.text = "비밀번호를 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;
        }
        if (registerEmailValue.text.Length == 0)
        {
            alarmText.text = "이메일을 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;
        }
        StartCoroutine(determineDuplicate());
    }
    public void clickGoToSugangBasket()
    {
        if (characterNickName.text.Length == 0)
        {
            alarmText.text = "캐릭터 이름을 빈칸으로\n할 수 없어!";
            alarmCanvas.SetActive(true);
            return;
        }
        StartCoroutine(GetDataFromServer());
        base_Canvas.SetActive(false);
        sugangBasketCanvas.SetActive(true);
        characterName = characterNickName.text;
        SoundManager.instance.clickSugangBasket();
    }
    public void clickCloseAlarm()
    {
        alarmCanvas.SetActive(false);
    }
    private IEnumerator deleteCharacter()
    {
        using (UnityWebRequest www = new UnityWebRequest(url + userId, "DELETE"))
        {

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                alarmText.text = "Error: " + www.error;
                alarmCanvas.SetActive(true);
            }
            else
            {
                characterAbleCanvas.SetActive(false);
                characterCreateCanvas.SetActive(true);
            }
        }
    }
    private IEnumerator determineDuplicate()
    {
        LoginData loginData = new LoginData
        {
            login_id = registerIdValue.text,
            login_password = registerPasswordValue.text
        };

        string jsonString = JsonUtility.ToJson(loginData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // 바디에 JSON 문자열을 바이트 배열로 변환

        using (UnityWebRequest www = new UnityWebRequest(url + "/check-user", "POST"))
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
                alarmText.text = "Error: " + www.error;
                alarmCanvas.SetActive(true);
            }
            else
            {

                string responseData = www.downloadHandler.text;
                if (responseData.Contains("없는"))
                {
                    StartCoroutine(registerCommunicate());
                }
                else
                {
                    alarmText.text = "이미 존재하는 사용자가 있어!";
                    alarmCanvas.SetActive(true);
                }
            }
        }
    }
    private IEnumerator registerCommunicate()
    {
        RegisterData registerData = new RegisterData
        {
            login_id = registerIdValue.text,
            login_password = registerPasswordValue.text,
            mail = registerEmailValue.text
        };

        string jsonString = JsonUtility.ToJson(registerData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // 바디에 JSON 문자열을 바이트 배열로 변환

        using (UnityWebRequest www = new UnityWebRequest(url + "/create-user", "POST"))
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
                
                alarmText.text = "Error: " + www.error;
                alarmCanvas.SetActive(true);
            }
            else
            {
                registerIdValue.text = "";
                registerPasswordValue.text = "";
                registerEmailValue.text = "";
                loginIdValue.text = "";
                loginPasswordValue.text = "";
                registerTabCanvas.SetActive(false);
                loginTabCanvas.SetActive(true);
            }
        }
    }
    private IEnumerator loginCommunicate()
    {
        LoginData loginData = new LoginData
        {
            login_id = loginIdValue.text,
            login_password = loginPasswordValue.text
        };

        string jsonString = JsonUtility.ToJson(loginData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // 바디에 JSON 문자열을 바이트 배열로 변환

        using (UnityWebRequest www = new UnityWebRequest(url + "/check-user", "POST"))
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
                alarmText.text = "Error: " + www.error;
                alarmCanvas.SetActive(true);
            }
            else
            {
                
                string responseData = www.downloadHandler.text;
                if (responseData.Contains("없는"))
                {
                    characterAble = -1;
                }
                else
                {
                    Debug.Log(responseData);
                    int[] extractedIntegers = ExtractIntegers(responseData);
                    characterAble = extractedIntegers[0];
                    userId = extractedIntegers[1];
                    if (characterAble == 0)
                    {
                        loginTabCanvas.SetActive(false);
                        characterCreateCanvas.SetActive(true);

                    }
                    else
                    {
                        loginTabCanvas.SetActive(false);
                        characterAbleCanvas.SetActive(true);
                    }
                }
            }
        }
    }
    int[] ExtractIntegers(string input)
    {
        // 정규 표현식을 사용하여 문자열에서 숫자를 찾아냅니다.
        MatchCollection matches = Regex.Matches(input, @"\d+");

        // 찾아낸 숫자를 int 배열로 변환합니다.
        int[] integers = new int[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            integers[i] = Int32.Parse(matches[i].Value);
        }

        return integers;
    }
    IEnumerator GetDataFromServer()
    {
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // 서버에서 받아온 데이터
                string responseData = www.downloadHandler.text;
                //Debug.Log("Received data: " + responseData);

                // 데이터를 파싱하고 저장
                ParseAndSaveData(responseData);

                // CSV 파일로 저장
                SaveToCSV("InternetSubject.csv");
            }
        }
    }

    void ParseAndSaveData(string rawData)
    {
        // 대괄호를 제거하고 각 데이터 레코드를 줄바꿈으로 분리
        string[] records = rawData.Trim('[', ']').Split(new[] { "],[" }, StringSplitOptions.None);

        foreach (var record in records)
        {
            // 각 레코드를 쉼표로 분리하여 저장
            string[] fields = record.Split(',');

            // 필요한 데이터를 추출하여 저장
            string parsedRecord = $"{fields[0]},{fields[1]},{fields[2]},{fields[3]},{fields[4]},{fields[5]},{fields[6]},{fields[7]},{fields[8]},{fields[9]}";
            parsedRecord = parsedRecord.Replace("\"", "");
            parsedData.Add(parsedRecord);
        }


    }

    void SaveToCSV(string fileName)
    {
        // 저장할 파일의 경로
        string filePath = Path.Combine(Application.dataPath, "Resources", fileName);

        // 파일에 데이터 쓰기
        File.WriteAllLines(filePath, parsedData.ToArray());

        //Debug.Log($"Data saved to: {filePath}");
    }
}

