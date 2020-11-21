using System;
using System.Collections;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private float transitionSpeed;

    [SerializeField] private GameObject transitionSpawn;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        var canvasGroup = GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1)
        {
            yield return new WaitForSeconds(transitionSpeed);
            canvasGroup.alpha += 0.1f;
        }

        UpdatePlayerPosition();

        StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        var canvasGroup = GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            yield return new WaitForSeconds(transitionSpeed);
            canvasGroup.alpha -= 0.1f;
        }
    }

    private void UpdatePlayerPosition()
    {
        PlayerScript.MyInstance.transform.position = transitionSpawn.transform.position;
    }
}
