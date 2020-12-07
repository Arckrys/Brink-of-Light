using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerScript : MonoBehaviour
{
	[SerializeField] private Text nameText;
	[SerializeField] private Text dialogueText;
	[SerializeField] private GameObject panel;

	private static DialogueManagerScript _instance;

	private bool dialogueIsOpen = false;
	private bool sentenceIsOver = false;
	private bool dialogueIsOver = false;

	public bool SentenceIsOver => sentenceIsOver;
	public bool DialogueIsOver => dialogueIsOver;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
		panel.SetActive(false);
	}

    private void Update()
    {
        if (dialogueIsOpen & sentenceIsOver)
        {
			if (Input.GetKeyDown(KeyCode.E))
            {
				DisplayNextSentence();
            }
		}
    }


    public static DialogueManagerScript MyInstance
	{
		get
		{
			if (!_instance)
			{
				_instance = FindObjectOfType<DialogueManagerScript>();
			}

			return _instance;
		}
	}

    public bool DialogueIsOpen { get => dialogueIsOpen; }

    public void StartDialogue(Dialogue dialogue)
	{
		dialogueIsOpen = true;

		dialogueIsOver = false;

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		panel.SetActive(true);
		
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		sentenceIsOver = false;
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(0.02f);
		}
		sentenceIsOver = true;

	}

	public void EndDialogue()
	{
		dialogueIsOpen = false;
		dialogueIsOver = true;
		StopCoroutine(TypeSentence(""));
		panel.SetActive(false);
	}

}
