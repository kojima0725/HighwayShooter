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
    /// シングルトン
    /// </summary>
    public static RoadManager current;

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
    private float sqrDeadDistance;

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
        RoadChip min = roadChips.First();
        float minMag = min.transform.position.sqrMagnitude;
        foreach (var item in roadChips)
        {
            float mag = item.transform.position.sqrMagnitude;
            if (mag < minMag)
            {
                min = item;
                minMag = mag;
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
        RoadChip chip = roadChips.FirstOrDefault();
        if (!chip)
        {
            hit = new GurdrailHit();
            return false;
        } 

        while (chip.Next != null)
        {
            Vector2Line rail = new Vector2Line(
                new Vector2(chip.Gurdrail(LR).position.x, chip.Gurdrail(LR).position.z), 
                new Vector2(chip.Next.Gurdrail(LR).position.x, chip.Next.Gurdrail(LR).position.z)
                );
            Vector2 hitPos;
            if (KMath.LineToLineCollision(line, rail, out hitPos))
            {
                hit = new GurdrailHit(rail, chip.Next, hitPos);
                return true;
            }
            chip = chip.Next;
        }


        hit = new GurdrailHit();
        return false;
    }

    private void WriteGurdrail()
    {
        RoadChip chip = roadChips.FirstOrDefault();
        if (!chip)
        {
            return;
        }
        while (chip.Next)
        {
            Debug.DrawLine(chip.Gurdrail(true).position + Vector3.up, chip.Next.Gurdrail(true).position + Vector3.up, Color.red);
            Debug.DrawLine(chip.Gurdrail(false).position + Vector3.up, chip.Next.Gurdrail(false).position + Vector3.up, Color.red);
            chip = chip.Next;
        }

    }


    private void Awake()
    {
        current = this;
        //最初の道路を設定しておく
        roadChips.Add(roadMaker.GetFirstRoadChip());
        //距離計算用のメンバ変数の設定
        float limit = StageDatabase.RoadData.LimitDistance;
        sqrObjDistance = limit * limit;
        limit = StageDatabase.RoadData.DeathDistance;
        sqrDeadDistance = limit * limit;

        world.JoinWorld(this);
    }


    private void Update()
    {
        //新しい道路を生成
        MakeNewRoads();
        //古い道路を削除
        DestroyOldRoads();
        //デバッグ用のガードレイル描画
        WriteGurdrail();
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
        a = roadChips.FirstOrDefault();
        while (sqrDeadDistance < a.transform.position.sqrMagnitude)
        {
            roadChips.RemoveAt(0);
            Destroy(a.gameObject);
            a = roadChips.FirstOrDefault();
        }
    }


}
