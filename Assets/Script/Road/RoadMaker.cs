using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    RoadChip firstRoadChip;

    /// <summary>
    /// 最後に生成された道路
    /// </summary>
    RoadChip latestRoadChip;

    private void Awake()
    {
        //はじめにあった道路を最新の道路として設定
        latestRoadChip = firstRoadChip;
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
        //自身の子オブジェクトとして道路を生成
        RoadChip maked = Instantiate(roadChipPrefab, this.transform);
        //道路の終端につなげる
        maked.transform.position = latestRoadChip.GetEnd().position;
        //作った道路が終端となる
        latestRoadChip = maked;


        return maked;
    }
}
