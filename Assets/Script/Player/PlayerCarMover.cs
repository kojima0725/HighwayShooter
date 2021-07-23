using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーの車を移動させる
/// </summary>
[RequireComponent(typeof(PlayerCarDriver))]
public class PlayerCarMover : MonoBehaviour
{
    [SerializeField]
    PlayerCarDriver driver;

    /// <summary>
    /// 車体
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// 車体の右前
    /// </summary>
    [SerializeField]
    private Transform rightFront;
    /// <summary>
    /// 車体の左後
    /// </summary>
    [SerializeField]
    private Transform leftBack;
    /// <summary>
    /// 車体の左前
    /// </summary>
    private Transform leftFront;
    /// <summary>
    /// 車体の右後ろ
    /// </summary>
    private Transform rightBack;

    /// <summary>
    /// 現在の車の速度
    /// </summary>
    private float speed;

    private float SpeedMS => KMath.KmHToMS(speed);

    public float Speed => speed;

    public Transform Body => body;

    private void Awake()
    {
        MakeData();
    }

    private void Start()
    {
        speed = PlayerDataBase.PlayerCarData.FirstSpeed;
    }

    public void MoveUpdate()
    {
        driver.DriverUpdate();
        //車回転
        RollBody();
        //車の速度変更
        ChangeCarSpeed();
        //車の座標修正
        SwipeCarAndWorld();

        //世界に自身の移動状況を通達する
        MoveWorld();
    }


    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        var roll = body.transform.localEulerAngles;

        roll.y += driver.HandleInput * PlayerDataBase.PlayerCarData.RollSensitivity * Time.deltaTime;

        body.transform.localEulerAngles = roll;
    }

    /// <summary>
    /// 車の速度を変更する
    /// </summary>
    private void ChangeCarSpeed()
    {
        float a = driver.Acceleration;
        if (a == 0)
        {
            return;
        }
        //加速時
        if (a >= 0)
        {
            speed = KMath.GetCloser(speed, PlayerDataBase.PlayerCarData.MaxSpeed,
                PlayerDataBase.PlayerCarData.BrakePower * a);
        }
        //減速時
        else if (driver.Acceleration <= 0)
        {
            a = -a;
            speed = KMath.GetCloser(speed, 0f,
                PlayerDataBase.PlayerCarData.BrakePower * a);
        }
    }

    /// <summary>
    /// 車のずれた座標を修正し、世界もそれに対応させる
    /// </summary>
    private void SwipeCarAndWorld()
    {
        Vector3 swipe = -body.transform.localPosition;
        World.current?.SwipeWorld(swipe);
        body.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 世界に自身の移動状況を伝える
    /// </summary>
    private void MoveWorld()
    {
        if (World.current)
        {
            World.current.SwipeWorld(
                -body.transform.forward * SpeedMS * Time.deltaTime );
        }
    }

    /// <summary>
    /// 車体前方の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line FrontLine() => MakeLine(rightFront, leftFront);

    /// <summary>
    /// 車体左の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line LeftLine() => MakeLine(leftFront, leftBack);

    /// <summary>
    /// 車体右の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line RightLine() => MakeLine(rightFront, rightBack);

    /// <summary>
    /// 指定された２個のトランスフォームから線を作成する(x.z座標)
    /// </summary>
    /// <returns></returns>
    private Vector2Line MakeLine(Transform start, Transform end)
    {
        return new Vector2Line(start.position.x, start.position.z, end.position.x, end.position.z);
    }


    /// <summary>
    /// 必要なデータの作成
    /// </summary>
    private void MakeData()
    {
        leftFront = new GameObject("LeftFront").transform;
        leftFront.parent = this.transform;
        leftFront.localPosition = new Vector3(leftBack.localRotation.x, 0, rightFront.localPosition.z);

        rightBack = new GameObject("RightBack").transform;
        rightBack.parent = this.transform;
        rightBack.localPosition = new Vector3(rightFront.localPosition.x, 0, leftBack.localPosition.z);
    }
}
