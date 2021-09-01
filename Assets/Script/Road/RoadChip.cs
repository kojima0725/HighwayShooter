using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 道路本体、自動で生成され続ける
/// </summary>
public class RoadChip : MonoBehaviour
{
    [SerializeField]
    private Transform end;
    [SerializeField]
    private MeshFilter meshFilter;
    private Transform[] lanes;
    private Transform center;
    private RoadChip nextChip = null;
    private RoadChip prevChip = null;

    private MapChip mapChipL;
    public Transform MapChipLEnd => mapChipL.End;
    private MapChip mapChipRE;
    public Transform MapChipREnd => mapChipRE.End;


    /// <summary>
    /// 道の終端の位置
    /// </summary>
    /// <returns></returns>
    public Transform End => end;
    public RoadChip Next { get => nextChip; set => nextChip = value; }
    public RoadChip Prev => prevChip;
    /// <summary>
    /// カーブの中心点(直線の場合はnull)
    /// </summary>
    public Transform Center { get => center; set => center = value; }
    /// <summary>
    /// カーブ時の中心点が右側にあるかどうか
    /// </summary>
    public bool IsCenterInRight { get; set; }
    /// <summary>
    /// 指定されたレーンの位置を返す
    /// </summary>
    /// <param name="lane">レーンの番号(0～)</param>
    /// <returns></returns>
    public Transform GetLanePos(int lane) => lanes[lane % lanes.Length];
    private readonly List<ICar> cars = new List<ICar>();
    public List<ICar> Cars => cars;

    #region GURDRAIL

    /// <summary>
    /// ガードレールの端の座標(左手前、右手前)
    /// </summary>
    private Transform[] gurdralis = new Transform[2];

    private Vector3 gurdLeftVector;
    private Vector3 gurdRightVector;

    private Vector2 gurdLeftNomal;
    private bool haveLeftNomal;
    private Vector2 gurdRightNomal;
    private bool haveRightNomal;

    /// <summary>
    /// 左ガードレールの終端
    /// </summary>
    public Transform GurdrailLeft => gurdralis[0];
    public Vector3 GurdrailLeftVector => gurdLeftVector;
    public Vector2 GurdrailLeftNomal => haveLeftNomal ? gurdLeftNomal : MakeLeftNomal();

    /// <summary>
    /// 右ガードレールの終端
    /// </summary>
    public Transform GurdrailRight => gurdralis[1];
    public Vector3 GurdrailRightVector => gurdRightVector;
    public Vector2 GurdrailRightNomal => haveRightNomal ? gurdRightNomal : MakeRightNomal();

    /// <summary>
    /// ガードレールの終端
    /// </summary>
    /// <param name="LR">true=左, false=右</param>
    public Transform Gurdrail(bool LR) => LR ? GurdrailLeft : GurdrailRight;

    #endregion


    /// <summary>
    /// 左下、右下、左上、右上
    /// </summary>
    public Vector3[] Vertices => meshFilter.mesh.vertices;



    /// <summary>
    /// 道路の初期化(メッシュの設置、各種調整)
    /// </summary>
    /// <param name="rotate">生成位置からどれぐらい曲げるか</param>
    /// <param name="length">チップの長さ</param>
    /// <param name="width">チップの幅</param>
    /// <param name="lane">車線数</param>
    public void Init(Vector3 rotate, float length, float width, int lane, RoadChip prev = null)
    {
        //ケツを設定
        prevChip = prev;

        //End位置の設定
        end.localPosition = new Vector3(0,0,length);
        end.localRotation = Quaternion.identity;

        float halfWidth = width / 2;

        //道路のメッシュ作成
        MakeMeshAndGurdrail(rotate, length, halfWidth);

        //レーンに対応した位置を作成
        MakeLanes(lane, length);

        //自身を曲げる
        this.transform.Rotate(rotate);

        //ガードレールのベクトルを作成しておく
        MakeGuardVectors();
    }

