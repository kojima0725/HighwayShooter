using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーの車を移動させる
/// </summary>
[RequireComponent(typeof(PlayerCarDriver))]
public class PlayerCarMover : MonoBehaviour
{
    [SerializeField]
    PlayerCarDriver driver;

    /// <summary>
    /// 車体
    /// </summary>
    [SerializeField]
    private Transform body;

    private float bodyHalfWidth;


    [SerializeField]
    private Transform rightFront;
    [SerializeField]
    private Transform leftBack;
    private Transform leftFront;
    private Transform rightBack;

    private Vector2Line frontLine;
    private Vector2Line rightLine;
    private Vector2Line leftLine;

    private float centerToLrLength;
    private float centerToLrSqrLength;

    private float speed;

    private float SpeedMS => KMath.KmHToMS(speed);

    public float Speed => speed;

    public Transform Body => body;

    private Vector2 BodyPos => new Vector2(body.position.x, body.position.z);
    private Vector2Line CenterToleftLine => new Vector2Line(BodyPos, new Vector2(leftFront.position.x, leftFront.position.z));
    private Vector2Line CenterToRightLine => new Vector2Line(BodyPos, new Vector2(rightFront.position.x, rightFront.position.z));



    private void Awake()
    {
        MakeData();
    }

    private void Start()
    {
        speed = PlayerDataBase.PlayerCarData.FirstSpeed;
    }

    public void MoveUpdate()
    {
        //データ更新
        DataUpdate();
        //入力更新
        driver.DriverUpdate();
        //車回転
        RollBody();
        //車の速度変更
        ChangeCarSpeed();
        //車の移動
        MoveCar();
        //車の座標修正
        SwipeCarAndWorld();
    }

    /// <summary>
    /// 毎フレーム走るデータ更新処理
    /// </summary>
    private void DataUpdate()
    {
        frontLine = MakeFrontLine();
        rightLine = MakeRightLine();
        leftLine = MakeLeftLine();
    }


    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        var roll = body.transform.localEulerAngles;

        roll.y += driver.HandleInput * PlayerDataBase.PlayerCarData.RollSensitivity * Time.deltaTime;

        body.transform.localEulerAngles = roll;
    }

    /// <summary>
    /// 車の速度を変更する
    /// </summary>
    private void ChangeCarSpeed()
    {
        float a = driver.Acceleration;
        if (a == 0)
        {
            return;
        }
        //加速時
        if (a >= 0)
        {
            speed = KMath.GetCloser(speed, PlayerDataBase.PlayerCarData.MaxSpeed,
                PlayerDataBase.PlayerCarData.BrakePower * a);
        }
        //減速時
        else if (driver.Acceleration <= 0)
        {
            a = -a;
            speed = KMath.GetCloser(speed, 0f,
                PlayerDataBase.PlayerCarData.BrakePower * a);
        }
    }

    /// <summary>
    /// 車のずれた座標を修正し、世界もそれに対応させる
    /// </summary>
    private void SwipeCarAndWorld()
    {
        Vector3 swipe = -body.transform.localPosition;
        World.current?.SwipeWorld(swipe);
        body.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 世界に自身の移動状況を伝える
    /// </summary>
    private void MoveCar()
    {
        body.localPosition = body.forward * SpeedMS * Time.deltaTime;
        if (RoadManager.current)
        {
            GurdrailHit hit;
            if (RoadManager.current.GurdrailHitCheck(false, rightLine, out hit))
            {
                Debug.Log("hitR");
            }
            else if (RoadManager.current.GurdrailHitCheck(true, CenterToleftLine, out hit))
            {
                HittingToLeft(hit);
            }
        }
    }

    /// <summary>
    /// 左のガードレールにぶつかった時の処理
    /// </summary>
    /// <param name="hit">衝突地点の情報</param>
    private void HittingToLeft(GurdrailHit hit)
    {
        //ガードレールと車体の中心の距離が一定以下になっていないか調べる
        float kyori = Vector2.Dot(hit.hitChip.GurdrailLeftNomal, BodyPos - hit.hitLine.end);
        if (kyori < bodyHalfWidth)
        {
            //車を一旦後ろ(ガードレールの中)に戻し、そこから戻した距離壁に沿って移動する
            body.position += new Vector3(hit.hitChip.GurdrailLeftNomal.x,0, hit.hitChip.GurdrailLeftNomal.y) * (-kyori + bodyHalfWidth);
            body.rotation = Quaternion.LookRotation(new Vector3(hit.hitLine.vector.x, 0, hit.hitLine.vector.y), Vector3.up);
        }
        else
        {
            //ぶつかっているが、回転だけでどうにかなるので回転だけでどうにかする
            Vector2 cornerHit;
            if (KMath.LineToLineCollision(hit.hitLine, CenterToleftLine, out cornerHit))
            {
                float sqrKyori = kyori * kyori;
                float nokori = Mathf.Sqrt(centerToLrSqrLength - sqrKyori);
                if (nokori > 0)
                {
                    Vector2 rollTo = -hit.hitChip.GurdrailLeftNomal * kyori + hit.hitLine.vector.normalized * nokori;
                    Vector2 from = CenterToleftLine.vector;
                    Quaternion quaternion = Quaternion.FromToRotation(new Vector3(from.x, 0, from.y), new Vector3(rollTo.x, 0, rollTo.y));
                    body.rotation *= quaternion;
                }
            }
        }
    }

    /// <summary>
    /// 車体前方の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line MakeFrontLine() => MakeLine(rightFront, leftFront);

    /// <summary>
    /// 車体左の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line MakeLeftLine() => MakeLine(leftFront, leftBack);

    /// <summary>
    /// 車体右の当たり判定の線
    /// </summary>
    /// <returns></returns>
    private Vector2Line MakeRightLine() => MakeLine(rightFront, rightBack);

    /// <summary>
    /// 指定された２個のトランスフォームから線を作成する(x.z座標)
    /// </summary>
    /// <returns></returns>
    private Vector2Line MakeLine(Transform start, Transform end)
    {
        return new Vector2Line(start.position.x, start.position.z, end.position.x, end.position.z);
    }


    /// <summary>
    /// 必要なデータの作成
    /// </summary>
    private void MakeData()
    {
        leftFront = new GameObject("LeftFront").transform;
        leftFront.parent = body.transform;
        leftFront.localPosition = new Vector3(leftBack.localPosition.x, 0, rightFront.localPosition.z);

        rightBack = new GameObject("RightBack").transform;
        rightBack.parent = body.transform;
        rightBack.localPosition = new Vector3(rightFront.localPosition.x, 0, leftBack.localPosition.z);

        bodyHalfWidth = rightFront.localPosition.x;
        centerToLrLength = rightFront.transform.localPosition.magnitude;
        centerToLrSqrLength = rightFront.transform.localPosition.sqrMagnitude;
    }
}
