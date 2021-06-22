using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 道路上の一般車の生成、削除等を管理する
/// </summary>
[RequireComponent(typeof(Road))]
public class NomalCarManager : MonoBehaviour, ICanGetTransforms
{
    [SerializeField]
    Road road;
    [SerializeField]
    World world;

    /// <summary>
    /// 生成した車をこのゲームオブジェクトの下に格納する
    /// </summary>
    [SerializeField]
    Transform nomalCarContainer;

    /// <summary>
    /// 生成した車たち
    /// </summary>
    List<NpcCar> cars = new List<NpcCar>();

    public IEnumerable<Transform> Transforms()
    {
        foreach (var item in cars)
        {
            yield return item.transform;
        }
    }

}
