using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrip : MonoBehaviour
{
    public AudioSource audioSource;

    public SFXSingleton SFX;

    public float time;
    public float minTime;
    public float maxTime;

    public List<AudioClip> waterDrips = new List<AudioClip>();

    public void OnEnable()
    {
        SFX = SFXSingleton.Instance;
        StartCoroutine(Drip());
    }

    IEnumerator Drip()
    {
        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);
        
        int randomIndex = Random.Range(0, waterDrips.Count);
        AudioClip randomClip = waterDrips[randomIndex];
        
        SFX.PlayClip(randomClip.name, audioSource);

        StartCoroutine(Drip());
    }
}
