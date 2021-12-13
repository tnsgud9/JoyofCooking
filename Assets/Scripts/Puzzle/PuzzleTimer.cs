using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour
{
    private float time = Constants.TIMELIMIT;
    private Slider slider;
    private IEnumerator timer;
    public Image catHeadImage;
    public Sprite catNormal;
    public Sprite catHurry;
    public Sprite catOver;
    public Text timeText;
    

    private void Awake()
    {
        timer = TimeCoroutine();
        slider = GetComponent<Slider>();
        //좋은 방법인가? Need to code review
        PuzzleManager.Instance.timer = this;
        slider.maxValue = Constants.TIMELIMIT;
    }

    public void OnEnable()
    {
        time = Constants.TIMELIMIT;
        catHeadImage.sprite = catNormal;
        slider.value = slider.maxValue;

    }

    private void TimeDecrease()
    {
        time -= Time.deltaTime;
        slider.value = time;
        timeText.text = System.Math.Truncate(time).ToString();
    }
    private IEnumerator TimeCoroutine()
    {
        while (Math.ValueToPercent(time,Constants.TIMELIMIT) > 0.3f)
        {
            TimeDecrease();
            yield return null;
        }
        PuzzleManager.Instance.HurryTime();
        catHeadImage.sprite = catHurry;
        while (time >= 0)
        {
            TimeDecrease();
            yield return null;
        }
        catHeadImage.sprite = catOver;
        PuzzleManager.Instance.GameOver();
    }

    public void TimeStart() => StartCoroutine(timer);
    public void TimePause() => StopCoroutine(timer);
    public void TimeAdd(float addTime) => time = (time + addTime) > Constants.TIMELIMIT ? Constants.TIMELIMIT: time + addTime;
}
