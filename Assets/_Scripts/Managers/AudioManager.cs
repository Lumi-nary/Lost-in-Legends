using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
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
    [SerializeField] private AudioConfig audioConfig;

    [Header("Audio Sources")]
    [SerializeField] private int sfxSourcePoolSize = 5;
    [SerializeField] private List<AudioSource> sfxSourcePool;

    [Header("Music System")]
    private AudioSource musicMainSource;
    private List<AudioSource> musicLayerSources = new List<AudioSource>();
    private string currentMusicTrack;
    private List<float> layerVolumes = new List<float>();

    [Header("Ambient System")]
    private Dictionary<string, AudioSource> ambientSources = new Dictionary<string, AudioSource>();

    [Header("Volume Controls")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float ambientVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Setup
    private void InitializeAudioSources()
    {
        // Initialize music sources
        musicMainSource = CreateAudioSource("MusicMain");
        musicMainSource.loop = true;

        // Initialize SFX pool
        sfxSourcePool = new List<AudioSource>();
        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            AudioSource source = CreateAudioSource($"SFX_{i}");
            source.playOnAwake = false;
            sfxSourcePool.Add(source);
        }
    }

    private AudioSource CreateAudioSource(string name)
    {
        var go = new GameObject(name);
        go.transform.parent = transform;
        return go.AddComponent<AudioSource>();
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        return sfxSourcePool[0]; // Reuse the first source if all are busy
    }
    #endregion

    #region SFX System
    // Play sound by category name
    public void PlaySFX(string categoryName, Vector3? position = null, float volumeMultiplier = 1f)
    {
        if (audioConfig == null) return;

        var category = audioConfig.GetSFXCategory(categoryName);
        if (category == null) return;

        AudioClip clip = category.GetRandomClip();
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.clip = clip;
        source.volume = sfxVolume * masterVolume * category.volume * volumeMultiplier;

        // Since it's a 2D game, we'll default to 2D sound
        source.spatialBlend = 0f;

        if (position.HasValue)
        {
            source.transform.position = position.Value;
            // You can still use spatial blend if you want positional audio in your 2D game
            //source.spatialBlend = 1f; // Uncomment if you want positional audio
        }

        source.Play();
    }

    public void PlaySFXOneShot(string categoryName, float volumeMultiplier = 1f)
    {
        if (audioConfig == null) return;

        var category = audioConfig.GetSFXCategory(categoryName);
        if (category == null) return;

        AudioClip clip = category.GetRandomClip();
        if (clip == null) return;

        GetAvailableSFXSource().PlayOneShot(clip, sfxVolume * masterVolume * category.volume * volumeMultiplier);
    }
    #endregion

    #region Music System
    public void PlayMusic(string trackName, bool fadeIn = true)
    {
        var musicTrack = audioConfig.GetMusicTrack(trackName);
        if (musicTrack == null) return;

        // If we're already playing this track, don't restart
        if (currentMusicTrack == trackName) return;

        // Stop current music first
        StopMusic(fadeIn);

        // Start new music
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeIn));
        currentMusicTrack = trackName;
    }

    private IEnumerator PlayMusicRoutine(AudioConfig.MusicTrack track, bool fadeIn)
    {
        // Setup main track
        musicMainSource.clip = track.mainTrack;
        musicMainSource.volume = 0f;
        musicMainSource.Play();

        // Setup layers
        SetupMusicLayers(track);

        // Music for layer
        if (fadeIn)
        {
            float elapsed = 0f;
            while (elapsed < track.fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / track.fadeInDuration;

                // Fade in main track
                musicMainSource.volume = Mathf.Lerp(0f, track.volume * musicVolume * masterVolume, t);

                // Fade in layers
                for (int i = 0; i < musicLayerSources.Count; i++)
                {
                    musicLayerSources[i].volume = Mathf.Lerp(0f, layerVolumes[i] * musicVolume * masterVolume, t);
                }

                yield return null;
            }
        }
        else // Music for non-layer
        {
            // why programming needs math :skull:
            musicMainSource.volume = track.volume * musicVolume * masterVolume;
            for (int i = 0; i < musicLayerSources.Count; i++)
            {
                musicLayerSources[i].volume = layerVolumes[i] * musicVolume * masterVolume;
            }
        }
    }

    private void SetupMusicLayers(AudioConfig.MusicTrack track)
    {
        // Clear existing layers
        foreach (var source in musicLayerSources)
        {
            Destroy(source.gameObject);
        }
        musicLayerSources.Clear();
        layerVolumes.Clear();

        // Create new layers
        if (track.layers != null)
        {
            foreach (var layer in track.layers)
            {
                var layerSource = CreateAudioSource($"MusicLayer_{musicLayerSources.Count}");
                layerSource.clip = layer;
                layerSource.loop = true;
                layerSource.volume = 0f;
                layerSource.Play();

                musicLayerSources.Add(layerSource);
                layerVolumes.Add(0f); // Start with layers muted
            }
        }
    }
    // Gradually bring in different layers based on game intensity
    //AudioManager.Instance.SetMusicLayerVolume(0, 0.8f); // Normal at 80%
    //AudioManager.Instance.SetMusicLayerVolume(2, 0.5f); // Tension at 50%
    // too much thinking that this might become useless in long run i dont know just future proofing
    public void SetMusicLayerVolume(int layerIndex, float volume)
    {
        if (layerIndex >= 0 && layerIndex < musicLayerSources.Count)
        {
            layerVolumes[layerIndex] = Mathf.Clamp01(volume);
            musicLayerSources[layerIndex].volume = layerVolumes[layerIndex] * musicVolume * masterVolume;
        }
    }

    // stops music
    public void StopMusic(bool fadeOut = true)
    {
        if (fadeOut && musicMainSource.isPlaying)
        {
            StartCoroutine(StopMusicRoutine());
        }
        else
        {
            musicMainSource.Stop();
            foreach (var layer in musicLayerSources)
            {
                layer.Stop();
            }
        }
    }

    private IEnumerator StopMusicRoutine()
    {
        var track = audioConfig.GetMusicTrack(currentMusicTrack);
        if (track == null) yield break;

        float startVolume = musicMainSource.volume;
        float elapsed = 0f;

        while (elapsed < track.fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / track.fadeOutDuration;

            musicMainSource.volume = Mathf.Lerp(startVolume, 0f, t);

            // Fade out layers
            for (int i = 0; i < musicLayerSources.Count; i++)
            {
                float layerStartVolume = musicLayerSources[i].volume;
                musicLayerSources[i].volume = Mathf.Lerp(layerStartVolume, 0f, t);
            }

            yield return null;
        }

        musicMainSource.Stop();
        foreach (var layer in musicLayerSources)
        {
            layer.Stop();
        }
    }

    #endregion

    #region Ambient System
    public void PlayAmbient(string soundName, bool fadeIn = true)
    {
        var ambientSound = audioConfig.GetAmbientSound(soundName);
        if (ambientSound == null) return;

        if (!ambientSources.ContainsKey(soundName))
        {
            var source = CreateAudioSource($"Ambient_{soundName}");
            source.clip = ambientSound.clip;
            source.loop = true;
            ambientSources.Add(soundName, source);
        }

        var audioSource = ambientSources[soundName];
        StartCoroutine(PlayAmbientRoutine(soundName, ambientSound, fadeIn));
    }

    private IEnumerator PlayAmbientRoutine(string soundName, AudioConfig.AmbientSound ambient, bool fadeIn)
    {
        var source = ambientSources[soundName];
        source.volume = fadeIn ? 0f : ambient.volume * ambientVolume * masterVolume;
        source.Play();

        if (fadeIn)
        {
            float elapsed = 0f;
            while (elapsed < ambient.fadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / ambient.fadeInDuration;
                source.volume = Mathf.Lerp(0f, ambient.volume * ambientVolume * masterVolume, t);
                yield return null;
            }
        }

        // Handle random delays between loops if specified
        if (ambient.randomDelayRange != Vector2.zero)
        {
            while (true)
            {
                yield return new WaitForSeconds(source.clip.length);
                float delay = Random.Range(ambient.randomDelayRange.x, ambient.randomDelayRange.y);
                source.Pause();
                yield return new WaitForSeconds(delay);
                source.UnPause();
            }
        }
    }

    public void StopAmbient(string soundName, bool fadeOut = true)
    {
        if (!ambientSources.ContainsKey(soundName)) return;

        var ambientSound = audioConfig.GetAmbientSound(soundName);
        if (ambientSound == null) return;

        if (fadeOut)
        {
            StartCoroutine(StopAmbientRoutine(soundName, ambientSound));
        }
        else
        {
            ambientSources[soundName].Stop();
        }
    }

    private IEnumerator StopAmbientRoutine(string soundName, AudioConfig.AmbientSound ambient)
    {
        var source = ambientSources[soundName];
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

    #region Volume Controls
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetAmbientVolume(float volume)
    {
        ambientVolume = Mathf.Clamp01(volume);
    }

    #endregion
}