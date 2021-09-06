using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


/// <summary>
/// 敵車の本体にあたる部分
/// </summary>
public class EnemyCarBody : MonoBehaviour
{ 
    private EnemyCar parent;

    private int targetLane;

    private int currentLane;

    private int sarchChipCount;

    private float handle;

    /// <summary>
    /// 衝突リスクの高い順のレーン
    /// </summary>
    private int[] hitRisk;

    public int CurrentLane => currentLane;
    public float Handle => handle;

    public void Init(EnemyCar car)
    {
        parent = car;
        MakeData();
    }

    /// <summary>
    /// 車を左右に移動させる
    /// </summary>
    /// <returns>左右に移動した結果</returns>
    public float MoveBodyUpdate()
    {
        float before = this.transform.localPosition.x;
        ChangeTargetLane();
        MoveBodyLR();
        //現在いるレーンを計算する
        MakeCurrentLane();
        float after = this.transform.localPosition.x;
        return after - before;
    }

    /// <summary>
    /// 周りの車の走行情報から移動先レーンを決定
    /// </summary>
    private void ChangeTargetLane()
    {
        CheckHitRisk();
        //現在のレーンにリスクがある場合はレーン変更を検討する
        if (hitRisk[currentLane] != 0)
        {
            int currentRisk = hitRisk[currentLane];
            int rightRisk = RiskOf(currentLane - 1);
            int leftRisk = RiskOf(currentLane + 1);
            bool LR; //trueは左
            //左右のリスクが同じ場合は中央に寄るようにする
            if (rightRisk == leftRisk)
            {
                LR = 1.0f * (hitRisk.Length / 2) > currentLane;
            }
            else
            {
                LR = rightRisk > leftRisk;
            }
            if ((LR ? leftRisk : rightRisk) < currentRisk)
            {
                //レーン変更
                targetLane = LR ? currentLane + 1 : currentLane - 1;
            }
        }
    }

    private int RiskOf(int index)
    {
        if (0 <= index && index < hitRisk.Length)
        {
            return hitRisk[index];
        }
        else
        {
            return int.MaxValue;
        }
    }

    /// <summary>
    /// 前方の衝突リスクのあるレーンを探索する
    /// </summary>
    private void CheckHitRisk()
    {
        //配列を0で埋める
        for (int i = 0; i < hitRisk.Length; i++)
        {
            hitRisk[i] = 0;
        }

        RoadChip sarch = parent.CurrentRoadChip;
        
        int risk = hitRisk.Length - 1;
        //ステイターゲットとは違うレーンに行こうとする
        bool haveTarget = parent.StayTarget;
        //前方を探索
        for (int i = 0; i < sarchChipCount && sarch; i++)
        {
            //乗っている車を取得
            var cars = sarch.Cars;
            foreach (var item in cars)
            {
                if (item.SpeedMS * 1.2 > parent.SpeedMS)
                {
                    //相手の方が速いので無視
                    continue;
                }
                //車のいるレーン取得
                int lane = item.CurrentLane;
                //リスクが設定されていないレーンなら追加
                if (hitRisk[lane] == 0)
                {
                    hitRisk[lane] = risk;
                    risk--;
                    //追従目標がいた場合
                    if (haveTarget)
                    {
                        if (hitRisk[parent.StayTarget.CurrentLane] == 0)
                        {
                            hitRisk[parent.StayTarget.CurrentLane] = risk;
                            risk--;
                        }
                        haveTarget = false;
                    }
                }
                if (risk <= 0)
                {
                    goto BREAK;
                }
            }
            //次
            sarch = sarch.Next;
        }
    BREAK:;
    }

    private void MoveBodyLR()
    {
        float xPos = transform.localPosition.x;
        float xPosTo = StageDatabase.RoadData.LanePosOffsets[targetLane];
        float ChangeEndWidth = StageDatabase.RoadData.LaneWidth * parent.CarData.MovementData.LaneChangeEndWidth / 2;
        float halfWidth = StageDatabase.RoadData.LaneWidth / 2;
        bool front = false;
        if (xPosTo + halfWidth < xPos)
        {
            HandleToL();
        }
        else if (xPosTo - halfWidth > xPos)
        {
            HandleToR();
        }
        else
        {
            HandleToF();
            if (xPosTo + ChangeEndWidth < xPos || xPosTo - ChangeEndWidth > xPos)
            {
                front = true;
            }
        }
        Vector3 moved = Vector3.zero;
        float movedX = xPos + handle * Time.deltaTime;
        if (front)
        {
            if ((handle > 0 ^ xPosTo - movedX > 0) || handle == 0)
            {
                movedX = KMath.GetCloser(movedX, xPosTo, 1);
            }
        }
        moved.x = movedX;
        transform.localPosition = moved;
    }

    private void HandleToL()//L is Left
    {
        EnemyCarMovementData data = parent.CarData.MovementData;
        float power = data.HandlePower;
        if (handle > 0)
        {
            power *= 2;
        }
        handle = KMath.GetCloser(handle, -data.LaneChangePower, power);
    }
    private void HandleToR()//R is Right
    {
        EnemyCarMovementData data = parent.CarData.MovementData;
        float power = data.HandlePower;
        if (handle < 0)
        {
            power *= 2;
        }
        handle = KMath.GetCloser(handle, data.LaneChangePower, power);
    }
    private void HandleToF()//F is Front
    {
        EnemyCarMovementData data = parent.CarData.MovementData;
        handle = KMath.GetCloser(handle, 0, data.HandlePower);
    }

    private void MakeCurrentLane()
    {
        float xPos = transform.localPosition.x;
        float min = float.MaxValue;
        currentLane = 0;
        int lane = 0;
        foreach (var item in StageDatabase.RoadData.LanePosOffsets)
        {
            float a = Mathf.Abs(xPos - item);
            if (a < min)
            {
                min = a;
                currentLane = lane;
            }
            lane++;
        }
    }
    

    private void MakeData()
    {
        sarchChipCount = (int)(parent.CarData.MovementData.SarchLength / StageDatabase.RoadData.Length);
        hitRisk = new int[StageDatabase.RoadData.Lane];
    }
}
