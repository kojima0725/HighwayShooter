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
    /// 道の長さ
    /// </summary>
    private const int MaxLength = 100;
    
    /// <summary>
    /// 現在の道の長さ
    /// </summary>
    static int length = 0;

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

    protected override void Awake()
    {
        length++;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Join()
    {
        Road.current.Join(transform, true);
        joined = true;
    }

    /// <summary>
    /// 道路を生成する必要があるか確認し、その場合は生成を行う
    /// </summary>
    private void SonCheck(bool onDeath = false)
    {
        if (sonIsMaked)
        {
            return;
        }
        if (length < MaxLength || onDeath)
        {
            MakeSon();
        }
    }


    /// <summary>
    /// 自身を複製する
    /// </summary>
    private void MakeSon()
    {
        //自身を複製
        GameObject son = Instantiate(chip, Road.current.transform);
        son.transform.position = end.position;
        son.transform.rotation = end.rotation;
        son.transform.Rotate(new Vector3(0, 2f, 0));
        sonIsMaked = true;
    }

    protected override void Update()
    {
        base.Update();
        SonCheck();
    }

    public override void Death()
    {
        SonCheck(true);
        base.Death();
    }

    protected override void OnDestroy()
    {
        length--;
        //道のリストから自身を削除
        Road.current.Leave(transform, true);
    }
}
