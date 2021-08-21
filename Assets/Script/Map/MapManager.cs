﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour,ICanGetTransforms
{
    [SerializeField]
    private World world;
    [SerializeField]
    private MapMaker mapMaker;

    /// <summary>
    /// マップチップをこのオブジェクト下に格納する
    /// </summary>
    [SerializeField]
    private Transform mapChipContainer;

    /// <summary>
    /// 生成したマップチップ
    /// </summary>
    readonly List<MapChip> chips = new List<MapChip>();

    public IEnumerable<Transform> Transforms()
    {
        foreach (var item in chips)
        {
            yield return item.transform;
        }
    }

    private void Awake()
    {
        //世界に登録
        world.JoinWorld(this);
    }

    private void Start()
    {
        MapChip make = mapMaker.MakeChip();
        chips.Add(make);
        make.transform.parent = mapChipContainer;
    }
}