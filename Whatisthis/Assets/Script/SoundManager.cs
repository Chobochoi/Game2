using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSoure;
    public AudioSource musicSource;
    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    public static SoundManager instance = null;

    public void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSoure.clip = clip;

        efxSoure.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIdx = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSoure.pitch = randomPitch;
        efxSoure.clip = clips[randomIdx];
        efxSoure.Play();
    }
}
