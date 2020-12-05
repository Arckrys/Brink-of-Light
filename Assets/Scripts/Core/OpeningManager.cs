using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private GameObject messageFrame;
    
    [SerializeField] private List<Dialogue> dialogue;

    [SerializeField] private Animator loadAnimator;

    [SerializeField] private List<GameObject> scene;

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
        if (DialogueManagerScript.MyInstance.SentenceIsOver && Input.GetKeyDown(KeyCode.E))
        {
            indexDialogue += 1;

            if (indexDialogue < dialogue.Count)
            {
                StartCoroutine(FadeOutDialogue());
                StartCoroutine(ChangeScene());
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
