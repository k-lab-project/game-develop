using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI;
using System.Text; // ���ڵ��� ����ϱ� ���� �߰�
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
        //������ ��Ʈ
        alarmText.text = "�غ��߿� �ֽ��ϴ�!\n�������� ������~";
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
            alarmText.text = "���̵� ��ĭ����\n�� �� ����!";
            alarmCanvas.SetActive(true);
            return;

        }
        if (loginPasswordValue.text.Length == 0)
        {
            alarmText.text = "��й�ȣ�� ��ĭ����\n�� �� ����!";
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
            alarmText.text = "���̵� ��ĭ����\n�� �� ����!";
            alarmCanvas.SetActive(true);
            return;

        }
        if (registerPasswordValue.text.Length == 0)
        {
            alarmText.text = "��й�ȣ�� ��ĭ����\n�� �� ����!";
            alarmCanvas.SetActive(true);
            return;
        }
        if (registerEmailValue.text.Length == 0)
        {
            alarmText.text = "�̸����� ��ĭ����\n�� �� ����!";
            alarmCanvas.SetActive(true);
            return;
        }
        StartCoroutine(determineDuplicate());
    }
    public void clickGoToSugangBasket()
    {
        if (characterNickName.text.Length == 0)
        {
            alarmText.text = "ĳ���� �̸��� ��ĭ����\n�� �� ����!";
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

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // �ٵ� JSON ���ڿ��� ����Ʈ �迭�� ��ȯ

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
                if (responseData.Contains("����"))
                {
                    StartCoroutine(registerCommunicate());
                }
                else
                {
                    alarmText.text = "�̹� �����ϴ� ����ڰ� �־�!";
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

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // �ٵ� JSON ���ڿ��� ����Ʈ �迭�� ��ȯ

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

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString); // �ٵ� JSON ���ڿ��� ����Ʈ �迭�� ��ȯ

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
                if (responseData.Contains("����"))
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
        // ���� ǥ������ ����Ͽ� ���ڿ����� ���ڸ� ã�Ƴ��ϴ�.
        MatchCollection matches = Regex.Matches(input, @"\d+");

        // ã�Ƴ� ���ڸ� int �迭�� ��ȯ�մϴ�.
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
                // �������� �޾ƿ� ������
                string responseData = www.downloadHandler.text;
                //Debug.Log("Received data: " + responseData);

                // �����͸� �Ľ��ϰ� ����
                ParseAndSaveData(responseData);

                // CSV ���Ϸ� ����
                SaveToCSV("InternetSubject.csv");
            }
        }
    }

    void ParseAndSaveData(string rawData)
    {
        // ���ȣ�� �����ϰ� �� ������ ���ڵ带 �ٹٲ����� �и�
        string[] records = rawData.Trim('[', ']').Split(new[] { "],[" }, StringSplitOptions.None);

        foreach (var record in records)
        {
            // �� ���ڵ带 ��ǥ�� �и��Ͽ� ����
            string[] fields = record.Split(',');

            // �ʿ��� �����͸� �����Ͽ� ����
            string parsedRecord = $"{fields[0]},{fields[1]},{fields[2]},{fields[3]},{fields[4]},{fields[5]},{fields[6]},{fields[7]},{fields[8]},{fields[9]}";
            parsedRecord = parsedRecord.Replace("\"", "");
            parsedData.Add(parsedRecord);
        }


    }

    void SaveToCSV(string fileName)
    {
        // ������ ������ ���
        string filePath = Path.Combine(Application.dataPath, "Resources", fileName);

        // ���Ͽ� ������ ����
        File.WriteAllLines(filePath, parsedData.ToArray());

        //Debug.Log($"Data saved to: {filePath}");
    }
}

