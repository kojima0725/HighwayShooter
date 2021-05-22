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

    [SerializeField]
    float reticleSpeed;


    [SerializeField]
    bool debugMode;
    [SerializeField]
    Text debugText;

    private void Update()
    {
        MoveReticle();
        if (debugMode)
        {
            debugText.text = "X:" + reticle.rectTransform.position.x.ToString("000000") + "Y:" + reticle.rectTransform.position.y.ToString("000000");
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

        reticle.rectTransform.position += move;
    }
}
