using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 道路上の一般車の生成、削除等を管理する
/// </summary>
[RequireComponent(typeof(Road))]
public class NomalCarManager : MonoBehaviour, ICanGetTransforms
{
    [SerializeField]
    private Road road;
    [SerializeField]
    private World world;

    /// <summary>
    /// 生成した車をこのゲームオブジェクトの下に格納する
    /// </summary>
    [SerializeField]
    private Transform nomalCarContainer;

    /// <summary>
    /// 生成する車
    /// </summary>
    [SerializeField]
    private NomalCar npcCarPrefab;

    /// <summary>
    /// 一般車に関するデータ
    /// </summary>
    private NomalCarData carData;

    /// <summary>
    /// 道路のデータ
    /// </summary>
    private RoadData roadData;

    /// <summary>
    /// 車がスポーンするまでの敷居のロードチップ数
    /// </summary>
    private int toSpawnCount;

    /// <summary>
    /// 生成した車たち
    /// </summary>
    readonly List<NomalCar> cars = new List<NomalCar>();

    /// <summary>
    /// 削除予定の車たち
    /// </summary>
    readonly List<NomalCar> destroyBookingCars = new List<NomalCar>();

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
        //生成する車のデータを読み込み
        carData = Resources.Load("NomalCarData") as NomalCarData;
        //道路のデータを読み込み
        roadData = Resources.Load("RoadData") as RoadData;
        //各種数値情報の設定
        MakeData();
    }

    private void Update()
    {
        MoveCars();
        SpawnCars();
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
    /// 道路の開いてる部分に車をスポーンさせていく
    /// </summary>
    private void SpawnCars()
    {
        if (cars.Count == 0)
        {
            //車が一台もいない場合はとりあえずスポーン
            Spawn(road.GetRoadChips()[0], Random.Range(0,roadData.Lane), carData.SpeedMS);
        }
    }

    /// <summary>
    /// 指定した箇所に車をスポーンさせる
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="lane"></param>
    private void Spawn(RoadChip spawnPoint, int lane, float speedMS)
    {
        NomalCar maked = Instantiate(npcCarPrefab, nomalCarContainer);
        maked.Init(spawnPoint, lane, speedMS);
        cars.Add(maked);
        maked.OnRoadIsNull += DestroyBooking;
    }

    /// <summary>
    /// 車を削除予定のリストに格納しておく
    /// </summary>
    /// <param name="car">削除対象</param>
    private void DestroyBooking(NomalCar car)
    {
        cars.Remove(car);
        Destroy(car.gameObject);
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
        toSpawnCount = (int)(carData.CarSpawnLength / roadData.Length);
    }
}
