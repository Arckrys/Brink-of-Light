﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject messageFrame;

    [SerializeField] private List<Dialogue> dialogue;

    [SerializeField] private GameObject spawnerEquip;

    [SerializeField] private GameObject spawnerConso;

    [SerializeField] private GameObject spawnerCombu;

    private int indexDialogue;
    private bool interacted = false;

    private static TutorialScript _instance;
    private CombustibleScript combustibleScript;

    
    private void Start()
    {
        indexDialogue = 0;

        StartCoroutine(FadeInDialogue());
        if (MusicManager.MyInstance != null)
            MusicManager.MyInstance.SetCurrentMusic("village");

        combustibleScript = spawnerCombu.GetComponent<CombustibleScript>();
    }

    private void Update()
    {
        if (indexDialogue == dialogue.Count - 1 && DialogueManagerScript.MyInstance.SentenceIsOver && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(EndTutorialScene());
        }
    }
    
    // Load town scene
    private IEnumerator EndTutorialScene()
    {
        GameObject.Find("Crossfade").GetComponent<Animator>().SetTrigger("Start");
    
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("VillageScene");
    }

    public static TutorialScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<TutorialScript>();
            }

            return _instance;
        }
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

    // When training mob is hit, go to next step
    public void TriggerEnnemy()
    {
        if(indexDialogue == 0)
        {
            indexDialogue = 1;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
            spawnerConso.SetActive(true);
        } 
    }

    // When consumable is triggered, go to next step
    public void TriggerConsumable()
    {
        if (indexDialogue == 1)
        {
            indexDialogue = 2;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
            spawnerEquip.SetActive(true);
        }
    }

    // When equipment is triggered, go to next step
    public void TriggerEquipment()
    {
        if (indexDialogue == 2)
        {
            indexDialogue = 3;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
            spawnerCombu.SetActive(true);
            combustibleScript.SetIsLit(true);

        }
    }
    
    // When combustible is triggered, go to next step
    public void TriggerCombustible()
    {
        if (indexDialogue == 3)
        {
            indexDialogue = 4;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
        }
    }
}
