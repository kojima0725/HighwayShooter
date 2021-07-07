using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーの乗っている車
/// ゲームパッドの左側で操作する
/// </summary>
[RequireComponent(typeof(PlayerCarMover))]
public class PlayerCar : MonoBehaviour
{
    /// <summary>
    /// 車体
    /// </summary>
    [SerializeField]
    private GameObject body;

    /// <summary>
    /// 現在の車の速度
    /// </summary>
    private float speed;

    [SerializeField]
    PlayerCarMover mover;

    /// <summary>
    /// 車のハンドルの入力状態
    /// </summary>
    private float handleInput;


    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    [SerializeField]
     private bool debugMode = false;

    [SerializeField]
     private Text hundleTxt;

    private void Start()
    {
        speed = StageDatabase.PlayerCarData.FirstSpeed;
        World.current?.SetCarSpeed(speed);
    }

    // Update is called once per frame
    private void Update()
    {
        //ハンドル関連の処理
        GetHandleInput();
        RollBody();
        //アクセル、ブレーキ関連の処理
        ChangeCarSpeed();
        //車の座標修正
        SwipeCarAndWorld();

        //世界に自身の移動状況を通達する
        MoveWorld();

        if (debugMode)
        {
            hundleTxt.text = "ハンドルは" + handleInput;
        }
    }

    /// <summary>
    /// プレイヤーからのハンドル入力を受け取る
    /// </summary>
    private void GetHandleInput()
    {
        handleInput = MathKoji.GetCloser(handleInput, KInputManager.GetCarMoveInput(), StageDatabase.PlayerCarData.HandleInputSensitivity) ; KInputManager.GetCarMoveInput();
        //Debug.Log(holizontalPower);
    }


    /// <summary>
    /// 車の速度を変更する
    /// </summary>
    private void ChangeCarSpeed()
    {
        float input = KInputManager.GetCarBrakeInput();
        if (0 != input)
        {
            speed = MathKoji.GetCloser(speed,0f, StageDatabase.PlayerCarData.BrakePower * input);
            return;
        }
        input = KInputManager.GetCerAcceleratorInput();
        if (0 != input)
        {
            speed = MathKoji.GetCloser(speed, StageDatabase.PlayerCarData.MaxSpeed, StageDatabase.PlayerCarData.BrakePower * input);
            return;
        }
        
    }

    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        var roll = body.transform.localEulerAngles;

        roll.y += handleInput * StageDatabase.PlayerCarData.RollSensitivity * Time.deltaTime;

        body.transform.localEulerAngles = roll;
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
