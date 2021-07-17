using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ステージに登場する車(一般車と敵車)をまとめたデータ
/// </summary>
[CreateAssetMenu(fileName = "CarTypesData", menuName = "ScriptableObjects/CreateCarTypesData")]
public class CarTypesData : ScriptableObject
{
    [SerializeField]
    private EnemyCarData[] enemyCarList;
    [SerializeField]
    private NomalCarData[] nomalCarList; 

    /// <summary>
    /// 敵車のデータリスト
    /// </summary>
    public EnemyCarData[] Enemys => enemyCarList;

    public NomalCarData[] NomalCarList => nomalCarList;
}
