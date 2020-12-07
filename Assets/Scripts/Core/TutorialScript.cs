using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        combustibleScript = spawnerCombu.GetComponent<CombustibleScript>();
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

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spell"))
            print("spell");
        interacted = true;
        Debug.Log("Helllllo");
    }*/

    public void TriggerEnnemy()
    {
        if(indexDialogue == 0)
        {
            indexDialogue = 1;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
            spawnerConso.SetActive(true);
        } 
    }

    public void TriggerConsumable()
    {
        if (indexDialogue == 1)
        {
            indexDialogue = 2;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
            spawnerEquip.SetActive(true);
        }
    }

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
    public void TriggerCombustible()
    {
        if (indexDialogue == 3)
        {
            indexDialogue = 4;
            DialogueManagerScript.MyInstance.StartDialogue(dialogue[indexDialogue]);
        }
    }
}
