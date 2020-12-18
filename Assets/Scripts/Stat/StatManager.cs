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

            // If this stat is not like life (had variation which need two separated value)
            if (!variableStat)
            {
                MyCurrentValue = value;
            }

            if (MyCurrentValue > 0 && value > 0)
                UpdateUIStat();
        }
    }
    
    public float MyCurrentValue
    {
        get => currentValue;

        set
        {
            // Set properly the stat value by taking care of min and max values
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
