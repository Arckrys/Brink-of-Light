using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Button btnResume;
    
    [SerializeField] private Button btnVideo;
    
    [SerializeField] private Button btnSound;
    
    [SerializeField] private Button btnRestart;
    
    [SerializeField] private Button btnTown;
    
    [SerializeField] private Button btnMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        btnResume.onClick.AddListener(OnResumePressed);
        btnVideo.onClick.AddListener(OnResumePressed);
        btnSound.onClick.AddListener(OnResumePressed);
        btnRestart.onClick.AddListener(OnResumePressed);
        btnTown.onClick.AddListener(OnResumePressed);
        btnMenu.onClick.AddListener(OnResumePressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void OnResumePressed()
    {
        GameManager.MyInstance.EditPauseState(false);
    }
}
