using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class GetSubject : MonoBehaviour
{

    private string url = "http://3.35.187.239:8090";

   public void getData()
    {
        StartCoroutine(GetDataFromServer());
    }
    private List<string> parsedData = new List<string>();

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
