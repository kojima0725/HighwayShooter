using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一般車のデータ
/// </summary>
[CreateAssetMenu(fileName = "NomalCarData", menuName = "ScriptableObjects/NomalCarData")]
public class NomalCarData : ScriptableObject
{
    [SerializeField]
    private NomalCar carPrefab;
    [SerializeField]
    private float hp;

    public float HP => hp;

    /// <summary>
    /// 車を生成する
    /// </summary>
    /// <returns>生成された車</returns>
    public NomalCar GeneratePlayerCar()
    {
        NomalCar maked = Instantiate(carPrefab);
        maked.SetData(this);
        return maked;
    }
}
