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

    [SerializeField]
    float speed;

    /////////////////////////////////////
    /////////////////////////////////////
    /////////////////////////////////////
    #region ハンドル関連

    /// <summary>
    /// 車体を傾ける感度;
    /// </summary>
    [SerializeField]
    float rollSensitivity;
    
    /// <summary>
    /// ハンドルの入力感度(入力にどれだけ素直に反応するか)
    /// </summary>
    [SerializeField]
    float handleInputSensitivity;

    /// <summary>
    /// 車のハンドルの入力状態
    /// </summary>
    float handleInput;

    #endregion
    /////////////////////////////////////
    /////////////////////////////////////
    /////////////////////////////////////
    

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
        RollBody();
        MoveCar();

        if (debugMode)
        {
            hundleTxt.text = "ハンドルは" + handleInput;
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
    /// 車を動かす(道を後ろに流す)
    /// </summary>
    private void MoveCar()
    {
        //Vector3 positon = transform.position;

        //positon.x += handle * Time.deltaTime;


        //transform.position = positon;

        World.current.SetCarSpeed(speed);
        
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
