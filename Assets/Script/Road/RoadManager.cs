using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 道路及び世界を後ろに流していく
/// </summary>
[RequireComponent(typeof(RoadMaker))]
public class RoadManager : MonoBehaviour, ICanGetTransforms
{
    /// <summary>
    /// 道路生成機
    /// </summary>
    [SerializeField]
    private RoadMaker roadMaker;

    /// <summary>
    /// 世界
    /// </summary>
    [SerializeField]
    private World world;


    /// <summary>
    /// 道路の生成限界距離(二乗)
    /// </summary>
    private float sqrObjDistance;

    /// <summary>
    /// ロードチップ達
    /// </summary>
    readonly List<RoadChip> roadChips = new List<RoadChip>();

    /// <summary>
    /// ロードチップのリストを返す(リストの先頭が道路の先頭、リスト末尾が道路の末端)
    /// </summary>
    /// <returns></returns>
    public List<RoadChip> GetRoadChips() => roadChips;

    
    public IEnumerable<Transform> Transforms()
    {
        foreach (var item in roadChips)
        {
            yield return item.transform;
        }
    }

    /// <summary>
    /// プレイヤーが今どのロードチップの近くにいるかを返す(少し重い)
    /// </summary>
    /// <returns></returns>
    public RoadChip GetPlayerRoadChip()
    {
        RoadChip min = null;
        float minMag = 0;
        foreach (var item in roadChips)
        {
            if (min is null)
            {
                min = item;
                minMag = item.transform.position.sqrMagnitude;
            }
            else
            {
                float mag = item.transform.position.sqrMagnitude;
                if (mag < minMag)
                {
                    min = item;
                    minMag = mag;
                }
            }
        }

        return min;
    }

    /// <summary>
    /// ガードレールと線があたっているかどうか調べる
    /// </summary>
    /// <param name="LR">true=左, false=右</param>
    /// <returns></returns>
    public bool GurdrailHitCheck(bool LR, Vector2Line line, out GurdrailHit hit)
    {
        RoadChip chip = roadChips.First();
        Vector2Line rail = new Vector2Line();
        Vector2 hitPos;
        while (chip.Next)
        {
            rail.start = chip.Gurdrail(LR).position;
            rail.end = chip.Next.Gurdrail(LR).position;
            if (KMath.LineToLineCollision(line, rail, out hitPos))
            {
                hit = new GurdrailHit(rail, chip.Next, hitPos);
                return true;
            }
        }
        hit = new GurdrailHit();
        return false;
    }


    private void Awake()
    {
        //最初の道路を設定しておく
        roadChips.Add(roadMaker.GetFirstRoadChip());
        //距離計算用のメンバ変数の設定
        float limit = StageDatabase.RoadData.LimitDistance;
        sqrObjDistance = limit * limit;

        world.JoinWorld(this);
    }


    private void Update()
    {
        //新しい道路を生成
        MakeNewRoads();
        //古い道路を削除
        DestroyOldRoads();
    }

    

    /// <summary>
    /// 道をどんどん生成していく
    /// </summary>
    private void MakeNewRoads()
    {
        //生成距離が限界に達するまで、道路を生成する
        while (sqrObjDistance > roadMaker.GetLatestRoadChip().transform.position.sqrMagnitude)
        {
            foreach (var item in roadMaker.MakeRoads())
            {
                roadChips.Add(item);
            }
        }
    }

    /// <summary>
    /// 生存距離を超えた道路を削除していく
    /// </summary>
    private void DestroyOldRoads()
    {
        RoadChip a;
        a = roadChips.First();
        while (sqrObjDistance < a.transform.position.sqrMagnitude)
        {
            roadChips.RemoveAt(0);
            Destroy(a.gameObject);
            a = roadChips.First();
        }
    }


}
