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
    /// 自身の最奥の位置
    /// </summary>
    [SerializeField]
    float far;

    /// <summary>
    /// マップチップ
    /// </summary>
    [SerializeField]
    GameObject chip;

    bool sonIsMaked = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 道路を生成する必要があるか確認し、その場合は生成を行う
    /// </summary>
    private void SonCheck()
    {
        if (sonIsMaked)
        {
            return;
        }
        if (transform.position.z < far)
        {
            //自身を複製
            GameObject son = Instantiate(chip, this.transform.parent);
            son.transform.position = end.position;
            son.transform.rotation = end.rotation;
            son.transform.Rotate(new Vector3(0,2f,0));
            sonIsMaked = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        SonCheck();
    }

    public override void Death()
    {
        SonCheck();
        base.Death();
    }
}
