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

    private bool IsCurveToRight;

    private void Awake()
    {
        //はじめにあった道路を最新の道路として設定
        latestRoadChip = firstRoadChip;
        //はじめにあった道路を初期化
        latestRoadChip.Init(Vector3.zero, StageDatabase.RoadData.Length, StageDatabase.RoadData.Width, StageDatabase.RoadData.Lane);
        //ゲーム開始直後は直線の道路を生成する
        currentRoadType = RoadType.Straight;
        //最初は直線道路を500メートル作る
        remaining = 500;
    }

    /// <summary>
    /// シーン開始時にはじめに存在していた道路を渡す
    /// </summary>
    /// <returns></returns>
    public RoadChip GetFirstRoadChip() => firstRoadChip;

    /// <summary>
    /// 最後に生成された道路を渡す
    /// </summary>
    /// <returns></returns>
    public RoadChip GetLatestRoadChip() => latestRoadChip;

    /// <summary>
    /// 道路を生成する
    /// </summary>
    /// <returns>生成した道路</returns>
    public IEnumerable<RoadChip> MakeRoads()
    {
        RoadChip start = latestRoadChip;
        RoadChip end = null;
        Transform center = null;
        bool isCurve = currentRoadType == RoadType.Curve;
        //カーブの場合は中心点を予め生成
        if (isCurve)
        {
            //道路がカーブの場合は円の中心点を設定する
            center = new GameObject().transform;
        }
        //道路生成
        while (remaining >= 0)
        {
            //自身の子オブジェクトとして道路を生成
            RoadChip maked = MakeChip(center);
            
            //現在制作中の道がどれぐらいで終わるか計算する
            DesignRoad();

            end = maked;
            yield return maked;
        }
        //カーブの場合は中心点の座標を計算
        if (isCurve)
        {
            MakeCenterPos(center, start, end);
            //生成した道路の終端に格納する(一緒に削除されるように)
            center.parent = end.transform;
        }

        //道路が設計の終端に達したため新しい設計を作成する
        MakeNextRoadState();
    }

    /// <summary>
    /// 次の
    /// </summary>
    private void DesignRoad()
    {
        switch (currentRoadType)
        {
            case RoadType.Straight:
                remaining -= StageDatabase.RoadData.Length;
                break;
            case RoadType.Curve:
                remaining -= Mathf.Abs(chipRotate);
                break;
            case RoadType.Winding:
                remaining -= StageDatabase.RoadData.Length;
                break;
            default:
                Debug.LogError("RoadTypeが正しくありません");
                break;
        }
        currentAngle += chipRotate;
    }

    /// <summary>
    /// 次に作る道路の形を決定する
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
        //距離設定
        remaining = UnityEngine.Random.Range
            (StageDatabase.RoadDesignDocument.StraightLengthMin,
            StageDatabase.RoadDesignDocument.StraightLengthMax);
        //曲がる角度はゼロ
        chipRotate = 0;
    }
    /// <summary>
    /// カーブの設計
    /// </summary>
    private void MakeCurve()
    {
        remaining = UnityEngine.Random.Range
            (StageDatabase.RoadDesignDocument.CurveMin,
            StageDatabase.RoadDesignDocument.CurveMax);
        chipRotate = UnityEngine.Random.Range
            (StageDatabase.RoadDesignDocument.CurveStrengthMin,
            StageDatabase.RoadDesignDocument.CurveStrengthMax);
        //常に道路が前に進み続けるようにしている
        if (currentAngle > 0)
        {
            chipRotate = -chipRotate;
            IsCurveToRight = false;
        }
        else
        {
            IsCurveToRight = true;
        }
    }
    /// <summary>
    /// うねうね道路の設計(使わないので直線にしている)
    /// </summary>
    private void MakeWinding()
    {
        //距離設定
        remaining = UnityEngine.Random.Range
            (StageDatabase.RoadDesignDocument.StraightLengthMin,
            StageDatabase.RoadDesignDocument.StraightLengthMax);
        //曲がる角度はゼロ
        chipRotate = 0;
    }

    /// <summary>
    /// ロードチップを作成する
    /// </summary>
    /// <param name="center">カーブ時の道路が描く円の中心点</param>
    /// <returns>作成したチップ</returns>
    private RoadChip MakeChip(Transform center)
    {
        //自身の子オブジェクトとして道路を生成
        RoadChip maked = Instantiate(roadChipPrefab, this.transform);
        //道路の終端につなげる
        maked.transform.position = latestRoadChip.End.position;
        maked.transform.rotation = latestRoadChip.End.rotation;
        //道路を初期化(曲げる,ケツを設定する)
        maked.Init(new Vector3(0, chipRotate, 0),
                   StageDatabase.RoadData.Length, StageDatabase.RoadData.Width,
                   StageDatabase.RoadData.Lane, latestRoadChip);
        //中心点を設定
        maked.Center = center;
        //中心点がどちら側にあるかを設定
        maked.IsCenterInRight = IsCurveToRight;
        //一個昔のロードチップに次をセットする
        latestRoadChip.Next = maked;
        //作った道路が終端となる
        latestRoadChip = maked;

        return maked;
    }

    /// <summary>
    /// カーブの始まりと終わりの座標から、円の中心点を算出する
    /// </summary>
    /// <param name="center">座標を適用するオブジェクト</param>
    /// <param name="start">カーブの始まり</param>
    /// <param name="end">カーブの終わり</param>
    private void MakeCenterPos(Transform center, RoadChip start, RoadChip end)
    {
        //中心点の座標を計算
        Transform startPos = start.End;
        Vector3 l1s = startPos.position + startPos.right * StageDatabase.RoadDesignDocument.CenterCheckLength;
        Vector3 l1e = startPos.position - startPos.right * StageDatabase.RoadDesignDocument.CenterCheckLength;
        Transform endPos = end.End;
        Vector3 l2s = endPos.position + endPos.right * StageDatabase.RoadDesignDocument.CenterCheckLength;
        Vector3 l2e = endPos.position - endPos.right * StageDatabase.RoadDesignDocument.CenterCheckLength;


        Vector2 l1s2 = new Vector2(l1s.x, l1s.z);
        Vector2 l1e2 = new Vector2(l1e.x, l1e.z);
        Vector2 l2s2 = new Vector2(l2s.x, l2s.z);
        Vector2 l2e2 = new Vector2(l2e.x, l2e.z);

        Vector2 hit;
        if (MathKoji.LineToLineCollision(l1s2, l1e2, l2s2, l2e2, out hit))
        {
            center.position = new Vector3(hit.x, 0, hit.y);
        }
        else
        {
            Debug.LogError("NO HIT!!!(円の中心座標算出時)");
        }
    }
}
