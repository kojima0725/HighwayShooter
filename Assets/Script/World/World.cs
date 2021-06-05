using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの動きによって動かされるオブジェクトたちを管理する
/// </summary>
[RequireComponent(typeof(Road))]
public class World : MonoBehaviour
{
    /// <summary>
    /// シングルトン
    /// </summary>
    public static World current;

    /// <summary>
    /// 道路
    /// </summary>
    [SerializeField]
    private Road road;


    /// <summary>
    /// 後ろに流していくオブジェクト達
    /// </summary>
    readonly List<Transform> WorldObjects = new List<Transform>();

    private List<RoadChip> roadChips;

    #region 移動速度及び方向関連の変数及び関数

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
    private Vector3 moveAxis = new Vector3(0, 0, -1);

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

    #endregion

    /// <summary>
    /// 世界をずらす
    /// </summary>
    /// <param name="move">ずらす座標</param>
    public void SwipeWorld(Vector3 move)
    {
        MoveWorlds(move);
    }

    private void Awake()
    {
        current = this;
        if (road)
        {
            roadChips = road.GetRoadChips();
        }
    }

    private void LateUpdate()
    {
        //車の速度、方向に応じて世界を移動させる
        MoveWorlds(moveAxis * speedMS, true);
    }




    /// <summary>
    /// 世界を移動させる
    /// </summary>
    /// <param name="dist">移動量</param>
    /// <param name="time">時間を考慮するか</param>
    private void MoveWorlds(Vector3 dist, bool time = false)
    {
        if (time)
        {
            dist *= Time.deltaTime;
        }
        foreach (var item in WorldObjects)
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
}
