using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーターのウェイトを変える
/// </summary>
public class AnimatorWeightChanger : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    float nomalWeight;
    [SerializeField]
    float slowWeight = 1;

    private void Update()
    {
        float timeScale = Time.timeScale;
        if (timeScale < 1.0f)
        {
            if (timeScale < 0.5f)
            {
                SetWeight(slowWeight);
            }
            else
            {
                float p = (timeScale - 0.5f) * 2;
                float q = 1.0f - p;
                SetWeight(p * nomalWeight + q * slowWeight);
            }
        }
        else
        {
            SetWeight(nomalWeight);
        }
    }

    private void SetWeight(float weight)
    {
        animator.SetLayerWeight(1, weight);
    }
}
