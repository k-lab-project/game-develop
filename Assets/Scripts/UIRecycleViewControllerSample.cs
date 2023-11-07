
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UI;
using System.Text; // ���ڵ��� ����ϱ� ���� �߰�

public class UIRecycleViewControllerSample : UIRecycleViewController<UICellSampleData>
{
    private string csvFilePath = "Assets/Resources/Subject.csv";

    private void LoadData()
    {
        tableData = new List<UICellSampleData>();

        // UTF-8�� ���ڵ��� ������ �б� ���� Encoding ����
        Encoding encoding = Encoding.UTF8;

        // StreamReader�� ���ڵ� ���� ����
        using (StreamReader reader = new StreamReader(csvFilePath, encoding))
        {
            string line;
            bool header = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (header)
                {
                    header = false;
                    continue;
                }
                string[] values = line.Split(',');
                if (values.Length > 7)
                {

                    UICellSampleData data = new UICellSampleData
                    {
                        Number = int.Parse(values[0]),
                        ClassMGE = values[1],
                        ClassName_KR = values[3],
                        ClassName_EN = values[4],
                        Popularity = int.Parse(values[5]),
                        Schedule_Day = values[6],
                        Schedule_Date = values[7],
                        Credit = int.Parse(values[2]),
                        Star = float.Parse(values[8]),
                        Professor = values[9]
                    };
                    tableData.Add(data);
                }
            }
        }

        InitializeTableView();
    }

    protected override void Start()
    {
        base.Start();
        LoadData();
    }


}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace UI
{
    public class UIRecycleViewControllerSample : UIRecycleViewController<UICellSampleData>
    {
        private string csvFilePath= "Assets/Resources/Subject.csv";
        private void LoadData()
        {
            tableData = new List<UICellSampleData>();
            List<Dictionary<string, object>> csvData = CSVReader.Read("Subject");
            if (csvData.Count > 0)
            {
                for(int i = 0; i < csvData.Count; i++)
                {
                    
                    UICellSampleData data = new UICellSampleData
                       {
                            Number = int.Parse(csvData[i]["Number"].ToString()),
                            ClassMGE = csvData[i]["Class"].ToString(),
                            ClassName_KR = csvData[i]["Korea_Name"].ToString(),
                            ClassName_EN = csvData[i]["English_Name"].ToString(),
                            Popularity = int.Parse(csvData[i]["Popularity"].ToString()),
                            Schedule = csvData[i]["Schedule"].ToString(),
                            Credit = int.Parse(csvData[i]["Credit"].ToString()),
                            Star = float.Parse(csvData[i]["Star"].ToString())
                        };
                        tableData.Add(data);
                    }
                }
            InitializeTableView();
        }

        protected override void Start()
        {
            base.Start();
            LoadData();
        }
        public void OnPressCell(UIRecycleviewsample cell)
        {
            Debug.Log("Cell click");
            Debug.Log(tableData[cell.Index].ClassName_KR);
        }
    }

}*/