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
    
    public void Initialized(float initValue, float topValue, bool isVariable = true)
    {
        MyMaxValue = topValue;

        MyCurrentValue = initValue;

        variableStat = isVariable;
    }
    
    protected virtual void UpdateUIStat()
    {
        
    }
}
