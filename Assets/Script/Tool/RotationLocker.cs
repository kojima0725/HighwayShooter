using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの回転をさせない
/// </summary>
public class RotationLocker : MonoBehaviour
{
    Quaternion rotation;
    private void Start()
    {
        rotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = rotation;
    }
}
