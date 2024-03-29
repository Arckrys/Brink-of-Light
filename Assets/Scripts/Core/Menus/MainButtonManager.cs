﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

// Manage buttons animation in main menu
public class MainButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject fire;

    private Coroutine fireCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        HideFire();
    }

    private void HideFire()
    {
        var tempColor = fire.GetComponent<SpriteRenderer>().color;
        tempColor.a = 0;
        fire.GetComponent<SpriteRenderer>().color = tempColor;
        fire.GetComponent<Light2D>().intensity = 0;
    }
    
    // Fade out animation on fire sprite
    private IEnumerator OffFire()
    {
        var tempColor = fire.GetComponent<SpriteRenderer>().color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= 0.1f;
            fire.GetComponent<Light2D>().intensity -= 0.1f;
            fire.GetComponent<SpriteRenderer>().color = tempColor;
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(fireCoroutine);
    }

    // Fade in animation on fire sprite
    private IEnumerator OnFire()
    {
        var tempColor = fire.GetComponent<SpriteRenderer>().color;
        while (tempColor.a < 1f)
        {
            tempColor.a += 0.1f;
            fire.GetComponent<Light2D>().intensity += 0.1f;
            fire.GetComponent<SpriteRenderer>().color = tempColor;
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(fireCoroutine);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MainMenuManager.MyInstance.InMenu || MainMenuManager.MyInstance.InTransition) return;
        
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        
        // Start animation when mouse hover a button
        fireCoroutine = StartCoroutine(OnFire());
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (MainMenuManager.MyInstance.InTransition) return;
        
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        
        // Stop animation when mouse leave a button
        fireCoroutine = StartCoroutine(OffFire());
    }

    public void OffButton()
    {
        HideFire();
        
        gameObject.SetActive(false);
    }
    
    public void OnButton()
    {
        gameObject.SetActive(true);
    }
}
