using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    /// <summary>
    /// 操作対象のカメラ
    /// </summary>
    [SerializeField]
    Transform camera;

    [SerializeField]
    PlayerGun gun;
}
