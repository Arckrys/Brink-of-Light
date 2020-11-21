using System;
using System.Collections;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    CanvasTransitionScript canvasTransition;

    [SerializeField] private GameObject transitionSpawn;

    public void UpdatePlayerPosition()
    {
        PlayerScript.MyInstance.transform.position = transitionSpawn.transform.position;
    }

    private void Start()
    {
        canvasTransition = GameObject.Find("CanvasTransition").GetComponent<CanvasTransitionScript>();

        //StartCoroutine(canvasTransition.FadeOut());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(canvasTransition.FadeIn(this.gameObject));
    }
}
