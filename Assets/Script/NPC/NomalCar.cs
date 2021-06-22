using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NomalCar : MonoBehaviour
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
    public void Move(bool hasDistance = false,float distance = float.NaN)
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
                currentRoadChip = currentRoadChip.GetNext();
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
                this.transform.rotation = moveTo.rotation;
                break;
            }
        }
    }
}
