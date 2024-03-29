﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool inDialogueTransition;
    
    // Start is called before the first frame update
    private void Start()
    {
        indexDialogue = 0;

        inDialogueTransition = false;
        
        StartCoroutine(FadeInDialogue());
    }

    // Update is called once per frame
    private void Update()
    {
        // Switch between dialogues and scenes
        if (!inDialogueTransition && DialogueManagerScript.MyInstance.DialogueIsOver) //  && Input.GetKeyDown(KeyCode.E)
        {
            indexDialogue += 1;
            
            inDialogueTransition = true;

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
            else
            {
                StartCoroutine(EndOpeningScene());
            }
        }
    }

    // End opening scene and switch to tutorial scene
    private IEnumerator EndOpeningScene()
    {
        loadAnimator.SetTrigger("Start");
    
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("TutorialScene");
    }

    // Change scene to continue history
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

    // Provokes a flash type animation for the arrival of the enemies
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
        
        inDialogueTransition = false;
    }

    // Show dialogues
    private IEnumerator FadeInDialogue()
    {
        yield return new WaitForSeconds(1);
        
        DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);

        while (messageFrame.GetComponent<CanvasGroup>().alpha < 1)
        {
            messageFrame.GetComponent<CanvasGroup>().alpha += 0.1f;
            
            yield return new WaitForSeconds(0.1f);
        }
        
        inDialogueTransition = false;
    }
    
    // Hide dialogues
    private IEnumerator FadeOutDialogue()
    {
        while (messageFrame.GetComponent<CanvasGroup>().alpha > 0)
        {
            messageFrame.GetComponent<CanvasGroup>().alpha -= 0.1f;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
