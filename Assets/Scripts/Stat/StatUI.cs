using System;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : StatManager
{
    private Image content;

    PlayerScript playerScript;

    [SerializeField] private bool scalable;

    [SerializeField] private bool filled;

    [SerializeField] private Text statText;

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

                UpdatePlayerLight(scale);

                transform.localScale = scale;
            }
        }
        
        if (filled && content && Math.Abs(content.fillAmount - MyCurrentValue / MyMaxValue) > tolerance)
        {
            content.fillAmount = MyCurrentValue / MyMaxValue;
        }

        if (statText)
        {
            statText.text = MyCurrentValue + "/" + MyMaxValue;
        }
        
        base.UpdateUIStat();
    }

    protected void UpdatePlayerLight(Vector2 scale)
    {
        if (playerScript == null)
            playerScript = PlayerScript.MyInstance;

        // Outer radius is between 4 and 8
        // Inner radius is between 0.4 and 0.8
        playerScript.SetPlayerLighting(scale.x * 4 + 4);
        

    }
}
