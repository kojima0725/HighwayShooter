using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;

    float width;
    float height;
    int splitX;
    int splitY;
    float noiseLoop;
    Vector2 noizeStartPos;
    

    public void Init(float width, float height, int splitX, int splitY, float noiseLoop, float noizePosX, float noizePosY)
    {
        this.width = width;
        this.height = height;
        this.splitX = splitX;
        this.splitY = splitY;
        this.noiseLoop = noiseLoop;
        this.noizeStartPos = new Vector2(noizePosX, noizePosY);
        MakeMesh();
    }

    private void MakeMesh()
    {
        //メッシュ生成用意
        Mesh mesh = new Mesh();

        //頂点生成
        int xCount = splitX * 2 + 1;
        int yCount = splitY * 2 + 1;
        float xSize = width / (splitX * 2);
        float ySize = height / (splitY * 2);
        int verticsCount = xCount * yCount;
        Vector3[] vertices = new Vector3[verticsCount];
        Vector2[] uvs = new Vector2[verticsCount];
        float posX;
        float posY = -height / 2;

        int index = 0;
        bool p = false;
        for (int y = 0; y < yCount; y++)
        {
            posX = -width / 2;
            posX += p ? xSize / 2 : 0;
            for (int x = 0; x < xCount; x++)
            {
                vertices[index] = new Vector3(posX, Noize(posX, posY) ,posY);
                uvs[index] = new Vector2((posX + width / 2) / width, (posY + height / 2) / height);
                posX += xSize;
                index++;
            }
            posY += ySize;
            p = !p;
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;

        int doubleX = splitX * 2;
        int doubleY = splitY * 2;
        int[] triangles = new int[3 * doubleX * 2 * doubleY];
        int tIndex = 0;
        int bIndex = 0;
        for (int y = 0; y < doubleY; y++)
        {
            for (int x = 0; x < doubleX; x++)
            {
                Inport(bIndex);
                Inport(bIndex + doubleX + 1);
                Inport(bIndex + 1);
                Inport(bIndex + 1);
                Inport(bIndex + doubleX + 1);
                Inport(bIndex + doubleX + 2);
                bIndex++;
            }
            bIndex++;
        }
        void Inport(int a)
        {
            triangles[tIndex] = a;
            tIndex++;
        }


        mesh.triangles = triangles;

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private Vector2 NoizePos(float x, float y)
    {
        float yoko = (noizeStartPos.x + x / noiseLoop) % 1 * 256;
        float tate = (noizeStartPos.y + y / noiseLoop) % 1 * 256;
        return new Vector2(yoko, tate);
    }

    private float Noize(float x, float y)
    {
        Vector2 pos = NoizePos(x, y);
        return Mathf.PerlinNoise(pos.x, pos.y) * 50;
    }
}
