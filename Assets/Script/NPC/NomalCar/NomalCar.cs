using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 一般車
/// </summary>
public class NomalCar : MonoBehaviour, ICar
{
    /// <summary>
    /// 現在いる箇所のロードチップ
    /// </summary>
    private RoadChip currentRoadChip;

    /// <summary>
    /// 走行中のレーン
    /// </summary>
    private int lane;

    /// <summary>
    /// スピード
    /// </summary>
    private float speedMS;

    /// <summary>
    /// 移動目標地点がNullのときに呼ばれる
    /// </summary>
    public event Action<NomalCar> OnRoadIsNull;

    /// <summary>
    /// 現在いる箇所のロードチップ
    /// </summary>
    public RoadChip CurrentRoadChip => currentRoadChip;

    public int Lane => lane;

    public int CurrentLane => lane;

    public float SpeedMS => speedMS;

    /// <summary>
    /// 生成時の初期設定を行う
    /// </summary>
    /// <param name="spawnPoint">スポーン箇所</param>
    /// <param name="lane">スポーンするレーン</param>
    /// <param name="speedMS">移動速度</param>
    public void Init(RoadChip spawnPoint, int lane, float speedMS)
    {
        //各種ステータスを設定
        currentRoadChip = spawnPoint;
        this.lane = lane;
        this.speedMS = speedMS;

        //車をスポーン位置に移動
        Transform spawn = currentRoadChip.GetLanePos(lane);
        this.transform.position = spawn.position;
        this.transform.rotation = spawn.rotation;
    }

    /// <summary>
    /// 車を移動させる
    /// </summary>
    /// <param name="hasDistance">移動距離を指定するか</param>
    /// <param name="distance">指定する移動距離</param>
    /// <param name="back">バックするか</param>
    public void Move(bool hasDistance = false, float distance = float.NaN, bool back = false)
    {

        if (!currentRoadChip)
        {
            OnRoadIsNull(this);
            return;
        }

        //移動距離を計算
        float moveDistance;
        if (hasDistance)
        {
            moveDistance = distance;
        }
        else
        {
            moveDistance = speedMS * Time.deltaTime;
        }

        //残り移動距離がゼロになるまで道路を進み続ける
        Transform moveTo = currentRoadChip.GetLanePos(lane);
        while (true)
        {
            float sqrLength = (this.transform.position - moveTo.position).sqrMagnitude;
            if (sqrLength <= moveDistance * moveDistance)
            {
                this.transform.position = moveTo.position;
                moveDistance -= Mathf.Sqrt(sqrLength);
                currentRoadChip = GetNextRoadChip(currentRoadChip, back);
                if (!currentRoadChip)
                {
                    //次がない場合は削除
                    OnRoadIsNull(this);
                    return;
                }
                //次の移動先を指定
                moveTo = currentRoadChip.GetLanePos(lane);
            }
            else
            {
                //移動距離が移動先に届かない場合は近づいて終了
                Vector3 dir = moveTo.position - this.transform.position;
                Vector3 pos = this.transform.position;
                this.transform.position = pos + dir.normalized * moveDistance;
                break;
            }
        }

        //車の角度を決定
        ChangeRotation();
    }

    private RoadChip GetNextRoadChip(RoadChip current, bool back)
    {
        current.Leave(this);
        current = back ? current.Prev : current.Next;
        if (current)
        {
            current.Join(this);
        }
        return current;
    }

    private void ChangeRotation()
    {
        Transform center;
        if (center = currentRoadChip.Center)
        {
            Vector3 vector = this.transform.position - center.position;
            //センターと自身の位置から、角度の計算
            float angle = Mathf.Atan2(vector.x, vector.z);
            Quaternion rotate = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);
            Quaternion rightLeft = currentRoadChip.IsCenterInRight ? Quaternion.AngleAxis(90, Vector3.up) : Quaternion.AngleAxis(-90, Vector3.up);
            this.transform.rotation = rotate * rightLeft;
        }
        else
        {
            this.transform.rotation = currentRoadChip.End.rotation;
        }
    }
}
