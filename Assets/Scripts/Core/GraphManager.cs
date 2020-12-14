using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour
{
    [SerializeField] private Button fullscreen;

    [SerializeField] private Image fullscreenState;

    [SerializeField] private Button nextResolution;
    
    [SerializeField] private Button previousResolution;
    
    [SerializeField] private Text txtResolution;

    private readonly List<string> resolutions = new List<string>();
    
    [SerializeField] private Button nextGraphics;
    
    [SerializeField] private Button previousGraphics;
    
    [SerializeField] private Text txtGraphics;

    [SerializeField] private List<string> graphics;
    
    [SerializeField] private Button apply;
    
    [SerializeField] private Button back;

    private int currentResolutionIndex;
    
    private int currentGraphicIndex;

    private bool currentFullscreenState;
    
    private const string ResolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string ResolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string FullScreenPlayerPrefKey = "FullScreen";
    private const string Graphics = "Graphics";
    
    // Start is called before the first frame update
    private void Start()
    {
        InitGraphics();

        foreach (var t in Screen.resolutions)
        {
            resolutions.Add(t.width + "x" + t.height);
        }
        var (width, height) = GetResolution();
        currentResolutionIndex = resolutions.IndexOf(width + "x" + height);

        fullscreen.onClick.AddListener(OnFullscreenPressed);
        nextResolution.onClick.AddListener(OnNextResolutionPressed);
        previousResolution.onClick.AddListener(OnPreviousResolutionPressed);
        nextGraphics.onClick.AddListener(OnNextGraphicPressed);
        previousGraphics.onClick.AddListener(OnPreviousGraphicPressed);
        apply.onClick.AddListener(OnApplyPressed);
        back.onClick.AddListener(OnBackPressed);
    }

    public void InitGraphics()
    {
        currentFullscreenState = GetFullscreen();
        SetFullscreen(currentFullscreenState);
        
        var (width, height) = GetResolution();
        SetResolution(width, height);

        currentGraphicIndex = GetGraphics();
        SetGraphics(currentGraphicIndex);
    }

    private void OnApplyPressed()
    {
        SetFullscreen(currentFullscreenState);
        
        var resolution = resolutions[currentResolutionIndex].Split('x');
        SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]));
        
        SetGraphics(currentGraphicIndex);
        
        ApplyChanges();
    }

    private void ApplyChanges()
    {
        QualitySettings.SetQualityLevel(currentGraphicIndex);

        var mode = currentFullscreenState ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        var resolution = resolutions[currentResolutionIndex].Split('x');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), mode);
    }

    private void OnBackPressed()
    {
        if (GameManager.MyInstance) GameManager.MyInstance.SetGraphicMenu(false);
        else MainMenuManager.MyInstance.UpdateMenu();
    }

    private void OnFullscreenPressed()
    {
        currentFullscreenState = !currentFullscreenState;
        
        UpdateFullscreenCheckbox(currentFullscreenState);
    }
    
    private void OnNextGraphicPressed()
    {
        if (currentGraphicIndex + 1 < graphics.Count)
        {
            currentGraphicIndex += 1;
        }
        else
        {
            currentGraphicIndex = 0;
        }
        
        UpdateGraphicText(graphics[currentGraphicIndex]);
    }
    
    private void OnPreviousGraphicPressed()
    {
        if (currentGraphicIndex - 1 >= 0)
        {
            currentGraphicIndex -= 1;
        }
        else
        {
            currentGraphicIndex = graphics.Count - 1;
        }
        
        UpdateGraphicText(graphics[currentGraphicIndex]);
    }

    private void OnNextResolutionPressed()
    {
        if (currentResolutionIndex + 1 < resolutions.Count)
        {
            currentResolutionIndex += 1;
        }
        else
        {
            currentResolutionIndex = 0;
        }
        
        UpdateResolutionText(resolutions[currentResolutionIndex]);
    }
    
    private void OnPreviousResolutionPressed()
    {
        if (currentResolutionIndex - 1 >= 0)
        {
            currentResolutionIndex -= 1;
        }
        else
        {
            currentResolutionIndex = resolutions.Count - 1;
        }
        
        UpdateResolutionText(resolutions[currentResolutionIndex]);
    }

    private bool GetFullscreen()
    {
        return PlayerPrefs.GetInt(FullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) == 1;
    }
    
    private void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt(FullScreenPlayerPrefKey, isFullscreen ? 1 : 0);
        
        UpdateFullscreenCheckbox(isFullscreen);
    }
    
    private void UpdateFullscreenCheckbox(bool state)
    {
        var tempColor = fullscreenState.color;
        tempColor.a = state ? 1f : 0f;
        fullscreenState.color = tempColor;
    }
    
    private (int width, int height) GetResolution()
    {
        var width = PlayerPrefs.GetInt(ResolutionWidthPlayerPrefKey, Screen.currentResolution.width);
        var height = PlayerPrefs.GetInt(ResolutionHeightPlayerPrefKey, Screen.currentResolution.height);

        return (width, height);
    }

    private void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt(ResolutionWidthPlayerPrefKey, width);
        PlayerPrefs.SetInt(ResolutionHeightPlayerPrefKey, height);
        
        UpdateResolutionText(width + "x" + height);
    }

    private void UpdateResolutionText(string resolution)
    {
        txtResolution.text = resolution;
    }

    private int GetGraphics()
    {
        return PlayerPrefs.GetInt(Graphics, QualitySettings.GetQualityLevel());
    }

    private void SetGraphics(int graphic)
    {
        PlayerPrefs.SetInt(Graphics, graphic);
        
        UpdateGraphicText(graphics[graphic]);
    }
    
    private void UpdateGraphicText(string graphic)
    {
        txtGraphics.text = graphic;
    }
}
