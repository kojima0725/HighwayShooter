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

    
    [SerializeField]
    private Transform rightFront;
    [SerializeField]
    private Transform leftBack;
    private Transform leftFront;
    private Transform rightBack;

    private Vector2Line frontLine;
    private Vector2Line rightLine;
    private Vector2Line leftLine;

    /// <summary>
    /// 現在の車の速度
    /// </summary>
    private float speed;

    private float SpeedMS => KMath.KmHToMS(speed);

    public float Speed => speed;

    public Transform Body => body;

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
        
        if (RoadManager.current)
        {
            GurdrailHit hit;
            if (RoadManager.current.GurdrailHitCheck(false, rightLine, out hit))
            {
                Debug.Log("hitR");
            }
            else if (RoadManager.current.GurdrailHitCheck(true, leftLine, out hit))
            {
                Debug.Log("hitL");
            }
            else if (RoadManager.current.GurdrailHitCheck(false, frontLine, out hit))
            {
                Debug.Log("hitRF");
            }
            else if (RoadManager.current.GurdrailHitCheck(true, frontLine, out hit))
            {
                Debug.Log("hitLF");
            }
        }
        body.localPosition = body.forward * SpeedMS * Time.deltaTime;
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
        leftFront.parent = this.transform;
        leftFront.localPosition = new Vector3(leftBack.localRotation.x, 0, rightFront.localPosition.z);

        rightBack = new GameObject("RightBack").transform;
        rightBack.parent = this.transform;
        rightBack.localPosition = new Vector3(rightFront.localPosition.x, 0, leftBack.localPosition.z);
    }
}
