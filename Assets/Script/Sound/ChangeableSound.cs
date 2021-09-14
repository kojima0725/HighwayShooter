using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ピッチ変更可能な効果音
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ChangeableSound : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        SoundEffectManager.instance?.Join(source);
    }

    private void OnDestroy()
    {
        SoundEffectManager.instance?.Leave(source);
    }
}
