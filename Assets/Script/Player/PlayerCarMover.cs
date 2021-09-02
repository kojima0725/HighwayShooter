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

    private float centerToLrSqrLength;

    private float speed;

    private float SpeedMS => KMath.KmHToMS(speed);

    public float Speed => speed;

    public Transform Body => body;

    private Vector2 BodyPos => new Vector2(body.position.x, body.position.z);
    private Vector2Line CenterToLeftLine => new Vector2Line(BodyPos, new Vector2(leftFront.position.x, leftFront.position.z));
    private Vector2Line CenterToRightLine => new Vector2Line(BodyPos, new Vector2(rightFront.position.x, rightFront.position.z));



    private void Awake()
    {
        MakeData();
    }

    private void Start()
    {
        speed = PlayerDataBase.PlayerCarData.FirstSpeed;
    }

    private void Update()
    {
        //当たり判定の描画
        Debug.DrawLine(transform.position + Vector3.up, transform.position + leftFront.position + Vector3.up, Color.blue);
        Debug.DrawLine(transform.position + Vector3.up, transform.position + rightFront.position + Vector3.up, Color.blue);
    }

    public void MoveUpdate()
    {
        //データ更新
        DataUpdate();
        //入力更新
        driver.DriverUpdate();
        //車の速度変更
        ChangeCarSpeed();
        //車の移動
        MoveCar();
        //車回転
        RollBody();
        //車の座標修正
        SwipeCarAndWorld();
    }

    /// <summary>
    /// 毎フレーム走るデータ更新処理
    /// </summary>
    private void DataUpdate()
    {
        
    }


    /// <summary>
    /// 車体を傾ける
    /// </summary>
    private void RollBody()
    {
        Quaternion old = body.transform.rotation;
        Vector3 roll = body.transform.localEulerAngles;
        float y = driver.HandleInput * PlayerDataBase.PlayerCarData.RollSensitivity * Time.deltaTime;
        roll.y += y;
        body.transform.localEulerAngles = roll;
        GurdrailHit hit;
        if (y < 0 && RoadManager.current.GurdrailHitCheck(true, MakeLine(leftFront, leftBack), out hit))
        {
            body.transform.rotation = old;
        }
        else if (y > 0 && RoadManager.current.GurdrailHitCheck(true, MakeLine(rightFront, rightBack), out hit))
        {
            body.transform.rotation = old;
        }
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
            if (RoadManager.current.GurdrailHitCheck(false, CenterToRightLine, out hit))
            {
                HittingToRight(hit);
            }
            else if (RoadManager.current.GurdrailHitCheck(true, CenterToLeftLine, out hit))
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
            if (KMath.LineToLineCollision(hit.hitLine, CenterToLeftLine, out cornerHit))
            {
                float sqrKyori = kyori * kyori;
                float nokori = Mathf.Sqrt(centerToLrSqrLength - sqrKyori);
                if (nokori >= 0)
                {
                    Vector2 rollTo = -hit.hitChip.GurdrailLeftNomal * kyori + hit.hitLine.vector.normalized * nokori;
                    Vector2 from = CenterToLeftLine.vector;
                    Quaternion quaternion = Quaternion.FromToRotation(new Vector3(from.x, 0, from.y), new Vector3(rollTo.x, 0, rollTo.y));
                    body.rotation *= quaternion;
                    ZuriLeft(hit);
                }
            }
        }
    }

    /// <summary>
    /// 左のガードレールにぶつかった時の処理
    /// </summary>
    /// <param name="hit">衝突地点の情報</param>
    private void HittingToRight(GurdrailHit hit)
    {
        //ガードレールと車体の中心の距離が一定以下になっていないか調べる
        float kyori = Vector2.Dot(hit.hitChip.GurdrailRightNomal, BodyPos - hit.hitLine.end);
        if (kyori < bodyHalfWidth)
        {
            //車を一旦後ろ(ガードレールの中)に戻し、そこから戻した距離壁に沿って移動する
            body.position += new Vector3(hit.hitChip.GurdrailRightNomal.x, 0, hit.hitChip.GurdrailRightNomal.y) * (-kyori + bodyHalfWidth);
            body.rotation = Quaternion.LookRotation(new Vector3(hit.hitLine.vector.x, 0, hit.hitLine.vector.y), Vector3.up);
        }
        else
        {
            //ぶつかっているが、回転だけでどうにかなるので回転だけでどうにかする
            Vector2 cornerHit;
            if (KMath.LineToLineCollision(hit.hitLine, CenterToRightLine, out cornerHit))
            {
                float sqrKyori = kyori * kyori;
                float nokori = Mathf.Sqrt(centerToLrSqrLength - sqrKyori);
                if (nokori >= 0)
                {
                    Vector2 rollTo = -hit.hitChip.GurdrailRightNomal * kyori + hit.hitLine.vector.normalized * nokori;
                    Vector2 from = CenterToRightLine.vector;
                    Quaternion quaternion = Quaternion.FromToRotation(new Vector3(from.x, 0, from.y), new Vector3(rollTo.x, 0, rollTo.y));
                    body.rotation *= quaternion;
                    ZuriRight(hit);
                }
            }
        }
    }

    private void ZuriLeft(GurdrailHit hit)
    {
        //道路が曲がっているときは角度を補完する
        Transform center;
        if (center = hit.hitChip.Center)
        {
            Vector3 vector = body.transform.position - center.position;
            //センターと自身の位置から、角度の計算
            float angle = Mathf.Atan2(vector.x, vector.z);
            Quaternion rotate = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);
            Quaternion rightLeft = hit.hitChip.IsCenterInRight ? Quaternion.AngleAxis(90, Vector3.up) : Quaternion.AngleAxis(-90, Vector3.up);
            Quaternion result = rotate * rightLeft;
            if (Vector3.Cross(body.forward, result * Vector3.forward).y >= 0)
            {
                body.rotation = result;
            }
        }
    }

    private void ZuriRight(GurdrailHit hit)
    {
        //道路が曲がっているときは角度を補完する
        Transform center;
        if (center = hit.hitChip.Center)
        {
            Vector3 vector = body.transform.position - center.position;
            //センターと自身の位置から、角度の計算
            float angle = Mathf.Atan2(vector.x, vector.z);
            Quaternion rotate = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);
            Quaternion rightLeft = hit.hitChip.IsCenterInRight ? Quaternion.AngleAxis(90, Vector3.up) : Quaternion.AngleAxis(-90, Vector3.up);
            Quaternion result = rotate * rightLeft;
            if (Vector3.Cross(body.forward, result * Vector3.forward).y <= 0)
            {
                body.rotation = result;
            }
        }
    }


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
        centerToLrSqrLength = rightFront.transform.localPosition.sqrMagnitude;
    }
}
