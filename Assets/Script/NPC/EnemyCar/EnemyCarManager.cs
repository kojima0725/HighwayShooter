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
    /// 車はプレイヤーの位置からこの数後ろでスポーンする
    /// </summary>
    private int toSpawnCount;

    /// <summary>
    /// 生成した敵車
    /// </summary>
    readonly List<EnemyCar> cars = new List<EnemyCar>();

    /// <summary>
    /// 削除予定の車たち
    /// </summary>
    readonly List<EnemyCar> destroyBookingCars = new List<EnemyCar>();

    private float enemySpawnTimer;

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
        MakeData();
        enemySpawnTimer = StageDatabase.EnemyCarSpawnData.FirstIntervalTime;
    }

    private void Update()
    {
        MoveCars();
        EnemySpawnUpdate();
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
    /// 敵の生成管理
    /// </summary>
    private void EnemySpawnUpdate()
    {
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer < 0)
        {
            if (cars.Count < StageDatabase.EnemyCarSpawnData.MaxCount)
            {
                RoadChip spawnTo = road.GetPlayerRoadChip();
                int count = 0;
                while (count <= toSpawnCount)
                {
                    if (spawnTo.Prev is null)
                    {
                        break;
                    }
                    spawnTo = spawnTo.Prev;
                    count++;
                }
                Spawn(spawnTo, PlayerCar.current.SpeedMS + StageDatabase.EnemyCarSpawnData.FirstAddSpeedMS);
            }
            enemySpawnTimer = StageDatabase.EnemyCarSpawnData.IntervalTime;
        }
    }


    /// <summary>
    /// 指定した箇所に車をスポーンさせる
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="lane"></param>
    private EnemyCar Spawn(RoadChip spawnPoint, float speedMS)
    {
        //敵車のリストからランダムに生成
        EnemyCarData[] carList = StageDatabase.CarTypesData.Enemys;
        EnemyCar maked = carList[Random.Range(0,carList.Length)].GenerateEnemyCar();
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


    private void MakeData()
    {
        toSpawnCount = (int)(StageDatabase.EnemyCarSpawnData.SpawnBackLength / StageDatabase.RoadData.Length);
    }
}
