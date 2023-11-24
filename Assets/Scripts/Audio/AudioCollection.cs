using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "ScriptableObjects/AudioCollection", order = 1)]
public class AudioCollection : ScriptableObject
{
    [Header("Audio clips")]
    public Dictionary<string, AudioClip> backgroundSounds;
    public Dictionary<string, AudioClip> SFXSounds, texts;

    [Header("Audio clips")]
    public string pathForBackgroundSounds;
    public string pathForSFXSounds, pathForTexts;

    public void Init()
    {
        backgroundSounds = CreateDictionary(pathForBackgroundSounds);
        SFXSounds = CreateDictionary(pathForSFXSounds);
        texts = CreateDictionary(pathForTexts);
    }

    private Dictionary<string, AudioClip> CreateDictionary(string path)
    {
        Dictionary<string, AudioClip> clipsDictonary = new();
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
        string key;

        foreach (AudioClip clip in clips)
        {
            key = clip.name;

            if (!clipsDictonary.ContainsKey(key))
            {
                clipsDictonary.Add(key, clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate key '{key}' found when loading from Resources folder. Skipping.");
            }
        }

        return clipsDictonary;
    }
}