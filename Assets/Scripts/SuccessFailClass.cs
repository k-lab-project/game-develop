using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SuccessFailClass : MonoBehaviour
{
    public static SuccessFailClass instance;
    public TextMeshProUGUI text;
    private void Awake()
    {
        instance = this;
    }
}
