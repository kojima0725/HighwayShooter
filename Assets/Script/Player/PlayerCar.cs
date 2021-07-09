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
    [SerializeField]
    PlayerCarMover mover;

    // Update is called once per frame
    private void Update()
    {
        mover.MoveUpdate();
    }
}
