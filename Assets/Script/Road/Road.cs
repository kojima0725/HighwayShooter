using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自身に所属しているRoadObjectを後ろに流していく
/// </summary>
public class Road : MonoBehaviour
{
    /// <summary>
    /// シングルトン
    /// </summary>
    public static Road current;
    private void Awake()
    {
        current = this;
    }

    /// <summary>
    /// 許容するオブジェクトの最大値
    /// </summary>
    [SerializeField]
    int maxObjects;

    /// <summary>
    /// 速度(時速何キロメートルか)
    /// </summary>
    float speedKmH = 0;

    /// <summary>
    /// 速度(秒速何メートルか)
    /// </summary>
    float speedMS;

    /// <summary>
    /// 移動方向
    /// </summary>
    Vector3 moveAxis = new Vector3(0,0,-1);

    /// <summary>
    /// 後ろに流していくオブジェクト達
    /// </summary>
    readonly List<Transform> roadObjects = new List<Transform>();

    /// <summary>
    /// 道本体は別物として扱う
    /// </summary>
    readonly List<Transform> roadChips = new List<Transform>();

    #region public関数


    /// <summary>
    /// 後ろに流すオブジェクトを登録する
    /// </summary>
    /// <param name="obj"></param>
    public void Join(Transform obj, bool roadChip = false)
    {
        if (roadChip)
        {
            roadChips.Add(obj);
        }
        else
        {
            roadObjects.Add(obj);
        }
    }


    /// <summary>
    /// 後ろに流すオブジェクトを解除する
    /// </summary>
    /// <param name="obj"></param>
    public void Leave(Transform obj, bool roadChip = false)
    {
        if (roadChip)
        {
            roadChips.Remove(obj);
        }
        else
        {
            roadObjects.Remove(obj);
        }
    }

    /// <summary>
    /// 道路のスピードをセットする
    /// </summary>
    /// <param name="KmH"></param>
    public void SetCarSpeed(float KmH)
    {
        speedKmH = KmH;
        speedMS = MathKoji.KmHToMS(speedKmH);
    }

    /// <summary>
    /// 世界の移動方向を設定する
    /// </summary>
    /// <param name="axis"></param>
    public void SetMoveAxis(Vector3 axis)
    {
        moveAxis = axis.normalized;
    }

    /// <summary>
    /// 世界の移動方向を確認する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveAxis()
    {
        return moveAxis;
    }

    #endregion

    #region プライベート関数

    private void Update()
    {
        //速度計算
        speedMS = MathKoji.KmHToMS(speedKmH);
    }

    private void LateUpdate()
    {
        MoveObjects();
    }

    /// <summary>
    /// 所属しているオブジェクトを後ろに受け流していく
    /// </summary>
    private void MoveObjects()
    {
        Vector3 dist = moveAxis * speedMS * Time.deltaTime;
        foreach (var item in roadObjects)
        {
            Vector3 pos = item.position;
            pos += dist;
            item.position = pos;
        }
        foreach (var item in roadChips)
        {
            Vector3 pos = item.position;
            pos += dist;
            item.position = pos;
        }
    }

    #endregion

}
