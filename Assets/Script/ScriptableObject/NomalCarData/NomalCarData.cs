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

    public NomalCar GeneratePlayerCar()
    {
        NomalCar maked = Instantiate(carPrefab);
        return maked;
    }
}
