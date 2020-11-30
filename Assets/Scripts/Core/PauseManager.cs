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
        if (btnResume != null)
            btnResume.onClick.AddListener(OnResumePressed);
        if (btnVideo != null)
            btnVideo.onClick.AddListener(OnVideoPressed);
        if (btnSound != null)
            btnSound.onClick.AddListener(OnAudioPressed);
        btnRestart.onClick.AddListener(OnRestartPressed);
        btnTown.onClick.AddListener(OnTownPressed);
        btnMenu.onClick.AddListener(OnMainMenuPressed);
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
        Time.timeScale = 1;
        //StartCoroutine(GameObject.Find("CanvasTransition").GetComponent<CanvasTransitionScript>().FadeIn(gameObject));
    }

    private void OnTownPressed()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadVillage());
    }

    private void OnMainMenuPressed()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadVillage()
    {
        animator.SetTrigger("Start");

        GetComponent<CanvasGroup>().alpha = 0f;
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private IEnumerator LoadMainMenu()
    {
        animator.SetTrigger("Start");

        GetComponent<CanvasGroup>().alpha = 0f;
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainMenuScene");
    }
}
