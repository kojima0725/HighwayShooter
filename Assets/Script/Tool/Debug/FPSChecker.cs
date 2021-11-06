using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フレームレート計測器
/// </summary>
public class FPSChecker : MonoBehaviour
{
    [SerializeField]
    Text text;

    // Update is called once per frame
    void Update()
    {
        if (text)
        {
            text.text = ((int)(1.0f / Time.deltaTime)).ToString();
        }
        
    }
}
