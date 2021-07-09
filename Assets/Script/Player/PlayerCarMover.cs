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
    /// 現在の車の速度
    /// </summary>
    private float speed;

    private void Start()
    {
        speed = StageDatabase.PlayerCarData.FirstSpeed;
        World.current?.SetCarSpeed(speed);
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

        roll.y += driver.HandleInput * StageDatabase.PlayerCarData.RollSensitivity * Time.deltaTime;

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
            speed = MathKoji.GetCloser(speed, StageDatabase.PlayerCarData.MaxSpeed,
                StageDatabase.PlayerCarData.BrakePower * a);
        }
        //減速時
        else if (driver.Acceleration <= 0)
        {
            a = -a;
            speed = MathKoji.GetCloser(speed, 0f, 
                StageDatabase.PlayerCarData.BrakePower * a);
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
            World.current.SetCarSpeed(speed);
            World.current.SetMoveAxis(-body.transform.forward);
        }
    }
}
