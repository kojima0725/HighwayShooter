using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーからの入力に対応する
/// </summary>
public class PlayerCarDriver : MonoBehaviour
{
    [SerializeField]
    private Text handleTxt;

    /// <summary>
    /// 車のハンドルの入力状態
    /// </summary>
    private float handleInput;

    private float acceleration;


    /// <summary>
    /// 車のハンドルの入力状態
    /// </summary>
    public float HandleInput => handleInput;

    /// <summary>
    /// 車の加速度の入力状態
    /// </summary>
    public float Acceleration => acceleration;

    public void DriverUpdate()
    {
        GetHandleInput();
        GetAccelerationInput();
    }

    private void GetHandleInput()
    {
        handleInput = MathKoji.GetCloser(handleInput, 
            KInputManager.GetCarMoveInput(),
            PlayerDataBase.PlayerCarData.HandleInputSensitivity);
        if (handleTxt)
        {
            handleTxt.text = "ハンドルは" + handleInput;
        }
    }

    private void GetAccelerationInput()
    {
        //優先度は{ブレーキ＞アクセル＞無入力}
        float input = KInputManager.GetCarBrakeInput();
        if (0 != input)
        {
            acceleration = -input;
            return;
        }
        input = KInputManager.GetCerAcceleratorInput();
        if (0 != input)
        {
            acceleration = input;
            return;
        }
        acceleration = 0;
    }
}
