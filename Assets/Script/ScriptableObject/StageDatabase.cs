using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲームのステージの各データを保持するクラス
/// </summary>
public class StageDatabase : MonoBehaviour
{
    private static NomalCarData nomalCarData;
    private static RoadData roadData;
    private static RoadDesignDocument roadDesignDocument;
    private static PlayerCarData playerCarData;

    /// <summary>
    /// 一般車に関するデータ
    /// </summary>
    public static NomalCarData NomalCarData => nomalCarData;
    /// <summary>
    /// 道に関するデータ
    /// </summary>
    public static RoadData RoadData => roadData;
    /// <summary>
    /// 道の生成に関するデータ
    /// </summary>
    public static RoadDesignDocument RoadDesignDocument => roadDesignDocument;
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
        //生成する車のデータを読み込み
        nomalCarData = Resources.Load("NomalCarData") as NomalCarData;
        roadData = Resources.Load("RoadData") as RoadData;
        roadDesignDocument = Resources.Load("RoadDesignDocument") as RoadDesignDocument;
        playerCarData = Resources.Load("PlayerCarData") as PlayerCarData;
    }
}
