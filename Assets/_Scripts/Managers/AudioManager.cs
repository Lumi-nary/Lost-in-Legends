using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Global Access
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            // If the instance isnt set find or create it
            // not recommended since i dont know how to set the SO directly yet
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    [Header("Configuration")]
    [SerializeField] private AudioConfig audioConfig; // Stores references to all audio clips
    [SerializeField] private AudioMixer audioMixer;   // Audio mixer for controlling sound groups

    [Header("Audio Sources")]
    [SerializeField] private int sfxSourcePoolSize = 5;  // Number of SFX sources to pre-instantiate
    private List<AudioSource> sfxSourcePool; // Pool of reusable audio sources for SFX

    [Header("Core Audio Sources")]
    private AudioSource musicSource;
    private Dictionary<AmbientKey, AudioSource> ambientSources = new Dictionary<AmbientKey, AudioSource>();

    private MusicKey currentMusic;

    private void Awake()
    {
        // Ensure there's only one instance of AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
            InitializeAudioSources(); // Set up audio sources
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManagers
        }
    }

    #region Setup
    private void InitializeAudioSources()
    {
        // Create and configure the music audio source
        musicSource = CreateAudioSource("MusicSource");
        musicSource.loop = true; // Music loops by default

        // Create a pool of audio sources for SFX
        sfxSourcePool = new List<AudioSource>();
        GameObject sfxPool = new GameObject("SFX_Pool");
        sfxPool.transform.parent = transform;

        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            GameObject sfxObject = new GameObject($"SFX_{i}");
            sfxObject.transform.parent = sfxPool.transform;

            AudioSource source = sfxObject.AddComponent<AudioSource>();
            source.playOnAwake = false; // SFX should not auto-play
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            sfxSourcePool.Add(source);
        }

        // Assign the music source to the "Music" mixer group
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
    }

    private AudioSource CreateAudioSource(string name)
    {
        // Helper method to create a new AudioSource and attach it to AudioManager
        var go = new GameObject(name);
        go.transform.parent = transform;
        return go.AddComponent<AudioSource>();
    }
    #endregion

    #region SFX System
    private AudioSource GetAvailableSFXSource()
    {
        // Find an available audio source from the pool, or use the first one if all are busy
        foreach (var source in sfxSourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        return sfxSourcePool[0]; // Fallback if all sources are in use
    }

    // Play sound by category name
    // Plays an SFX clip based on the key provided, optionally at a specific position
    public void PlaySFX(SFXKey key, Vector3? position = null, float volumeMultiplier = 1f)
    {
        if (audioConfig == null) return;

        var category = audioConfig.GetSFXCategory(key);
        if (category == null) return;

        AudioClip clip = category.GetRandomClip();
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.clip = clip;
        source.volume = category.volume * volumeMultiplier;
        source.spatialBlend = 0f; // SFX is non-spatial by default because this is 2D with my big D

        if (position.HasValue)
        {
            source.transform.position = position.Value;
        }

        source.Play();
    }

    // Plays a quick one-shot SFX that doesn’t need a separate AudioSource
    public void PlaySFXOneShot(SFXKey key, float volumeMultiplier = 1f)
    {
        if (audioConfig == null) return;

        var category = audioConfig.GetSFXCategory(key);
        if (category == null) return;

        AudioClip clip = category.GetRandomClip();
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.PlayOneShot(clip, category.volume * volumeMultiplier);
    }
    #endregion

    #region Music System
    public void PlayMusic(MusicKey key, bool fadeIn = true)
    {
        // Play new music only if it’s different from the current track
        var musicTrack = audioConfig.GetMusicTrack(key);
        if (musicTrack == null || key == currentMusic) return;

        // Stop current music first
        StopMusic(fadeIn);

        // Start new music
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeIn));
        currentMusic = key; // Update the current music track
    }

    private IEnumerator PlayMusicRoutine(AudioConfig.MusicTrack track, bool fadeIn)
    {
        // Setup main track
        musicSource.clip = track.mainTrack;
        musicSource.volume = 0f;
        musicSource.Play();

        // Music fadein entrance
        if (fadeIn)
        {
            float elapsed = 0f;
            while (elapsed < track.fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / track.fadeInDuration;
                musicSource.volume = Mathf.Lerp(0f, track.volume, t);
                yield return null;
            }
        }
    }

    // Stops the current music, with an optional fade-out effect
    public void StopMusic(bool fadeOut = true)
    {
        if (fadeOut && musicSource.isPlaying)
        {
            StartCoroutine(StopMusicRoutine());
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    private IEnumerator StopMusicRoutine()
    {
        // Gradually reduce the volume for a fade-out effect
        var track = audioConfig.GetMusicTrack(currentMusic);
        if (track == null) yield break;

        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < track.fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / track.fadeOutDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;
    }

    #endregion

    #region Ambient System
    public void PlayAmbient(AmbientKey key, bool fadeIn = true)
    {
        // Retrieve or create an audio source for the given ambient key
        var ambientSound = audioConfig.GetAmbientSound(key);
        if (ambientSound == null) return;

        if (!ambientSources.ContainsKey(key))
        {
            var source = CreateAudioSource($"Ambient_{key}");
            source.clip = ambientSound.clip;
            source.loop = true;
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambient")[0];
            ambientSources.Add(key, source);
        }
        // Start playing the ambient sound with optional fade-in
        var audioSource = ambientSources[key];
        StartCoroutine(PlayAmbientRoutine(key, ambientSound, fadeIn));
    }

    private IEnumerator PlayAmbientRoutine(AmbientKey key, AudioConfig.AmbientSound ambient, bool fadeIn)
    {
        // Set up the volume and start playing
        var source = ambientSources[key];
        source.volume = fadeIn ? 0f : ambient.volume;
        source.Play();

        // Fade-in effect if enabled
        if (fadeIn)
        {
            float elapsed = 0f;
            while (elapsed < ambient.fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / ambient.fadeInDuration;
                source.volume = Mathf.Lerp(0f, ambient.volume, t);
                yield return null;
            }
        }
    }

    public void StopAmbient(AmbientKey key, bool fadeOut = true)
    {
        // Stop ambient sound for a given key with optional fade-out

        if (!ambientSources.ContainsKey(key)) return;

        var ambientSound = audioConfig.GetAmbientSound(key);
        if (ambientSound == null) return;

        if (fadeOut)
        {
            StartCoroutine(StopAmbientRoutine(key, ambientSound));
        }
        else
        {
            ambientSources[key].Stop();
        }
    }

    private IEnumerator StopAmbientRoutine(AmbientKey key, AudioConfig.AmbientSound ambient)
    {
        // Fade-out effect for ambient sound
        var source = ambientSources[key];
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < ambient.fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / ambient.fadeOutDuration;
            source.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        source.Stop();
    }
    #endregion
}