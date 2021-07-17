using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数のトランスフォームを取得可能とする
/// </summary>
public interface ICanGetTransforms
{
    /// <summary>
    /// 複数のトランスフォームを取得する
    /// </summary>
    /// <returns></returns>
    IEnumerable<Transform> Transforms();
}

/// <summary>
/// プレイヤーの動きによって動かされるオブジェクトたちを管理する
/// </summary>
[RequireComponent(typeof(RoadManager))]
public class World : MonoBehaviour
{
    /// <summary>
    /// シングルトン
    /// </summary>
    public static World current;

    /// <summary>
    /// 後ろに流していくオブジェクト達
    /// </summary>
    readonly List<ICanGetTransforms> worldObjectLists = new List<ICanGetTransforms>();

    /// <summary>
    /// 動かすオブジェクトのリストを世界に登録する
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public void JoinWorld<T>(T a)
        where T : ICanGetTransforms
    {
        worldObjectLists.Add(a);
    }

    /// <summary>
    /// 世界をずらす
    /// </summary>
    /// <param name="move">ずらす座標</param>
    public void SwipeWorld(Vector3 move) => MoveWorlds(move);

    private void Awake()
    {
        current = this;
    }

    /// <summary>
    /// 世界を移動させる
    /// </summary>
    /// <param name="dist">移動量</param>
    /// <param name="time">時間を考慮するか</param>
    private void MoveWorlds(Vector3 dist, bool time = false)
    {
        if (time)
        {
            dist *= Time.deltaTime;
        }
        foreach (var item in worldObjectLists)
        {
            foreach (var obj in item.Transforms())
            {
                Vector3 pos = obj.transform.position;
                pos += dist;
                obj.transform.position = pos;
            }
        }
    }
}
