using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioReference", menuName = "Audio/Audio Reference")]
public class AudioReference : ScriptableObject
{
    //[SerializeField] private string categoryName;
    //[SerializeField] private AudioConfig config;

    //public string CategoryName => categoryName;
    //public AudioConfig Config => config;

    //public void Validate()
    //{
    //    if (config != null && !string.IsNullOrEmpty(categoryName))
    //    {
    //        var category = config.GetSFXCategory(categoryName);
    //        if (category == null)
    //        {
    //            Debug.LogError($"Audio Reference '{name}' points to non-existent category '{categoryName}' in config '{config.name}'");
    //        }
    //    }
    //}
}
