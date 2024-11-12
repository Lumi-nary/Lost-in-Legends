
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambientSlider;

    // Constants for volume settings
    private const float MIN_VOLUME = 0.001f; // -80dB
    private const float MAX_VOLUME = 1f; // 0dB
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string AMBIENT_VOLUME_KEY = "AmbientVolume";

    private void Awake()
    {
        // Load saved volumes
        LoadVolumes();
    }

    private void Start()
    {
        // Add listeners to sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        ambientSlider.onValueChanged.AddListener(SetAmbientVolume);

        // Initialize volumes with current slider values
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetAmbientVolume(ambientSlider.value);
    }
    private void LoadVolumes()
    {
        // Load saved volumes or set to default (0.75f = 75%)
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.75f);
        ambientSlider.value = PlayerPrefs.GetFloat(AMBIENT_VOLUME_KEY, 0.75f);
    }

    public void SetMusicVolume(float volume)
    {
        // Clamp the volume to prevent -infinity when converting to log10
        volume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    public void SetAmbientVolume(float volume)
    {
        volume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        myMixer.SetFloat("ambient", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(AMBIENT_VOLUME_KEY, volume);
    }

    private void OnDisable()
    {
        // Save volumes when script is disabled
        PlayerPrefs.Save();
    }

}
