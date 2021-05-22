using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 後ろに流れるオブジェクト達
/// </summary>
public class RoadObject : MonoBehaviour
{
    /// <summary>
    /// Roadスクリプトに参加したかどうか
    /// </summary>
    protected bool joined = false;

    protected virtual void Awake()
    {
        if (Road.current)
        {
            Join();
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!joined)
        {
            Join();
        }
    }

    protected virtual void Join()
    {
        Road.current.Join(transform);
        joined = true;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// 自身を消滅させる
    /// </summary>
    public virtual void Death()
    {
        Destroy(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        //道のリストから自身を削除
        Road.current.Leave(transform);
    }
}
