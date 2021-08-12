using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 敵の車
/// </summary>
public class EnemyCar : MonoBehaviour , ICar
{
    /// <summary>
    /// 移動先がNullのときに呼ばれる
    /// </summary>
    public event Action<EnemyCar> OnRoadIsNull;

    private RoadChip currentRoadChip;

    /// <summary>
    /// スピード
    /// </summary>
    private float speedMS;

    private EnemyCarData myData;

    [SerializeField]
    private EnemyCarBody body;

    public int CurrentLane => body.CurrentLane;

    public float SpeedMS => speedMS;

    /// <summary>
    /// 現在いる箇所のロードチップ
    /// </summary>
    public RoadChip CurrentRoadChip => currentRoadChip;

    public void SetEnemyCarData(EnemyCarData data) => myData = data;

    public EnemyCarData CarData => myData;

    /// <summary>
    /// 生成時の初期設定を行う
    /// </summary>
    /// <param name="spawnPoint">スポーン箇所</param>
    /// <param name="speedMS">移動速度</param>
    public void Init(RoadChip spawnPoint, float speedMS)
    {
        //各種ステータスを設定
        currentRoadChip = spawnPoint;
        this.speedMS = speedMS;

        //車をスポーン位置に移動
        Transform spawn = currentRoadChip.End;
        this.transform.position = spawn.position;
        this.transform.rotation = spawn.rotation;

        //ボディの初期化
        body.Init(this);
    }

    /// <summary>
    /// 車を移動させる
    /// </summary>
    /// <param name="hasDistance">移動距離を指定するか</param>
    /// <param name="distance">指定する移動距離</param>
    /// <param name="back">バックするか</param>
    public void Move(bool hasDistance = false, float distance = float.NaN, bool back = false)
    {
        ChangeSpeed();
        MoveBase(hasDistance, distance, back);
        float lr = body.MoveBodyUpdate();
        Vector3 direction = Vector3.forward * speedMS * Time.deltaTime + Vector3.right * lr * 2;
        body.transform.localRotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// プレイヤーとの相対位置応じて加減速を行う
    /// </summary>
    private void ChangeSpeed()
    {
        Vector3 thisPos = body.transform.position;
        float sqrDist = thisPos.sqrMagnitude;
        float min = myData.MovementData.StayLengthMin;
        if (sqrDist < min * min)
        {
            //距離が近すぎる場合は加速
            SpeedUp();
            return;
        }
        float angle = Vector3.Angle(PlayerCar.current.Body.forward, thisPos);
        if (angle > myData.MovementData.StayAngle / 2)
        {
            //プレイヤーの視界の外にいる場合は加速
            SpeedUp();
            return;
        }
        float max = myData.MovementData.StayLengthMax;
        if (sqrDist < max * max)
        {
            //視野内、一定の距離内にいる場合は速度維持して並走
            SpeedKeep();
            return;
        }
        //プレイヤーの前方遠くにいる場合は減速
        SpeedDown();
    }

    private void SpeedUp()
    {
        speedMS = KMath.GetCloser(
            speedMS,
            PlayerCar.current.SpeedMS + myData.MovementData.AddSpeedMS,
            myData.MovementData.AcceleraratorPower);
    }

    private void SpeedDown()
    {
        speedMS = KMath.GetCloser(speedMS,
            PlayerCar.current.SpeedMS - myData.MovementData.RemoveSpeedMS,
            myData.MovementData.BrakePower);
    }

    private void SpeedKeep()
    {
        if (speedMS > PlayerCar.current.SpeedMS)
        {
            speedMS = KMath.GetCloser(speedMS,
            PlayerCar.current.SpeedMS,
            myData.MovementData.BrakePower);
        }
        else
        {
            speedMS = KMath.GetCloser(speedMS,
            PlayerCar.current.SpeedMS,
            myData.MovementData.AcceleraratorPower);
        }
        
    }

    /// <summary>
    /// 道路の中心を動くベース部分の移動
    /// </summary>
    /// <param name="hasDistance"></param>
    /// <param name="distance"></param>
    /// <param name="back"></param>
    private void MoveBase(bool hasDistance = false, float distance = float.NaN, bool back = false)
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
        Transform moveTo = currentRoadChip.End;
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
                moveTo = currentRoadChip.End;
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
        ChangeBaseRotation();
    }

    private RoadChip GetNextRoadChip(RoadChip current, bool back)
    {
        //current.Leave(this);
        current = back ? current.Prev : current.Next;
        if (current)
        {
            //current.Join(this);
        }
        return current;
    }

    /// <summary>
    /// 道路に合わせて土台部分の回転を行う
    /// </summary>
    private void ChangeBaseRotation()
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
