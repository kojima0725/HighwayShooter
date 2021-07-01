using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    /// <summary>
    /// 操作対象のカメラ
    /// </summary>
    [SerializeField]
    private Transform camera;

    /// <summary>
    /// プレイヤーの銃
    /// </summary>
    [SerializeField]
    private PlayerGun gun;

    [SerializeField]
    float rotateX;

    [SerializeField]
    float rotateY;

    private Quaternion baseRotation;

    private void Awake()
    {
        baseRotation = camera.localRotation;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector2 pos = gun.GetReticlePos();
        float x = rotateX * pos.x;
        float y = rotateY * pos.y;
        Quaternion rotate = Quaternion.Euler(-y,x,0);
        Quaternion after = baseRotation * rotate;
        camera.localRotation = after;
    }

}
