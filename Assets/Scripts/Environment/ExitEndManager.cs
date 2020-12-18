using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitEndManager : MonoBehaviour
{
    private Animator loadAnimator;

    private void Start()
    {
        loadAnimator = GameObject.Find("Crossfade").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(LoadEndingScene());
    }
    
    // Load ending scene at the end of the game
    private IEnumerator LoadEndingScene()
    {
        loadAnimator.SetTrigger("Start");
    
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("EndingScene");
    }
}
