using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volume;

    [SerializeField] private AudioMixer audioMixer;

    private float currentVolume;
    
    private const string AudioMaster = "AudioMaster";
    
    // Start is called before the first frame update
    private void Start()
    {
        currentVolume = GetVolume();
        SetVolume(currentVolume);
        
        volume.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
    }

    private void OnVolumeChanged()
    {
        SetVolume(volume.value);
    }

    private float GetVolume()
    {
        return PlayerPrefs.GetFloat(AudioMaster, audioMixer.GetFloat("Volume", out var volumeValue) ? volumeValue : 40f);
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
