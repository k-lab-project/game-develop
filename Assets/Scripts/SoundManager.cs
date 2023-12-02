using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource clickAudio;
    public AudioSource tombAudio;
    public AudioSource goUpDown;
    public AudioSource backgroundAudio;
    public AudioClip sugangBasket;
    public AudioClip realSugang;
    public AudioClip goDown;
    public AudioClip goUp;
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickAudio.Play();  
        }
    }
    public void clickSugangBasket()
    {
        backgroundAudio.clip = sugangBasket;
        backgroundAudio.Play();
    }
    public void clickRealSugang()
    {
        backgroundAudio.clip = realSugang;
        backgroundAudio.Play();
    }
    public void scoreGoDown()
    {
        goUpDown.clip = goDown;
        goUpDown.Play();
    }
    public void scoreGoUp()
    {
        goUpDown.clip = goUp;
        goUpDown.Play();
    }
    public void tombCome()
    {
        tombAudio.Play();
    }
}
