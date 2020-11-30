using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuSettings;
    
    [SerializeField] private GameObject menuAudio;
    
    [SerializeField] private GameObject menuGraphics;

    [SerializeField] private Button settings;
    
    [SerializeField] private Button quit;
    
    [SerializeField] private Button audio;
    
    [SerializeField] private Button graphics;
    
    [SerializeField] private Button back;

    private bool inMenu;

    public bool InMenu => inMenu;

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
        menuSettings.SetActive(false);
        menuAudio.SetActive(false);
        menuGraphics.SetActive(false);
        
        settings.onClick.AddListener(OnSettingsPressed);
        quit.onClick.AddListener(OnQuitPressed);
        
        audio.onClick.AddListener(OnAudioPressed);
        graphics.onClick.AddListener(OnGraphicsPressed);
        back.onClick.AddListener(OnBackPressed);
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || !inMenu) return;

        UpdateMenu();
    }

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

    private void OnSettingsPressed()
    {
        if (inMenu) return;
        
        inMenu = true;
        menuSettings.SetActive(true);
    }
    
    private void OnQuitPressed()
    {
        Application.Quit();
    }
    
    private void OnAudioPressed()
    {
        menuSettings.SetActive(false);
        menuAudio.SetActive(true);
    }
    
    private void OnGraphicsPressed()
    {
        menuSettings.SetActive(false);
        menuGraphics.SetActive(true);
    }
    
    private void OnBackPressed()
    {
        inMenu = false;
        menuSettings.SetActive(false);
        menuAudio.SetActive(false);
        menuGraphics.SetActive(false);
    }
}
