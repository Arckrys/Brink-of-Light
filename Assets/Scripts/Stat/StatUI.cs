using System;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : StatManager
{
    private Image content;

    [SerializeField] private bool scalable;

    [SerializeField] private bool filled;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (filled)
        {
            content = GetComponent<Image>();
        }
    }

    protected override void UpdateUIStat()
    {
        const float tolerance = 0.01f;
        
        if (scalable)
        {
            var scale = transform.localScale;

            if (Math.Abs(scale.x - MyCurrentValue / MyMaxValue) > tolerance && Math.Abs(scale.y - MyCurrentValue / MyMaxValue) > tolerance)
            {
                scale.x = MyCurrentValue / MyMaxValue;
                scale.y = MyCurrentValue / MyMaxValue;

                transform.localScale = scale;
            }
        }
        
        if (filled && content && Math.Abs(content.fillAmount - MyCurrentValue / MyMaxValue) > tolerance)
        {
            content.fillAmount = MyCurrentValue / MyMaxValue;
        }
        
        base.UpdateUIStat();
    }
}
