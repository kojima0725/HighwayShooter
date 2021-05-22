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


    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////

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

    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////


    /// <summary>
    /// 後ろに流すオブジェクトを登録する
    /// </summary>
    /// <param name="obj"></param>
    public void Join(Transform obj)
    {
        roadObjects.Add(obj);
    }


    /// <summary>
    /// 後ろに流すオブジェクトを解除する
    /// </summary>
    /// <param name="obj"></param>
    public void Leave(Transform obj)
    {
        roadObjects.Remove(obj);
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

    public void SetMoveAxis(Vector3 axis)
    {
        moveAxis = axis.normalized;
    }

    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////

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
    }



}
