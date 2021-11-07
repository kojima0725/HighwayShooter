using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRail : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshCollider meshCollider;

    public void Init(float height, float length)
    {
        //メッシュ用意
        Mesh mesh = new Mesh();
        //頂点生成
        Vector3[] vertices = new Vector3[8];

        //手前下、手前上、奥下、奥上
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, height, 0);
        vertices[2] = new Vector3(0, 0, -length);
        vertices[3] = new Vector3(0, height, -length);
        vertices[4] = new Vector3(0.01f, 0, 0);
        vertices[5] = new Vector3(0.01f, height, 0);
        vertices[6] = new Vector3(0.01f, 0, -length);
        vertices[7] = new Vector3(0.01f, height, -length);
        mesh.vertices = vertices;

        //uv設定
        mesh.uv = new Vector2[] {
        new Vector2 (0, 0),
        new Vector2 (1, 0),
        new Vector2 (0, 1),
        new Vector2 (1, 1),
        new Vector2 (0, 0),
        new Vector2 (1, 0),
        new Vector2 (0, 1),
        new Vector2 (1, 1),
        };

        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3, 6, 7, 5, 5, 4, 6 };

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
