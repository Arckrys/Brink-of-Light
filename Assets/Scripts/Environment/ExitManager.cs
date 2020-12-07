﻿using System;
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

        StartCoroutine(canvasTransition.FadeIn(this.gameObject, false));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        //if this is a door, the player changes room on contact with the door
        if (gameObject.name != "BorderTilemapNextFloor")
            StartCoroutine(canvasTransition.FadeIn(this.gameObject, false));

        isInCollision = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isInCollision = false;
    }
}
