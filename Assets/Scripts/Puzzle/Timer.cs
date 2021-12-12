using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool isRunning = false;
    private float time = Constants.TIMELIMIT;
    private Slider slider;
    public Image catHeadImage;
    public Sprite catNormal;
    public Sprite catHurry;
    public Sprite catNoTime;
    public Sprite catGameOver;
    public Text timeText;
    

    private void Awake()
    {
        slider = GetComponent<Slider>();
        catHeadImage = GetComponent<Image>();
        
        //좋은 방법인가? Need to code review
        PuzzleManager.Instance.timer = this;
        slider.maxValue = Constants.TIMELIMIT;
    }

    public void OnEnable()
    {
        time = Constants.TIMELIMIT;
        slider.value = slider.maxValue;

    }

    void Update()
    {
        if (isRunning)
        {
            //TODO: need to update Timer code
            time -= Time.deltaTime;
            if (time < 0)
            {
                TimePause();
                PuzzleManager.Instance.GameOver();
                
            }
            slider.value = time;
            timeText.text = System.Math.Truncate(time).ToString();
            
        }
    }

    public void TimeReset()
    {
        slider.value = 0;
        time = 0f;
    }

    public void TimeStart() => isRunning = true;
    public void TimePause() => isRunning = false;

    public void TimeStop()
    {
        isRunning = false;
        TimeReset();
    }

    public void TimeAdd(float addTime) => time = (time + addTime) > Constants.TIMELIMIT ? Constants.TIMELIMIT: time + addTime;
}
