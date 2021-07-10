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
    private Transform enemyCarContainer;

    /// <summary>
    /// 生成する敵車
    /// </summary>
    [SerializeField]
    private EnemyCar enemyCarPrefab;

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
    }

    private void Start()
    {
        //テスト用！敵をとりあえず一体スポーンさせる
        Spawn(road.GetRoadChips()[0], MathKoji.KmHToMS(200));
    }

    private void Update()
    {
        MoveCars();
    }

    /// <summary>
    /// 車を移動させる
    /// </summary>
    private void MoveCars()
    {
        foreach (var item in cars)
        {
            item.Move();
        }
        //道から溢れた車を削除
        DestroyBookedCars();
    }

    /// <summary>
    /// 指定した箇所に車をスポーンさせる
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="lane"></param>
    private EnemyCar Spawn(RoadChip spawnPoint, float speedMS)
    {
        EnemyCar maked = Instantiate(enemyCarPrefab, enemyCarContainer);
        maked.Init(spawnPoint, speedMS);
        cars.Add(maked);
        maked.OnRoadIsNull += DestroyBooking;
        return maked;
    }

    /// <summary>
    /// 車を削除予定のリストに格納しておく
    /// </summary>
    /// <param name="car">削除対象</param>
    private void DestroyBooking(EnemyCar car)
    {
        destroyBookingCars.Add(car);
    }

    /// <summary>
    /// 削除予定の車をすべて削除する
    /// </summary>
    private void DestroyBookedCars()
    {
        foreach (var item in destroyBookingCars)
        {
            cars.Remove(item);
            Destroy(item.gameObject);
        }
        destroyBookingCars.Clear();
    }
}
