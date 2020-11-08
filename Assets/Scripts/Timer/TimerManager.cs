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

            timeInSeconds = 0;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        isTimerOn = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTimerOn)
        {
            timeInSeconds += Time.deltaTime;
            
            var minutes = Mathf.FloorToInt(timeInSeconds / 60); 
            var seconds = Mathf.FloorToInt(timeInSeconds % 60);

            timerValue.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
