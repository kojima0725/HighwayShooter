using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自身に所属しているRoadObjectを後ろに流していく
/// </summary>
public class Road : MonoBehaviour
{
    public static Road current;

    private void Awake()
    {
        current = this;
    }

    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField]
    float speed;

    /// <summary>
    /// 後ろに流していくオブジェクト達
    /// </summary>
    readonly List<Transform> roads = new List<Transform>();


    /// <summary>
    /// 後ろに流すオブジェクトを登録する
    /// </summary>
    /// <param name="obj"></param>
    public void Join(Transform obj)
    {
        roads.Add(obj);
    }


    /// <summary>
    /// 後ろに流すオブジェクトを解除する
    /// </summary>
    /// <param name="obj"></param>
    public void Leave(Transform obj)
    {
        roads.Remove(obj);
    }

}
