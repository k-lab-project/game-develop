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

public class ButtonManager_start : MonoBehaviour {
    public static ButtonManager_start instance;
    [System.Serializable]
    public class LoginData
    {
        public string login_id;
        public string login_password;
    }
    [SerializeField] private GameObject tabToStart;
    [SerializeField] private GameObject loginTabCanvas;
    [SerializeField] private GameObject alarmCanvas;
    [SerializeField] private TextMeshProUGUI alarmText;
    [SerializeField] private TMP_InputField loginIdValue;
    [SerializeField] private TMP_InputField loginPasswordValue;
    [SerializeField] private TMP_InputField registerIdValue;
    [SerializeField] private TMP_InputField registerPasswordValue;
    [SerializeField] private TMP_InputField registerEmailValue;
    private string url = "http://3.35.187.239:8090";
    public int characterAble;
    public int userId;
    public string characterName;
    public string characterGender;

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
        StartCoroutine(loginCommunicate());
    }
    public void clickCloseAlarm()
    {
        alarmCanvas.SetActive(false);
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
                Debug.LogError("Error: " + www.error);
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
}

