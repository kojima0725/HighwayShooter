using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーの乗っている車
/// ゲームパッドの左側で操作する
/// </summary>
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
    [SerializeField]
    private float speed;

    #region ハンドル関連

    /// <summary>
    /// 車体を傾ける感度;
    /// </summary>
    [SerializeField]
    private float rollSensitivity;
    
    /// <summary>
    /// ハンドルの入力感度(入力にどれだけ素直に反応するか)
    /// </summary>
    [SerializeField]
    private float handleInputSensitivity;

    /// <summary>
    /// 車のハンドルの入力状態
    /// </summary>
    private float handleInput;

    #endregion

    #region アクセル、ブレーキ関連

    /// <summary>
    /// 最大速度
    /// </summary>
    [SerializeField]
    private float maxSpeed;

    /// <summary>
    /// 加速力
    /// </summary>
    [SerializeField]
    private float accelerationPower;

    /// <summary>
    /// 減速力
    /// </summary>
    [SerializeField]
    private float brakePower;

    #endregion

    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    [SerializeField]
     private bool debugMode = false;

    [SerializeField]
     private Text hundleTxt;

    private void Start()
    {
        World.current.SetCarSpeed(speed);
    }

    // Update is called once per frame
    private void Update()
    {
        //ハンドル関連の処理
        GetHandleInput();
        RollBody();
        //アクセル、ブレーキ関連の処理
        ChangeCarSpeed();

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
        handleInput = MathKoji.GetCloser(handleInput, KInputManager.GetCarMoveInput(), handleInputSensitivity) ; KInputManager.GetCarMoveInput();
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
            speed = MathKoji.GetCloser(speed,0f,brakePower * input);
            World.current.SetCarSpeed(speed);
            return;
        }
        input = KInputManager.GetCerAcceleratorInput();
        if (0 != input)
        {
            speed = MathKoji.GetCloser(speed, maxSpeed, brakePower * input);
            World.current.SetCarSpeed(speed);
            return;
        }
        
    }

    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        var roll = body.transform.localEulerAngles;

        roll.y += handleInput * rollSensitivity * Time.deltaTime;

        body.transform.localEulerAngles = roll;

        World.current.SetMoveAxis(-body.transform.forward);
    }
}
