
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UI;
using System.Text; // ���ڵ��� ����ϱ� ���� �߰�


public class UIRecycleViewControllerSample : UIRecycleViewController<UICellSampleData>
{
    public static UIRecycleViewControllerSample instance;

    private string csvFilePath = "Assets/Resources/InternetSubject.csv";
    
    public void LoadData()
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
                    
                    
                    if (ArrangeClassButton.instance.SortByMajor == "ALL" || ArrangeClassButton.instance.SortByMajor == data.ClassMGE)
                    {
                        
                        if (ArrangeClassButton.instance.SortByCredit == 1 || ArrangeClassButton.instance.SortByCredit == data.Credit)
                        {
                            if (ArrangeClassButton.instance.SortByRegister == "BASKET")
                            {
                                for (int i = 0; i < SugangBasketManager.instance.FailedSubjectManager.Count; i++)
                                {
                                    if (data.ClassName_KR == SugangBasketManager.instance.FailedSubjectManager[i].ClassName_KR)
                                    {
                                        tableData.Add(data);
                                        break;
                                    }

                                }
                            }
                            else {
                                tableData.Add(data);
                            }

                        }
                    }
                        
                }
            }
        }
        if (ArrangeClassButton.instance.SortByMiddle == "STAR")
        {
            tableData.Sort(CompareByStar);
        }
        else if (ArrangeClassButton.instance.SortByMiddle == "NAME") {
            tableData.Sort((data1, data2) => string.Compare(data1.ClassName_KR, data2.ClassName_KR, StringComparison.Ordinal));
        }
        else if (ArrangeClassButton.instance.SortByMiddle == "POPULARITY")
        {
            tableData.Sort(CompareByPopularity);
        }



        InitializeTableView();
    }
    private int CompareByPopularity(UICellSampleData x, UICellSampleData y)
    {
        return y.Popularity.CompareTo(x.Popularity);
    }
    private int CompareByStar(UICellSampleData x, UICellSampleData y)
    {
        return y.Star.CompareTo(x.Star);
    }
    protected override void Start()
    {
        
        instance = this;
        base.Start();
        LoadData();
    }


}
