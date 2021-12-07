using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float time = 0;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Start()
    {
        slider.value = 0;
        Debug.Log(Math.ValueToPercent(30,60));
    }

    void Update()
    {
        //TODO: need to update Timer code
        time += Time.deltaTime;
        Debug.Log(time);
        slider.value = time / 100;
    }
    
}
