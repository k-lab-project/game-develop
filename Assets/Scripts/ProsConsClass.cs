using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProsConsClass : MonoBehaviour
{
    public static ProsConsClass instance;
    public TextMeshProUGUI text;
    private void Awake()
    {
        instance = this;
    }
}
