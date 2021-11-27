using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleUI : MonoBehaviour
{
    [SerializeField]
    Animator[] sticks;
    bool shootSwitch;

    public void PlayShootEffect()
    {
        shootSwitch = !shootSwitch;
        if (shootSwitch)
        {
            foreach (var item in sticks)
            {
                item.CrossFade("s1", 0.08f);
            }
        }
        else
        {
            foreach (var item in sticks)
            {
                item.CrossFade("s2", 0.08f);
            }
        }
        
    }
}
