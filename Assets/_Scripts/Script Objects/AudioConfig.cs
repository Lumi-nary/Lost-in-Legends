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
        public string name;
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
        public string name;
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
        public string name;
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

    public SoundCategory GetSFXCategory(string categoryName) =>
        sfxCategories.FirstOrDefault(c => c.name.Equals(categoryName, System.StringComparison.OrdinalIgnoreCase));

    public MusicTrack GetMusicTrack(string trackName) =>
        musicTracks.FirstOrDefault(t => t.name.Equals(trackName, System.StringComparison.OrdinalIgnoreCase));

    public AmbientSound GetAmbientSound(string soundName) =>
        ambientSounds.FirstOrDefault(a => a.name.Equals(soundName, System.StringComparison.OrdinalIgnoreCase));
}
