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

    [SerializeField] private Button btnQuit;

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
        if (btnQuit != null)
            btnQuit.onClick.AddListener(OnQuitPressed);
    }

    // Leave game
    private void OnQuitPressed()
    {
        PlayerPrefs.SetInt("Restart", 0);
        Application.Quit();
    }

    // Hide pause menu
    private static void OnResumePressed()
    {
        GameManager.MyInstance.EditPauseState(false);
    }

    // Show video settings
    private static void OnVideoPressed()
    {
        GameManager.MyInstance.SetGraphicMenu(true);
    }

    // Show audio settings
    private static void OnAudioPressed()
    {
        GameManager.MyInstance.SetAudioMenu(true);
    }

    // Restart dungeon
    private void OnRestartPressed()
    {
        StartCoroutine(RestartDungeon());

        Time.timeScale = 1;
    }

    // Go back to town
    private void OnTownPressed()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadVillage());
    }

    // Go to main menu
    private void OnMainMenuPressed()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadMainMenu());
    }

    // Load town scene
    private IEnumerator LoadVillage()
    {
        animator.SetTrigger("Start");
        if (MusicManager.MyInstance != null)
            MusicManager.MyInstance.SetCurrentMusic("village");

        GetComponent<CanvasGroup>().alpha = 0f;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("VillageScene");
    }

    // Leave dungeon and instantiate a new one
    private IEnumerator RestartDungeon()
    {
        animator.SetTrigger("Start");

        GetComponent<CanvasGroup>().alpha = 0f;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.SetInt("Restart", 1);
    }

    // Show main menu
    private IEnumerator LoadMainMenu()
    {
        animator.SetTrigger("Start");

        if (MusicManager.MyInstance != null)
            MusicManager.MyInstance.SetCurrentMusic("village");

        GetComponent<CanvasGroup>().alpha = 0f;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainMenuScene");
    }
}
