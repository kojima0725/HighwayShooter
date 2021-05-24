using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路本体、自動で生成され続ける
/// RoadObjectを継承している
/// </summary>
public class RoadChip : RoadObject
{

    /// <summary>
    /// 自身の最後端の位置
    /// </summary>
    [SerializeField]
    private Transform end;

    /// <summary>
    /// マップチップ(自分自身のプレハブ)
    /// </summary>
    [SerializeField]
    GameObject chip;

    bool sonIsMaked = false;

    /// <summary>
    /// 道の終端の位置を渡す
    /// </summary>
    /// <returns></returns>
    public Transform GetEnd()
    {
        return end;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Join()
    {
        Road.current.Join(this);
        joined = true;
    }


    /// <summary>
    /// 自身を複製してつなげる
    /// </summary>
    /// <param name="rotate">どれぐらい道を曲げるか</param>
    public void MakeSon(Vector3 rotate)
    {
        //自身を複製
        GameObject son = Instantiate(chip, Road.current.transform);
        son.transform.position = end.position;
        son.transform.rotation = end.rotation;
        son.transform.Rotate(rotate);
        sonIsMaked = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void OnDestroy()
    {
        //道のリストから自身を削除
        Road.current.Leave(this);
    }
}
