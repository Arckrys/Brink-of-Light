using System;
using System.Collections;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    CanvasTransitionScript canvasTransition;

    [SerializeField] private GameObject transitionSpawn;

    private bool isInCollision;

    private void Start()
    {
        canvasTransition = GameObject.Find("CanvasTransition").GetComponent<CanvasTransitionScript>();

        isInCollision = false;

        //StartCoroutine(canvasTransition.FadeOut());
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || !isInCollision || gameObject.name != "BorderTilemapNextFloor") return;

        PlayerScript.MyInstance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(canvasTransition.FadeIn(this.gameObject, false));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        //if this is a door, the player changes room on contact with the door
        if (gameObject.name != "BorderTilemapNextFloor")
            StartCoroutine(canvasTransition.FadeIn(this.gameObject, false));

        else
        {
            isInCollision = true;
            PlayerScript.MyInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isInCollision = false;
        PlayerScript.MyInstance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
