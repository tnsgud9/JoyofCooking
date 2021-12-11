using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool isRunning = false;
    private float time = 0;
    private Slider slider;

    public Text timeText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Start()
    {
        slider.value = 0;
    }

    void Update()
    {
        if (isRunning)
        {
            //TODO: need to update Timer code
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0f, Constants.TIMELIMIT);
            timeText.text = (Constants.TIMELIMIT - System.Math.Truncate(time)).ToString();
            slider.value = Math.ValueToPercent(time, Constants.TIMELIMIT);
            if (time < Constants.TIMELIMIT)
            {
                TimePause();
                //TODO : make it puzzleManager callback func
            }
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

    public void TimeAdd(float addTime) => time += addTime;
}
