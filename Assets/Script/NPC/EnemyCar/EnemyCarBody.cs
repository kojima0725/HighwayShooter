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

    private int sarchChipCount;

    /// <summary>
    /// 衝突リスクの高い順のレーン
    /// </summary>
    private int[] hitRisk;

    public int TargetLane => targetLane;

    public void Init(EnemyCar car)
    {
        parent = car;
        MakeData();
    }

    /// <summary>
    /// 車を左右に移動させる
    /// </summary>
    /// <returns></returns>
    public float MoveBodyUpdate()
    {
        float before = this.transform.localPosition.x;
        ChangeTargetLane();
        MoveBodyLR();
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
        if (hitRisk[targetLane] != 0)
        {
            int currentRisk = hitRisk[targetLane];
            int rightRisk = RiskOf(targetLane - 1);
            int leftRisk = RiskOf(targetLane + 1);
            bool LR; //trueは左
            //左右のリスクが同じ場合は中央に寄るようにする
            if (rightRisk == leftRisk)
            {
                LR = 1.0f * (hitRisk.Length / 2) > targetLane;
            }
            else
            {
                LR = rightRisk > leftRisk;
            }
            if ((LR ? leftRisk : rightRisk) < currentRisk)
            {
                //レーン変更
                targetLane = LR ? targetLane + 1 : targetLane - 1;
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
        sarch = sarch.Prev ? sarch.Prev : sarch;
        
        int risk = hitRisk.Length - 1;
        //前方を探索
        for (int i = 0; i < sarchChipCount && sarch; i++)
        {
            //乗っている車を取得
            var cars = sarch.Cars;
            foreach (var item in cars)
            {
                //車のいるレーン取得
                int lane = item.CurrentLane;
                bool haveRisk = false;
                //リスクが設定されていないレーンなら追加
                if (hitRisk[lane] == 0)
                {
                    hitRisk[lane] = risk;
                    haveRisk = true;
                }
                if (haveRisk)
                {
                    risk--;
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
        Vector3 pos = Vector3.zero;
        pos.x = StageDatabase.RoadData.LanePosOffsets[targetLane];
        this.transform.localPosition = pos;
    }
    

    private void MakeData()
    {
        sarchChipCount = (int)(parent.CarData.MovementData.SarchLength / StageDatabase.RoadData.Length);
        hitRisk = new int[StageDatabase.RoadData.Lane];
    }
}
