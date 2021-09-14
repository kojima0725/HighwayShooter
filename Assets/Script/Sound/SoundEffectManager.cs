using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            changeableSounds.Add(BGMAudioSource);
        }
    }

    [SerializeField]
    AudioSource UIAudioSource;
    [SerializeField]
    AudioSource BGMAudioSource;
    [SerializeField]
    AudioSource EffectAudioSource;

    readonly List<AudioSource> changeableSounds = new List<AudioSource>();

    public void Join(AudioSource source)
    {
        changeableSounds.Add(source);
    }

    public void Leave(AudioSource source)
    {
        changeableSounds.Remove(source);
    }

    public void SetSoundPitchToAll(float pitch)
    {
        foreach (var item in changeableSounds)
        {
            if (item)
            {
                item.pitch = pitch;
            }
        }
    }

}
