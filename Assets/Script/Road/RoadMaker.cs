using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadType
{
    Straight, //直線
    Curve, //カーブ
    Winding, //うねうね曲がりくねった道
}


/// <summary>
/// 道路生成器
/// </summary>
public class RoadMaker : MonoBehaviour
{
    /// <summary>
    /// 生成される道路のプレファブ
    /// </summary>
    [SerializeField]
    private RoadChip roadChipPrefab;

    /// <summary>
    /// 最初の道路(シーン上に道路の始まりを設置し、ここに設定しておく必要がある)
    /// </summary>
    [SerializeField]
    private RoadChip firstRoadChip;

    /// <summary>
    /// ロードチップの各ステータス
    /// </summary>
    private RoadData roadData;

    /// <summary>
    /// 現在の道路のステータス
    /// </summary>
    private RoadType currentRoadType;

    /// <summary>
    /// チップ生成時にどれぐらい道を曲げるか
    /// </summary>
    private float chipRotate;

    /// <summary>
    /// 生成の残り(角度や距離など、生成する道路の種類によって扱いは異なる)
    /// </summary>
    private float remaining;

    /// <summary>
    /// 最後に生成された道路
    /// </summary>
    private RoadChip latestRoadChip;

    private void Awake()
    {
        //ロードチップのデータを取得
        roadData = Resources.Load("RoadData") as RoadData;
        //はじめにあった道路を最新の道路として設定
        latestRoadChip = firstRoadChip;
        //ゲーム開始直後は直線の道路を生成する
        currentRoadType = RoadType.Straight;
        //直線道路は100メートル作る
        remaining = 100;
    }

    /// <summary>
    /// シーン開始時にはじめに存在していた道路を渡す
    /// </summary>
    /// <returns></returns>
    public RoadChip GetFirstRoadChip()
    {
        return firstRoadChip;
    }

    /// <summary>
    /// 最後に生成された道路を渡す
    /// </summary>
    /// <returns></returns>
    public RoadChip GetLatestRoadChip()
    {
        return latestRoadChip;
    }

    /// <summary>
    /// 道路を生成する
    /// </summary>
    /// <returns>生成した道路</returns>
    public RoadChip MakeRoad()
    {

        DesignRoad();

        //自身の子オブジェクトとして道路を生成
        RoadChip maked = Instantiate(roadChipPrefab, this.transform);
        //道路の終端につなげる
        maked.transform.position = latestRoadChip.GetEnd().position;
        maked.transform.rotation = latestRoadChip.GetEnd().rotation;
        //道路が曲がる
        maked.transform.Rotate(new Vector3(0,chipRotate,0));
        //作った道路が終端となる
        latestRoadChip = maked;


        return maked;
    }

    /// <summary>
    /// 道路の道順を設計する
    /// </summary>
    private void DesignRoad()
    {
        if (remaining <= 0)
        {
            MakeNextRoadState();
        }

        switch (currentRoadType)
        {
            case RoadType.Straight:
                chipRotate = 0;
                remaining -= roadData.Length;
                break;
            case RoadType.Curve:
                remaining -= chipRotate;
                break;
            case RoadType.Winding:
                remaining -= roadData.Length;
                break;
            default:
                break;
        }
    }

    private void MakeNextRoadState()
    {

    }
}
