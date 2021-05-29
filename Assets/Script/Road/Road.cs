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
    /// 道路生成機
    /// </summary>
    [SerializeField]
    private RoadMaker roadMaker;


    /// <summary>
    /// 道路の生成限界距離(二乗)
    /// </summary>
    private float sqrObjDistance;

    /// <summary>
    /// ロードチップ達
    /// </summary>
    readonly List<RoadChip> roadChips = new List<RoadChip>();
    

    public List<RoadChip> GetRoadChips()
    {
        return roadChips;
    }


    private void Awake()
    {
        //最初の道路を設定しておく
        roadChips.Add(roadMaker.GetFirstRoadChip());
        RoadData data = Resources.Load("RoadData") as RoadData;
        float limit = data.LimitDistance;
        //距離計算用のメンバ変数の設定
        sqrObjDistance = limit * limit;
    }


    private void Update()
    {
        MakeNewRoads();
        DestroyOldRoads();
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
