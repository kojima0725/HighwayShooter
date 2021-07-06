using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雑魚敵の生成、移動、削除を管理する
/// </summary>
[RequireComponent(typeof(Road))]
public class EnemyCarManager : MonoBehaviour,ICanGetTransforms
{
    [SerializeField]
    private Road road;
    [SerializeField]
    private World world;

    /// <summary>
    /// 生成した車をこのオブジェクトの下に格納する
    /// </summary>
    [SerializeField]
    private Transform nomalCarContainer;

    /// <summary>
    /// 生成する敵車
    /// </summary>
    [SerializeField]
    private EnemyCar enemyCarPrefab;

    /// <summary>
    /// 道路のデータ
    /// </summary>
    private RoadData roadData;

    /// <summary>
    /// 生成した敵車
    /// </summary>
    readonly List<EnemyCar> cars = new List<EnemyCar>();

    /// <summary>
    /// 削除予定の車たち
    /// </summary>
    readonly List<EnemyCar> destroyBookingCars = new List<EnemyCar>();

    public IEnumerable<Transform> Transforms()
    {
        foreach (var item in cars)
        {
            yield return item.transform;
        }
    }

    private void Awake()
    {
        world.JoinWorld(this);
        //道路のデータを読み込み
        roadData = Resources.Load("RoadData") as RoadData;
    }
}
