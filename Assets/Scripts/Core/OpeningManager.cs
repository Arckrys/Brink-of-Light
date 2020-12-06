﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private GameObject messageFrame;
    
    [SerializeField] private List<Dialogue> dialogue;

    [SerializeField] private Animator loadAnimator;
    
    [SerializeField] private Animator flashAnimator;

    [SerializeField] private List<GameObject> scene;

    [SerializeField] private Image ImageLoader;

    private int indexDialogue;
    
    // Start is called before the first frame update
    private void Start()
    {
        indexDialogue = 0;
        
        StartCoroutine(FadeInDialogue());
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO : Check in transition
        if (DialogueManagerScript.MyInstance.SentenceIsOver && Input.GetKeyDown(KeyCode.E))
        {
            indexDialogue += 1;

            print(indexDialogue);

            if (indexDialogue < dialogue.Count)
            {
                if (ImageLoader.color.Equals(Color.white))
                {
                    ImageLoader.color = Color.black;
                }

                if (indexDialogue == 4)
                {
                    StartCoroutine(FlashScene());
                }
                else
                {
                    StartCoroutine(FadeOutDialogue());
                    StartCoroutine(ChangeScene());
                }
            }
        }
    }

    private IEnumerator ChangeScene()
    {
        loadAnimator.SetTrigger("Start");
    
        yield return new WaitForSeconds(1);

        if (indexDialogue < scene.Count)
        {
            scene[indexDialogue - 1].SetActive(false);
            scene[indexDialogue].SetActive(true);
        }

        loadAnimator.SetTrigger("End");
        
        StartCoroutine(FadeInDialogue());
    }

    private IEnumerator FlashScene()
    {
        flashAnimator.SetTrigger("Start");
        
        yield return new WaitForSeconds(0.4f);
        
        if (indexDialogue < scene.Count)
        {
            scene[indexDialogue - 1].SetActive(false);
            scene[indexDialogue].SetActive(true);
        }
        
        DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
    }

    private IEnumerator FadeInDialogue()
    {
        yield return new WaitForSeconds(1);
        
        DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);

        while (messageFrame.GetComponent<CanvasGroup>().alpha < 1)
        {
            messageFrame.GetComponent<CanvasGroup>().alpha += 0.1f;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private IEnumerator FadeOutDialogue()
    {
        while (messageFrame.GetComponent<CanvasGroup>().alpha > 0)
        {
            messageFrame.GetComponent<CanvasGroup>().alpha -= 0.1f;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
