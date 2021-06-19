using System.Collections;
using System.Collections.Generic;
using System;
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
    /// 一番最初に生成されていた道路からどれぐらい違う方向を向いているか
    /// </summary>
    private float currentAngle = 0;

    /// <summary>
    /// ロードチップの各ステータス
    /// </summary>
    private RoadData roadData;
    /// <summary>
    /// ロードデザインの資料
    /// </summary>
    private RoadDesignDocument roadDesignDocument;

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
        //各すクリプタブルオブジェクトのロード
        roadData = Resources.Load("RoadData") as RoadData;
        roadDesignDocument = Resources.Load("RoadDesignDocument") as RoadDesignDocument;
        //はじめにあった道路を最新の道路として設定
        latestRoadChip = firstRoadChip;
        //はじめにあった道路を初期化
        latestRoadChip.Init(Vector3.zero, roadData.Length, roadData.Width, roadData.Lane);
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
        //角度設定
        DesignRoad();

        //自身の子オブジェクトとして道路を生成
        RoadChip maked = Instantiate(roadChipPrefab, this.transform);
        //道路の終端につなげる
        maked.transform.position = latestRoadChip.GetEnd().position;
        maked.transform.rotation = latestRoadChip.GetEnd().rotation;
        //道路を初期化(曲げる,ケツを設定する)
        maked.Init(new Vector3(0,chipRotate,0), roadData.Length, roadData.Width, roadData.Lane);
        //一個昔のロードチップに次をセットする
        latestRoadChip.SetNext(maked);
        //作った道路が終端となる
        latestRoadChip = maked;


        return maked;
    }

    /// <summary>
    /// 次の道路の角度を決定する
    /// </summary>
    private void DesignRoad()
    {
        //残りがない場合は新しい道路の設計を作り出す
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
                remaining -= Mathf.Abs(chipRotate);
                break;
            case RoadType.Winding:
                remaining -= roadData.Length;
                break;
            default:
                Debug.LogError("RoadTypeが正しくありません");
                break;
        }
        currentAngle += chipRotate;
    }

    /// <summary>
    /// 新しい道路を設計する
    /// </summary>
    private void MakeNextRoadState()
    {
        int a = Enum.GetValues(typeof(RoadType)).Length;
        int b = UnityEngine.Random.Range(0,a);

        currentRoadType = (RoadType)Enum.ToObject(typeof(RoadType), b);

        switch (currentRoadType)
        {
            case RoadType.Straight:
                MakeStraight();
                break;
            case RoadType.Curve:
                MakeCurve();
                break;
            case RoadType.Winding:
                MakeWinding();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 直線道路の設計
    /// </summary>
    private void MakeStraight()
    {
        remaining = UnityEngine.Random.Range
            (roadDesignDocument.StraightLengthMin,
            roadDesignDocument.StraightLengthMax);
    }
    /// <summary>
    /// カーブの設計
    /// </summary>
    private void MakeCurve()
    {
        remaining = UnityEngine.Random.Range
            (roadDesignDocument.CurveMin,
            roadDesignDocument.CurveMax);
        chipRotate = UnityEngine.Random.Range
            (roadDesignDocument.CurveStrengthMin,
            roadDesignDocument.CurveStrengthMax);
        if (currentAngle > 0)
        {
            chipRotate = -chipRotate;
        }
    }
    /// <summary>
    /// うねうね道路の設計(必要なさそうなのでとりあえず直線にしている)
    /// </summary>
    private void MakeWinding()
    {
        remaining = UnityEngine.Random.Range
            (roadDesignDocument.StraightLengthMin,
            roadDesignDocument.StraightLengthMax);
        chipRotate = 0;
    }
}
