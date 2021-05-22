using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが操作する銃
/// </summary>
public class PlayerGun : MonoBehaviour
{
    /// <summary>
    /// レティクルの画像
    /// </summary>
    [SerializeField]
    Image reticle;

    /// <summary>
    /// レティクルの移動速度
    /// </summary>
    [SerializeField]
    float reticleSpeed;

    /// <summary>
    /// レティクルのトランスフォーム
    /// </summary>
    RectTransform reticleTransform;



    [SerializeField]
    bool debugMode;
    [SerializeField]
    Text debugText;

    private void Awake()
    {
        reticleTransform = reticle.rectTransform;
    }

    private void Update()
    {
        MoveReticle();
        if (debugMode)
        {
            debugText.text = "X:" + reticleTransform.position.x.ToString("000000") + "Y:" + reticleTransform.position.y.ToString("000000");
        }
    }

    /// <summary>
    /// プレイヤーの入力に応じてレティクルを動かす
    /// </summary>
    private void MoveReticle()
    {
        Vector3 move = new Vector2(KInputManager.GetGunMoveInputX(), KInputManager.GetGunMoveInputY());
        move.x *= reticleSpeed;
        move.y *= reticleSpeed;
        //上下反転の必要あり
        move.y = -move.y;
        move.z = 0;

        reticleTransform.position += move;

        DoNotGoToOutSide();
    }


    /// <summary>
    /// レティクルが画面外に出ないようにする
    /// </summary>
    private void DoNotGoToOutSide()
    {
        int width = Screen.width;
        int height = Screen.height;
        Vector3 pos = reticleTransform.position;

        if (pos.x > width)
        {
            pos.x = width;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }

        if (pos.y > height)
        {
            pos.y = height;
        }
        else if (pos.y < 0)
        {
            pos.y = 0;
        }

        reticleTransform.position = pos;
    }
}
