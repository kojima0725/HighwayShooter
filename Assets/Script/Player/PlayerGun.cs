using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが操作する銃
/// </summary>
public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    Image reticle;
    [SerializeField]
    float reticleSpeed;
    [SerializeField]
    int rpm;
    [SerializeField]
    float length;
    [SerializeField]
    float power;
    [SerializeField]
    bool isSemiAuto;

    RectTransform reticleTransform;
    float interval;
    float timer;

    [SerializeField]
    Text debugTxt;

    private void Awake()
    {
        reticleTransform = reticle.rectTransform;
        reticleTransform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2);
        interval = 60.0f / rpm;
    }

    private void Update()
    {
        //発射処理
        if (timer < interval)
        {
            timer += Time.deltaTime;
        }
        if (KInputManager.GetGunShootInput(isSemiAuto) && timer > interval)
        {
            Shoot();
            timer = 0;
        }

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
        Vector3 move;
#if UNITY_ANDROID && !UNITY_EDITOR
        move = KInputManager.GetGunMoveInputMobile();
        reticleTransform.position += move;
#else
        move = new Vector2(KInputManager.GetGunMoveInputX(), KInputManager.GetGunMoveInputY());
        move.x *= reticleSpeed;
        move.y *= reticleSpeed;
        move.z = 0;
        //レティクル移動(時間、スクリーン解像度を考慮)
        reticleTransform.position += move * Time.deltaTime * Screen.height;
#endif
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

    private void Shoot()
    {
        RaycastHit hit;
        if (ShootRay(out hit))
        {
            GameObject hitEffect;
            if (hit.transform.tag == "Enemy")
            {
                Transform enemyTransform = KMath.GetRoot(hit.transform, "Enemy");
                EnemyCar enemy = enemyTransform.GetComponent<EnemyCar>();
                enemy?.GetDamage(power);
                hitEffect = EffectManager.instance.MakeBulletInpactCar();
            }
            else if (hit.transform.tag == "NPC")
            {
                Transform npcTransform = KMath.GetRoot(hit.transform, "NPC");
                NomalCar npc = npcTransform.GetComponent<NomalCar>();
                npc?.GetDamage(power);
                hitEffect = EffectManager.instance.MakeBulletInpactCar();
            }
            else if (hit.transform.tag == "Jumper")
            {
                Transform npcTransform = KMath.GetRoot(hit.transform, "Jumper");
                NomalCar npc = npcTransform.GetComponent<NomalCar>();
                npc?.GetDamage(power);
                hitEffect = EffectManager.instance.MakeBulletInpactCar();
            }
            else
            {
                hitEffect = EffectManager.instance.MakeBulletInpactRoad();
            }

            if (hitEffect)
            {
                hitEffect.transform.localScale *= 2;
                hitEffect.transform.parent = hit.transform;
                hitEffect.transform.position = hit.point;
                hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
            
        }

        SoundEffectManager.instance?.PlayShootSound();
    }

    private bool ShootRay(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(reticleTransform.position);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * length, Color.red);
        return Physics.Raycast(ray, out hit, length);
    }
}
