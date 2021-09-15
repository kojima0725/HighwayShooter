using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceAudioClip : ChangeableSound
{
    bool playStarted;

    private void Update()
    {
        if (!playStarted)
        {
            if (source.isPlaying)
            {
                playStarted = true;
            }
            return;
        }
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
