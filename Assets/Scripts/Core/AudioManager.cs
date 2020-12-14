using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volume;

    [SerializeField] private AudioMixer audioMixer;
    
    [SerializeField] private Button back;

    private float currentVolume;
    
    private const string AudioMaster = "AudioMaster";
    
    // Start is called before the first frame update
    private void Start()
    {
        InitMasterVolume();
        
        volume.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        back.onClick.AddListener(OnBackPressed);
    }

    public void InitMasterVolume()
    {
        currentVolume = GetVolume();
        SetVolume(currentVolume);
    }
    
    private void OnBackPressed()
    {
        if (GameManager.MyInstance) GameManager.MyInstance.SetAudioMenu(false);
        else MainMenuManager.MyInstance.UpdateMenu();
    }

    private void OnVolumeChanged()
    {
        SetVolume(volume.value);
    }

    private float GetVolume()
    {
        return PlayerPrefs.GetFloat(AudioMaster, audioMixer.GetFloat("Volume", out var volumeValue) ? volumeValue : -25f);
    }

    private void SetVolume(float value)
    {
        audioMixer.SetFloat("Volume", value);
        
        PlayerPrefs.SetFloat(AudioMaster, value);
        
        UpdateSlider(value);
    }

    private void UpdateSlider(float value)
    {
        volume.value = value;
    }
}
