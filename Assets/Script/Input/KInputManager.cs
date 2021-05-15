using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 入力の管理
/// </summary>
public class KInputManager : MonoBehaviour
{

    /// <summary>
    /// 車の左右ハンドルの入力を取得する
    /// </summary>
    public static float GetCarMoveInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            return 1f;
        }
        return 0f;
    }
}
