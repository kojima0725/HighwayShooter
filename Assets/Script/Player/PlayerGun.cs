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
    Text debugTxt;

    private void Awake()
    {
        reticleTransform = reticle.rectTransform;
        reticleTransform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2);
    }

    private void Update()
    {
        MoveReticle();
        if (debugTxt)
        {
            debugTxt.text = $"X:{GetReticlePos().x} Y:{GetReticlePos().y}";
        }
    }

    /// <summary>
    /// レティクルが画面のどのあたりにあるかの情報を返す
    /// 画面の中心がVector2.zeroとなる
    /// </summary>
    /// <returns>Vector2(各パラメータは-1～1の間)</returns>
    public Vector2 GetReticlePos()
    {
        Vector2 a = new Vector2();
        a.x = (reticleTransform.position.x / Screen.width - 0.5f) * 2;
        a.y = (reticleTransform.position.y / Screen.height - 0.5f) * 2;
        return a;
    }

    /// <summary>
    /// プレイヤーの入力に応じてレティクルを動かす
    /// </summary>
    private void MoveReticle()
    {
        Vector3 move = new Vector2(KInputManager.GetGunMoveInputX(), KInputManager.GetGunMoveInputY());
        move.x *= reticleSpeed;
        move.y *= reticleSpeed;

        move.z = 0;

        //レティクル移動(時間、スクリーン解像度を考慮)
        reticleTransform.position += move * Time.deltaTime * Screen.height;

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
