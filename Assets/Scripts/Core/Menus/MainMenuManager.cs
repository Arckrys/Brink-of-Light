using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuSettings;
    
    [SerializeField] private GameObject menuAudio;
    
    [SerializeField] private GameObject menuGraphics;
    
    [SerializeField] private Button newGame;
    
    [SerializeField] private Button resumeGame;

    [SerializeField] private GameObject noSaveText;

    [SerializeField] private Button settings;
    
    [SerializeField] private Button quit;
    
    [SerializeField] private Button audio;
    
    [SerializeField] private Button graphics;
    
    [SerializeField] private Button back;
    
    [SerializeField] private Button confirmNewGame;
    
    [SerializeField] private Button backNewGame;

    [SerializeField] private GameObject transitionNewGame;
    
    [SerializeField] private GameObject canvasMainMenu;
    
    [SerializeField] private Animator loadAnimator;

    [SerializeField] private GameObject chtulo;

    [SerializeField] private Sprite chtuloCasting;

    [SerializeField] private AudioManager audioManager;
    
    [SerializeField] private GraphManager graphManager;

    [SerializeField] private MusicManager musicManager;
    
    private bool inMenu;

    public bool InMenu => inMenu;
    
    private bool inTransition;
    
    public bool InTransition => inTransition;

    private static MainMenuManager _instance;
    
    public static MainMenuManager MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<MainMenuManager>();
            }

            return _instance;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        newGame.onClick.AddListener(OnNewGamePressed);
        resumeGame.onClick.AddListener(OnResumeGamePressed);
        settings.onClick.AddListener(OnSettingsPressed);
        quit.onClick.AddListener(OnQuitPressed);
        
        audio.onClick.AddListener(OnAudioPressed);
        graphics.onClick.AddListener(OnGraphicsPressed);
        back.onClick.AddListener(OnBackPressed);
        
        confirmNewGame.onClick.AddListener(OnConfirmNewGamePressed);
        backNewGame.onClick.AddListener(OnBackNewGamePressed);

        LoadPlayerSettings();

        PlayerPrefs.SetInt("Restart", 0);
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || !inMenu) return;

        UpdateMenu();
    }

    // Set audio and graphic
    private void LoadPlayerSettings() {
        audioManager.InitMasterVolume();
        graphManager.InitGraphics();
    }

    // Switch between menus
    public void UpdateMenu()
    {
        if (menuAudio.activeSelf || menuGraphics.activeSelf)
        {
            menuAudio.SetActive(false);
            menuGraphics.SetActive(false);
            menuSettings.SetActive(true);
        }
        else
        {
            inMenu = false;
            menuSettings.SetActive(false);
        }
    }

    private void OnBackNewGamePressed()
    {
        if (inMenu || inTransition) return;
        
        // Hide confirm menu (showing before starting new game)
        for (var i = 0; i < GameObject.Find("CanvasSecondMenu").transform.childCount; i++)
        {
            GameObject.Find("CanvasSecondMenu").transform.GetChild(i).gameObject.SetActive(false);
        }
        
        // Show main menu
        for (var i = 0; i < GameObject.Find("CanvasFirstMenu").transform.childCount; i++)
        {
            GameObject.Find("CanvasFirstMenu").transform.GetChild(i).gameObject.GetComponent<MainButtonManager>().OnButton();
        }
    }

    // Start transition when starting new game
    private void OnConfirmNewGamePressed()
    {
        if (inMenu || inTransition) return;
        
        inTransition = true;
        StartCoroutine(StartNewGame());
    }

    private void OnNewGamePressed()
    {
        if (inMenu || inTransition) return;
        
        var saveData = SaveSystem.LoadGame();

        // Start transition when starting new game
        if (saveData == null)
        {
            inTransition = true;
            StartCoroutine(StartNewGame());
        }
        // Show confirm menu (to erase save) before starting menu
        else
        {
            for (var i = 0; i < GameObject.Find("CanvasFirstMenu").transform.childCount; i++)
            {
                GameObject.Find("CanvasFirstMenu").transform.GetChild(i).gameObject.GetComponent<MainButtonManager>().OffButton();
            }
            
            for (var i = 0; i < GameObject.Find("CanvasSecondMenu").transform.childCount; i++)
            {
                GameObject.Find("CanvasSecondMenu").transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Launches the game with the saved data
    private void OnResumeGamePressed()
    {
        if (inMenu || inTransition) return;

        var saveData = SaveSystem.LoadGame();
        
        if (saveData == null)
        {
            noSaveText.SetActive(true);
        }
        else
        {
            inTransition = true;
            StartCoroutine(FadeOutMainMenu());
            StartCoroutine(LoadVillage());
        }
    }
    
    private IEnumerator LoadVillage()
    {
        loadAnimator.SetTrigger("Start");
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("VillageScene");
    }

    // Start music, delete save and witch to introduction scene
    private IEnumerator StartNewGame()
    {
        musicManager.SetCurrentMusic("cinematic");

        chtulo.GetComponent<SpriteRenderer>().sprite = chtuloCasting;
        
        SaveSystem.DeleteSave();
        
        transitionNewGame.SetActive(true);

        StartCoroutine(FadeOutMainMenu());
        
        yield return new WaitForSeconds(2);

        var animator = transitionNewGame.GetComponent<Animator>();
        animator.SetTrigger("Step");
        
        yield return new WaitForSeconds(2);
        
        SceneManager.LoadScene("OpeningScene");
    }
    
    // Fade out main menu
    private IEnumerator FadeOutMainMenu()
    {
        var startAlpha = canvasMainMenu.GetComponent<CanvasGroup>().alpha;

        while (startAlpha > 0)
        {
            startAlpha -= 0.1f;
            canvasMainMenu.GetComponent<CanvasGroup>().alpha = startAlpha;

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Show settings
    private void OnSettingsPressed()
    {
        if (inMenu || inTransition) return;
        
        inMenu = true;
        menuSettings.SetActive(true);
        noSaveText.SetActive(false);
    }
    
    // Leave game
    private void OnQuitPressed()
    {
        if (inMenu || inTransition) return;

        inTransition = true;
        PlayerPrefs.SetInt("Restart", 0);
        Application.Quit();
    }
    
    // Show audio settings
    private void OnAudioPressed()
    {
        if (inTransition) return;

        inMenu = true;
        menuSettings.SetActive(false);
        menuAudio.SetActive(true);
    }
    
    // Show graphic settings
    private void OnGraphicsPressed()
    {
        if (inTransition) return;

        inMenu = true;
        menuSettings.SetActive(false);
        menuGraphics.SetActive(true);
    }
    
    private void OnBackPressed()
    {
        if (inTransition) return;
        
        inMenu = false;
        menuSettings.SetActive(false);
        menuAudio.SetActive(false);
        menuGraphics.SetActive(false);
    }
}
