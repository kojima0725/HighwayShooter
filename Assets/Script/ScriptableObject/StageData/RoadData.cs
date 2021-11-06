using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 道路に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "RoadData", menuName = "ScriptableObjects/CreateRoadData")]
public class RoadData : ScriptableObject
{
    [SerializeField]
    private float length;
    [SerializeField]
    private float width;
    [SerializeField]
    private int lane;  
    [SerializeField]
    private float limitDistance;
    [SerializeField]
    private float deathDistance;
    [SerializeField]
    private float[] lanePos;


    /// <summary>
    /// ロードチップの長さ
    /// </summary>
    public float Length => length;

    /// <summary>
    /// ロードチップの幅
    /// </summary>
    public float Width => width;

    /// <summary>
    /// 車線数
    /// </summary>
    public int Lane => lane;

    /// <summary>
    /// 道路の生成限界
    /// </summary>
    public float LimitDistance => limitDistance;

    /// <summary>
    /// マップがこの距離以上離れたら破棄する
    /// </summary>
    public float DeathDistance => deathDistance;

    /// <summary>
    /// 各レーンのオフセット値
    /// </summary>
    public float[] LanePosOffsets => lanePos;

    /// <summary>
    /// 位置レーンの幅
    /// </summary>
    public float LaneWidth => width / (lane + 1);


    private void OnValidate()
    {
        MakeLanePos();
        //EditorUtility.SetDirty(this);
    }

    private void MakeLanePos()
    {
        lanePos = new float[lane];
        float halfWidth = width / 2;
        float laneWidth = width / (lane + 1);

        float pos = halfWidth - laneWidth;

        for (int i = 0; i < lane; i++)
        {
            lanePos[i] = pos;
            pos -= laneWidth;
        }
    }
}
