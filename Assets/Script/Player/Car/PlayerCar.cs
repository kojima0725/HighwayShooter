using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーの乗っている車
/// ゲームパッドの左側で操作する
/// </summary>
[RequireComponent(typeof(PlayerCarMover))]
public class PlayerCar : MonoBehaviour
{
    /// <summary>
    /// ゲーム中にどこからでもアクセスできるようにする
    /// </summary>
    public static PlayerCar current;

    [SerializeField]
    PlayerCarMover mover;
    [SerializeField]
    WindZone windZone;
    [SerializeField]
    Image speedGauge;

    public float SpeedMS => KMath.KmHToMS(mover.Speed);

    public Transform Body => mover.Body;

    private void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    private void Update()
    {
        mover.MoveUpdate();
        windZone.windMain = SpeedMS;
        speedGauge.fillAmount = mover.Speed / PlayerDataBase.PlayerCarData.MaxSpeed;
    }
}
