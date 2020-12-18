using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class StatField : StatManager
{
    [SerializeField] private Text statValue;
    
    protected override void UpdateUIStat()
    {
        if (statValue)
        {
            // Update stat text
            statValue.text = MyCurrentValue.ToString(CultureInfo.InvariantCulture);
        }
        
        base.UpdateUIStat();
    }
}
