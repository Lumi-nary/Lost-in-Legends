using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Configuration")]
public class AudioConfig : ScriptableObject
{
    [System.Serializable]
    public class SoundCategory
    {
        public SFXKey key;
        [Range(0f, 1f)]
        public float volume = 1f;
        public List<AudioClip> clips;

        public AudioClip GetRandomClip()
        {
            if (clips == null || clips.Count == 0) return null;
            return clips[Random.Range(0, clips.Count)];
        }
    }

    [System.Serializable]
    public class MusicTrack
    {
        public MusicKey key;
        public AudioClip mainTrack;
        [Tooltip("Optional layers that can be mixed in")]
        public List<AudioClip> layers;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0f, 10f)]
        public float fadeInDuration = 2f;
        [Range(0f, 10f)]
        public float fadeOutDuration = 2f;
    }

    [System.Serializable]
    public class AmbientSound
    {
        public AmbientKey key;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0f, 10f)]
        public float fadeInDuration = 1f;
        [Range(0f, 10f)]
        public float fadeOutDuration = 1f;
        [Tooltip("Random delay between loops")]
        public Vector2 randomDelayRange = new Vector2(0f, 0f);
    }

    public List<SoundCategory> sfxCategories = new List<SoundCategory>();
    public List<MusicTrack> musicTracks = new List<MusicTrack>();
    public List<AmbientSound> ambientSounds = new List<AmbientSound>();

    public SoundCategory GetSFXCategory(SFXKey key) =>
        sfxCategories.FirstOrDefault(c => c.key == key);

    public MusicTrack GetMusicTrack(MusicKey key) =>
        musicTracks.FirstOrDefault(t => t.key == key);

    public AmbientSound GetAmbientSound(AmbientKey key) =>
        ambientSounds.FirstOrDefault(a => a.key == key);

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Check for duplicate keys
        var duplicateSFX = sfxCategories.GroupBy(x => x.key).Where(g => g.Count() > 1).Select(g => g.Key);
        foreach (var key in duplicateSFX)
        {
            Debug.LogError($"Duplicate SFX key found: {key}");
        }

        var duplicateMusic = musicTracks.GroupBy(x => x.key).Where(g => g.Count() > 1).Select(g => g.Key);
        foreach (var key in duplicateMusic)
        {
            Debug.LogError($"Duplicate Music key found: {key}");
        }

        var duplicateAmbient = ambientSounds.GroupBy(x => x.key).Where(g => g.Count() > 1).Select(g => g.Key);
        foreach (var key in duplicateAmbient)
        {
            Debug.LogError($"Duplicate Ambient key found: {key}");
        }
    }
#endif
}
