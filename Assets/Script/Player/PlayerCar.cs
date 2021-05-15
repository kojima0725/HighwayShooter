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
    GameObject body;

    /// <summary>
    /// 車体の傾きの最大値
    /// </summary>
    [SerializeField]
    float bodyRollMax = 45f;

    /// <summary>
    /// 車体を傾ける感度;
    /// </summary>
    [SerializeField]
    float rollSensitivity;

    /// <summary>
    /// ハンドルの力
    /// </summary>
    [SerializeField]
    float handleMaxPower;

    /// <summary>
    /// ハンドルの感度
    /// </summary>
    [SerializeField]
    float handleSensitivity;

    /// <summary>
    /// ハンドルの入力感度(入力にどれだけ素直に反応するか)
    /// </summary>
    [SerializeField]
    float handleInputSensitivity;
    
    /// <summary>
    /// 車のハンドルをどちらに回しているか
    /// </summary>
    float handleInput;


    /// <summary>
    /// ハンドルの状態
    /// </summary>
    float handle;

    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    [SerializeField]
    bool debugMode = false;

    [SerializeField]
    Text hundleTxt;

    

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MoveHandle();
        RollBody();
        MoveCar();
        if (debugMode)
        {
            hundleTxt.text = "ハンドルは" + handle;
        }
    }

    /// <summary>
    /// プレイヤーからの入力を受け取る
    /// </summary>
    private void GetInput()
    {
        handleInput = MathKoji.GetCloser(handleInput, KInputManager.GetCarMoveInput(), handleInputSensitivity) ; KInputManager.GetCarMoveInput();
        //Debug.Log(holizontalPower);
    }

    /// <summary>
    /// ハンドルを回す
    /// </summary>
    private void MoveHandle()
    {
        handle += handleInput * handleSensitivity * Time.deltaTime;
        if (handle > handleMaxPower)
        {
            handle = handleMaxPower;
        }
        else if (handle < -handleMaxPower)
        {
            handle = -handleMaxPower;
        }
    }

    /// <summary>
    /// 車を左右に動かす
    /// </summary>
    private void MoveCar()
    {
        Vector3 positon = transform.position;

        positon.x += handle * Time.deltaTime;


        transform.position = positon;

    }

    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        var roll = body.transform.localEulerAngles;

        if (roll.y > 180)
        {
            roll.y -= 360;
        }

        roll.y = MathKoji.GetCloser(roll.y, handle / handleMaxPower * bodyRollMax, rollSensitivity);
        body.transform.localEulerAngles = roll;
    }
}
