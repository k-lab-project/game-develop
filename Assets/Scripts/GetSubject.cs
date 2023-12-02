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
