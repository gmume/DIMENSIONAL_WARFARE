using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "AudioLoader", menuName = "ScriptableObjects/AudioLoader", order = 1)]

public class AudioLoader : ScriptableObject
{
    [Header("Audio clips")]
    public Dictionary<string, AudioClip> backgroundSounds;
    public Dictionary<string, AudioClip> sfxSounds, texts;

    [Header("Paths")]
    public string pathForBackgroundSounds;
    public string pathForSFXSounds, pathForTexts;

    public void Initialize()
    {
        backgroundSounds = LoadAudioDictionary(pathForBackgroundSounds);
        sfxSounds = LoadAudioDictionary(pathForSFXSounds);
        texts = LoadAudioDictionary(pathForTexts);
    }

    private Dictionary<string, AudioClip> LoadAudioDictionary(string path)
    {
        Dictionary<string, AudioClip> audioDictionary = new();
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(path);
        string key;

        foreach (AudioClip clip in audioClips)
        {
            key = clip.name;

            if (!audioDictionary.ContainsKey(key))
            {
                audioDictionary.Add(key, clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate key '{key}' found when loading from Resources folder. Skipping.");
            }
        }

        return audioDictionary;
    }
}