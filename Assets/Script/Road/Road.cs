using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 道路及び世界を後ろに流していく
/// </summary>
[RequireComponent(typeof(RoadMaker))]
public class Road : MonoBehaviour
{
    /// <summary>
    /// シングルトン
    /// </summary>
    public static Road current;

    /// <summary>
    /// 道路生成機
    /// </summary>
    [SerializeField]
    private RoadMaker roadMaker;

    /// <summary>
    /// 生成物の生存距離
    /// </summary>
    [SerializeField]
    private float objDistance;

    /// <summary>
    /// 生成物の生存距離(二乗)
    /// </summary>
    private float sqrObjDistance;

    #region 移動速度及び方向関連のメンバ変数

    /// <summary>
    /// 速度(時速何キロメートルか)
    /// </summary>
    private float speedKmH = 0;

    /// <summary>
    /// 速度(秒速何メートルか)
    /// </summary>
    private float speedMS;

    /// <summary>
    /// 移動方向
    /// </summary>
    private Vector3 moveAxis = new Vector3(0,0,-1);

    #endregion


    /// <summary>
    /// 後ろに流していくオブジェクト達
    /// </summary>
    readonly List<Transform> roadObjects = new List<Transform>();

    /// <summary>
    /// 道本体は別物として扱う
    /// </summary>
    readonly List<RoadChip> roadChips = new List<RoadChip>();



    /// <summary>
    /// 道路のスピードをセットする
    /// </summary>
    /// <param name="KmH"></param>
    public void SetCarSpeed(float KmH)
    {
        speedKmH = KmH;
        speedMS = MathKoji.KmHToMS(speedKmH);
    }

    /// <summary>
    /// 世界の移動方向を設定する
    /// </summary>
    /// <param name="axis"></param>
    public void SetMoveAxis(Vector3 axis)
    {
        moveAxis = axis.normalized;
    }

    /// <summary>
    /// 世界の移動方向を確認する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveAxis()
    {
        return moveAxis;
    }


    private void Awake()
    {
        //自身をどこからでもアクセスできるように設定
        current = this;
        //最初の道路を設定しておく
        roadChips.Add(roadMaker.GetFirstRoadChip());
        //距離計算用のメンバ変数の設定
        sqrObjDistance = objDistance * objDistance;
    }


    private void Update()
    {
        MakeNewRoads();
        DestroyOldRoads();
    }

    private void LateUpdate()
    {
        MoveObjects();
    }

    /// <summary>
    /// 所属しているオブジェクトを後ろに流していく
    /// </summary>
    private void MoveObjects()
    {
        Vector3 dist = moveAxis * speedMS * Time.deltaTime;
        foreach (var item in roadObjects)
        {
            Vector3 pos = item.position;
            pos += dist;
            item.position = pos;
        }
        foreach (var item in roadChips)
        {
            Vector3 pos = item.transform.position;
            pos += dist;
            item.transform.position = pos;
        }
    }

    /// <summary>
    /// 道をどんどん生成していく
    /// </summary>
    private void MakeNewRoads()
    {
        int count = 100;
        //生成距離が限界に達するまで、道路を生成する
        while (sqrObjDistance > roadMaker.GetLatestRoadChip().transform.position.sqrMagnitude)
        {
            roadChips.Insert(0, roadMaker.MakeRoad());
            count--;
            if (count == 0)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 生存距離を超えた道路を削除していく
    /// </summary>
    private void DestroyOldRoads()
    {
        RoadChip a;
        a = roadChips.Last();
        while (sqrObjDistance < a.transform.position.sqrMagnitude)
        {
            roadChips.RemoveAt(roadChips.Count - 1);
            Destroy(a.gameObject);
            a = roadChips.Last();
        }
    }


}
