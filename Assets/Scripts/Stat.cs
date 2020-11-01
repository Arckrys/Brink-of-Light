﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField] private Text statValue;
    
    [SerializeField] private Boolean filledImage;

    public float MyMaxValue { get; set; }

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            if (statValue)
            {
                statValue.text = currentValue.ToString();
            }
            
            Image content = GetComponent<Image>();

            if (content != null && filledImage && content.fillAmount != currentValue / MyMaxValue)
            {
                content.fillAmount = currentValue / MyMaxValue;
            }
        }
    }

    private float currentValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialized(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;

        MyCurrentValue = currentValue;
    }
}
