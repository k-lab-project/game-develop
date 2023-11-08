using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeClass : MonoBehaviour
{
    public static ChangeClass instance;

    public TextMeshProUGUI SubjectName;
    public TextMeshProUGUI Popularity;
    public Image Star;
    public Image ColorChange;

    private void Awake()
    {
        instance = this;   
    }
}
