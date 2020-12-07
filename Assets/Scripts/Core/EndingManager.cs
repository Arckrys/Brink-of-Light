using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject messageFrame;
    
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private Animator loadAnimator;

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

            if (indexDialogue == dialogue.sentences.Length)
            {
                StartCoroutine(EndEndingScene());
            }
        }
    }
    
    private IEnumerator EndEndingScene()
    {
        loadAnimator.SetTrigger("Start");
    
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("VillageScene");
    }
    
    private IEnumerator FadeInDialogue()
    {
        yield return new WaitForSeconds(1);
        
        DialogueManagerScript.MyInstance.StartDialogue(dialogue);

        while (messageFrame.GetComponent<CanvasGroup>().alpha < 1)
        {
            messageFrame.GetComponent<CanvasGroup>().alpha += 0.1f;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
