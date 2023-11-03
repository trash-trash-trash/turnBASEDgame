using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSingleton : MonoBehaviour
{
    #region SingletonStuff

    private static SFXSingleton instance;

    public static SFXSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SFXSingleton>();
                
                if (instance == null)
                {
                    GameObject obj = new GameObject("SFXSingleton");
                    instance = obj.AddComponent<SFXSingleton>();
                }
            }

            return instance;
        }
    }

    #endregion

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public AudioSource audioSource;

    public List<AudioClip> clipList = new List<AudioClip>();

    private void Start()
    {
        foreach (AudioClip newClip in clipList)
        {
            audioClips.Add(newClip.name, newClip);
        }
    }

    public void PlayClip(string key, AudioSource newSource)
    {
        if (audioClips.ContainsKey(key))
        {
            AudioClip clip = audioClips[key];

            audioSource = newSource;

            audioSource.clip = clip;

            audioSource.Play();
        }
        else
        {
            Debug.Log("Audio clip with key '" + key + "' not found.");
        }
    }
}