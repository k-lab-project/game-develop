using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealSugangSystem : MonoBehaviour
{
    public static RealSugangSystem instance;
    public bool RealSugangStarted = false;
    public bool TimerFinished = false;
    
    private void Awake()
    {
        instance = this;
    }
    public void StartFinalStatus()
    {
        AddClassButton.instance.StartFinalStatus();
        ScoreManager.instance.UpdateChange();
    }
}
