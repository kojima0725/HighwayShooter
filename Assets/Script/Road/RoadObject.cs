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

    bool joined = false;

    protected virtual void Awake()
    {
        if (Road.current)
        {
            Road.current.Join(transform);
            joined = true;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!joined)
        {
            Road.current.Join(transform);
            joined = true;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.position.z < deadPos)
        {
            Death();
        }
    }

    /// <summary>
    /// 自身を消滅させる
    /// </summary>
    public virtual void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        //道のリストから自身を削除
        Road.current.Leave(transform);
    }
}
