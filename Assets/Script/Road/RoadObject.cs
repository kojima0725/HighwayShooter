using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 後ろに流れるオブジェクト達
/// </summary>
public class RoadObject : MonoBehaviour
{
    /// <summary>
    /// 自身が削除される位置
    /// </summary>
    [SerializeField]
    float deadPos;

    // Start is called before the first frame update
    void Start()
    {
        Road.current.Join(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < deadPos)
        {
            Death();
        }
    }

    /// <summary>
    /// 自身を消滅させる
    /// </summary>
    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        //道のリストから自身を削除
        Road.current.Leave(transform);
    }
}
