using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitEndManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(LoadEndingScene());
    }
    
    private IEnumerator LoadEndingScene()
    {
        //loadAnimator.SetTrigger("Start");
    
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene("EndingScene");
    }
}
