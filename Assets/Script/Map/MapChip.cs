using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;

    public void Init()
    {
        
    }

    public void Init(float width, float height, float makeStartX, float makeStartY, int splitX, int splitY)
    {
        MakeMesh(width, height, splitX, splitY);
    }

    private void MakeMesh(float width, float height, int splitX, int splitY)
    {

        //メッシュ生成用意
        Mesh mesh = new Mesh();
        //頂点生成
        //Vector3[] vertices = new Vector3[4];
        //float cosWidth = Mathf.Cos(Mathf.Deg2Rad * rotate.y) * halfWidth;
        //float sinWidth = Mathf.Sin(Mathf.Deg2Rad * rotate.y) * halfWidth;

        //vertices[0] = new Vector3(-cosWidth, 0, -sinWidth);
        //vertices[1] = new Vector3(cosWidth, 0, sinWidth);
        //vertices[2] = new Vector3(-halfWidth, 0, length);
        //vertices[3] = new Vector3(halfWidth, 0, length);
        //mesh.vertices = vertices;

        //頂点情報を使ってガードレールも生成する
        //MakeGuardrail(vertices);

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
}
