
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        SetMusicVolume();
    }
        public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10 (volume) *20);
    }

    public void SetsfxVolume()
    {
        float SFX = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(SFX) * 20);
    }

}
