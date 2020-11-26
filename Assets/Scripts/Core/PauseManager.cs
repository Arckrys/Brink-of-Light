using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Button btnResume;
    
    [SerializeField] private Button btnVideo;
    
    [SerializeField] private Button btnSound;
    
    [SerializeField] private Button btnRestart;
    
    [SerializeField] private Button btnTown;
    
    [SerializeField] private Button btnMenu;

    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        btnResume.onClick.AddListener(OnResumePressed);
        btnVideo.onClick.AddListener(OnVideoPressed);
        btnSound.onClick.AddListener(OnAudioPressed);
        btnRestart.onClick.AddListener(OnRestartPressed);
        btnTown.onClick.AddListener(OnTownPressed);
        btnMenu.onClick.AddListener(OnResumePressed);
    }

    private static void OnResumePressed()
    {
        GameManager.MyInstance.EditPauseState(false);
    }

    private static void OnVideoPressed()
    {
        GameManager.MyInstance.SetGraphicMenu(true);
    }
    
    private static void OnAudioPressed()
    {
        GameManager.MyInstance.SetAudioMenu(true);
    }

    private void OnRestartPressed()
    {
        //StartCoroutine(GameObject.Find("CanvasTransition").GetComponent<CanvasTransitionScript>().FadeIn(gameObject));
    }

    private void OnTownPressed()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        animator.SetTrigger("Start");

        GetComponent<CanvasGroup>().alpha = 0f;
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
