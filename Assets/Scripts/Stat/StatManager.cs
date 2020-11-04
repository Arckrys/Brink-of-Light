using UnityEngine;

public class StatManager : MonoBehaviour
{
    public float MyMaxValue { get; set; }
    
    public float MyCurrentValue
    {
        get => currentValue;

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
            
            UpdateUIStat();
        }
    }
    
    private float currentValue;
    
    public void Initialized(float initValue, float maxValue)
    {
        MyMaxValue = maxValue;

        MyCurrentValue = initValue;
    }
    
    protected virtual void UpdateUIStat()
    {
        
    }
}
