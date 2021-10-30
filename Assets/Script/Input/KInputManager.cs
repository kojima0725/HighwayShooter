using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 入力の管理
/// </summary>
public static class KInputManager
{

    /// <summary>
    /// 車の左右ハンドルの入力を取得する
    /// </summary>
    public static float GetCarMoveInput()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return Input.acceleration.x * 0.8f;
#endif
        float a = Input.GetAxis("LStickH");
        if (a != 0)
        {
            return a;
        }

        //ジョイスティック入力がない場合はキーボード入力を考慮する
        if (Input.GetKey(KeyCode.A))
        {
            return -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            return 1f;
        }
        return 0f;
    }

    /// <summary>
    /// 銃の移動入力を取得する(水平方向)
    /// </summary>
    /// <returns></returns>
    public static float GetGunMoveInputX()
    {
        float a = Input.GetAxis("RStickH");
        if (a != 0)
        {
            return a;
        }

        //ジョイスティック入力がない場合はキーボードの矢印入力を受け付ける
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return 1f;
        }
        return 0f;
    }

    /// <summary>
    /// 銃の移動入力を取得する(上下方向)
    /// </summary>
    /// <returns></returns>
    public static float GetGunMoveInputY()
    {
        float a = Input.GetAxis("RStickV");
        if (a != 0)
        {
            return -a;
        }

        //ジョイスティック入力がない場合はキーボードの矢印入力を受け付ける
        if (Input.GetKey(KeyCode.DownArrow))
        {
            return -1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return 1f;
        }
        return 0f;
    }


    /// <summary>
    /// 銃の発射入力を取得する
    /// </summary>
    /// <param name="down"></param>
    /// <returns></returns>
    public static bool GetGunShootInput(bool down)
    {
        if (down)
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            return Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.Space);
        }
    }

    /// <summary>
    /// アクセルの入力を取得する
    /// </summary>
    /// <returns></returns>
    public static float GetCerAcceleratorInput()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //モバイル端末は常に加速する
        return 0.8f;
#endif
        float a = Input.GetAxis("LT");
        if (a != 0)
        {
            return a;
        }

        if (Input.GetKey(KeyCode.W))
        {
            return 1f;
        }
        return 0f;
    }

    /// <summary>
    /// ブレーキの入力を取得する
    /// </summary>
    /// <returns></returns>
    public static float GetCarBrakeInput()
    {
        if (Input.GetButton("LB"))
        {
            return 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            return 1f;
        }

        return 0f;
    }
}