    public void SetMap(bool LR, MapChip chip)
    {
        chip.transform.parent = this.transform;
        int lrIndex = LR ? 0 : 1;
        chip.transform.localPosition = meshFilter.mesh.vertices[lrIndex];
        switch (LR)
        {
            case true:
                mapChipL = chip;
                break;
            case false:
                mapChipRE = chip;
                break;
        }
    }

    public void Join(ICar car)
    {
        cars.Add(car);
    }

    public void Leave(ICar car)
    {
        cars.Remove(car);
    }

    /// <summary>
    /// 道路のメッシュを作成する
    /// </summary>
    /// <param name="rotate"></param>
    /// <param name="length"></param>
    /// <param name="halfWidth"></param>
    private void MakeMeshAndGurdrail(Vector3 rotate, float length, float halfWidth)
    {
        //メッシュ生成用意
        Mesh mesh = new Mesh();
        //頂点生成
        Vector3[] vertices = new Vector3[4];
        float cosWidth = Mathf.Cos(Mathf.Deg2Rad * rotate.y) * halfWidth;
        float sinWidth = Mathf.Sin(Mathf.Deg2Rad * rotate.y) * halfWidth;

        //左下、右下、左上、右上
        vertices[0] = new Vector3(-cosWidth, 0, -sinWidth);
        vertices[1] = new Vector3(cosWidth, 0, sinWidth);
        vertices[2] = new Vector3(-halfWidth, 0, length);
        vertices[3] = new Vector3(halfWidth, 0, length);
        mesh.vertices = vertices;

        //頂点情報を使ってガードレールも生成する
        MakeGuardrail(vertices);

        //uv設定
        mesh.uv = new Vector2[] {
        new Vector2 (0, 0),
        new Vector2 (1, 0),
        new Vector2 (0, 1),
        new Vector2 (1, 1),
        };

        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    /// <summary>
    /// ガードレールの設定をする
    /// </summary>
    /// <param name="points">メッシュの四隅の座標</param>
    private void MakeGuardrail(Vector3[] points)
    {
        gurdralis[0] = new GameObject("leftGurd").transform;
        gurdralis[0].parent = this.transform;
        gurdralis[0].localPosition = points[2];

        gurdralis[1] = new GameObject("rightGurd").transform;
        gurdralis[1].parent = this.transform;
        gurdralis[1].localPosition = points[3];
    }

    /// <summary>
    /// ガードレールの方向ベクトルを作成する
    /// </summary>
    private void MakeGuardVectors()
    {
        if (prevChip)
        {
            gurdLeftVector = gurdralis[0].position - prevChip.gurdralis[0].position;
            gurdRightVector = gurdralis[1].position - prevChip.gurdralis[1].position;
        }
    }

    /// <summary>
    /// 左側のガードレールの法線ベクトルを作成する
    /// </summary>
    /// <returns></returns>
    private Vector2 MakeLeftNomal()
    {
        Vector3 nomal = Vector3.Cross(Vector3.up, gurdLeftVector).normalized;
        Vector2 two = new Vector2(nomal.x, nomal.z);
        gurdLeftNomal = two;
        haveLeftNomal = true;
        return two;
    }

    /// <summary>
    /// 右側のガードレールの法線ベクトルを作成する
    /// </summary>
    /// <returns></returns>
    private Vector2 MakeRightNomal()
    {
        Vector3 nomal = Vector3.Cross(gurdRightVector, Vector3.up).normalized;
        Vector2 two = new Vector2(nomal.x, nomal.z);
        gurdRightNomal = two;
        haveRightNomal = true;
        return two;
    }

    /// <summary>
    /// レーンを作る
    /// </summary>
    /// <param name="lane"></param>
    private void MakeLanes(int lane, float length)
    {
        //レーン作成
        lanes = new Transform[lane];

        for (int i = 0; i < lane; i++)
        {
            Transform laneTransform = new GameObject().transform;
            laneTransform.parent = this.transform;
            laneTransform.localPosition = new Vector3(StageDatabase.RoadData.LanePosOffsets[i],0,length);
            laneTransform.localRotation = Quaternion.identity;
            lanes[i] = laneTransform;
        }

        //テクスチャの幅をレーンに合わせる
        GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(lane + 1, 1);
    }
}
