using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 後ろに流れるオブジェクト達
/// </summary>
public class RoadObject : MonoBehaviour
{
    /// <summary>
    /// 自身が削除されるタイミング
    /// </summary>
    [SerializeField]
    float deadPos;

    // Start is called before the first frame update
    void Awake()
    {
        Road.current.Join(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < deadPos)
        {
            Road.current.Leave(transform);
            Destroy(this);
        }
    }
}
