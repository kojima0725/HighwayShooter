using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのデータを保持するクラス
/// </summary>
public class PlayerDataBase : MonoBehaviour
{
    private static PlayerCarData playerCarData;

    /// <summary>
    /// プレイヤーの車に関するデータ
    /// </summary>
    public static PlayerCarData PlayerCarData => playerCarData;


    private void Awake()
    {
        LoadData();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// データを一通り用意する
    /// </summary>
    public static void LoadData()
    {
        playerCarData = Resources.Load("PlayerCarData") as PlayerCarData;
    }
}
