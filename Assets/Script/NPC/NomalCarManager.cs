using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 道路上の一般車の生成、移動、削除を管理する
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
            Spawn(road.GetRoadChips().Last(), Random.Range(0,roadData.Lane), carData.SpeedMS);
        }

        SpawnCarsInFront();
        SpawnCarsInBack();
    }

    /// <summary>
    /// 道路の前方の空いてる部分に車を生成していく
    /// </summary>
    private void SpawnCarsInFront()
    {
        int count = 0;
        NomalCar car = cars.Last();
        RoadChip chip = car.CurrentRoadChip;
        while (true)
        {
            while (count != toSpawnCount && chip)
            {
                chip = chip.Next;
                count++;
            }
            if (count == toSpawnCount)
            {
                //一個前の車とレーンがかぶらないようにレーンを決定
                int spawnLane = Random.Range(0,roadData.Lane - 1);
                if (spawnLane >= car.Lane)
                {
                    spawnLane++;
                }
                //車をスポーン
                car = Spawn(car.CurrentRoadChip, spawnLane, carData.SpeedMS);
                //所定の位置まで移動
                car.Move(true, Random.Range(carData.BetweenMin, carData.BetweenMax));
                //各数値をリセット
                count = 0;
                chip = car.CurrentRoadChip;
            }
            else
            {
                //生成限界に達したのでreturn
                break;
            }
        }
    }

    /// <summary>
    /// 道路の後方の開いてる部分に車を生成していく
    /// </summary>
    private void SpawnCarsInBack()
    {
        int count = 0;
        NomalCar car = cars.First();
        RoadChip chip = car.CurrentRoadChip;
        while (true)
        {
            while (count != toSpawnCount && chip)
            {
                chip = chip.Prev;
                count++;
            }
            if (count == toSpawnCount)
            {
                //一個前の車とレーンがかぶらないようにレーンを決定
                int spawnLane = Random.Range(0, roadData.Lane - 1);
                if (spawnLane >= car.Lane)
                {
                    spawnLane++;
                }
                //車をスポーン
                car = Spawn(car.CurrentRoadChip, spawnLane, carData.SpeedMS, true);
                //所定の位置まで移動
                car.Move(true, Random.Range(carData.BetweenMin, carData.BetweenMax), true);
                //各数値をリセット
                count = 0;
                chip = car.CurrentRoadChip;
            }
            else
            {
                //生成限界に達したのでreturn
                break;
            }
        }
    }

    /// <summary>
    /// 指定した箇所に車をスポーンさせる
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="lane"></param>
    private NomalCar Spawn(RoadChip spawnPoint, int lane, float speedMS, bool insert = false)
    {
        NomalCar maked = Instantiate(npcCarPrefab, nomalCarContainer);
        maked.Init(spawnPoint, lane, speedMS);
        if (insert)
        {
            cars.Insert(0,maked);
        }
        else
        {
            cars.Add(maked);
        }
        maked.OnRoadIsNull += DestroyBooking;
        return maked;
    }

    /// <summary>
    /// 車を削除予定のリストに格納しておく
    /// </summary>
    /// <param name="car">削除対象</param>
    private void DestroyBooking(NomalCar car)
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
        toSpawnCount = (int)(carData.CarSpawnLength / roadData.Length);
    }
}
