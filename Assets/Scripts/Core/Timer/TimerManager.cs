using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    private static TimerManager _instance;
    
    public static TimerManager MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<TimerManager>();
            }

            return _instance;
        }
    }
    
    [SerializeField] private Text timerValue;

    private bool isTimerOn;

    private float timeInSeconds;

    public bool MyTimer
    {
        set
        {
            isTimerOn = value;

            timeInSeconds = !value ? 0 : timeInSeconds;
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (!isTimerOn) return;
        
        timeInSeconds += Time.deltaTime;
            
        var minutes = Mathf.FloorToInt(timeInSeconds / 60); 
        var seconds = Mathf.FloorToInt(timeInSeconds % 60);

        timerValue.text = $"{minutes:00}:{seconds:00}";
    }
}
