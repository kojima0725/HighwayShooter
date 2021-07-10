using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲームのステージの各データを保持するクラス
/// </summary>
public class StageDatabase : MonoBehaviour
{
    private static NomalCarSpawnData nomalCarSpawnData;
    private static RoadData roadData;
    private static RoadDesignDocument roadDesignDocument;

    /// <summary>
    /// 一般車に関するデータ
    /// </summary>
    public static NomalCarSpawnData NomalCarSpawnData => nomalCarSpawnData;
    /// <summary>
    /// 道に関するデータ
    /// </summary>
    public static RoadData RoadData => roadData;
    /// <summary>
    /// 道の生成に関するデータ
    /// </summary>
    public static RoadDesignDocument RoadDesignDocument => roadDesignDocument;

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
        nomalCarSpawnData = Resources.Load("NomalCarSpawnData") as NomalCarSpawnData;
        roadData = Resources.Load("RoadData") as RoadData;
        roadDesignDocument = Resources.Load("RoadDesignDocument") as RoadDesignDocument;
    }
}
