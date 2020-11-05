using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private bool variableStat;

    public float MyMaxValue
    {
        get => maxValue;

        set
        {
            maxValue = value;

            if (!variableStat)
            {
                MyCurrentValue = value;
            }
        }
    }
    
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
    
    private float maxValue;

    private float currentValue;

    public StatManager(float initValue, float topValue, bool variable)
    {
        variableStat = variable;
        
        Initialized(initValue, topValue);
    }
    
    public void Initialized(float initValue, float topValue)
    {
        MyMaxValue = topValue;

        MyCurrentValue = initValue;
    }
    
    protected virtual void UpdateUIStat()
    {
        
    }
}
