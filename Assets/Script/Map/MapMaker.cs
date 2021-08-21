using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private MapChip mapChipPrefab;

    private MapNoizeManager noizeManager = new MapNoizeManager();

    private Vector2 l;
    private Vector2 r;

    public MapChip MakeChip()
    {
        MapData d = StageDatabase.MapData;
        MapChip maked = Instantiate(mapChipPrefab);
        maked.Init(d.ChipWidth, d.ChipHeight, d.SplitX, d.SplitY, d.NoiseLoopSize, 1000, 1000);
        return maked;
    }

    public void MakeMap(List<RoadChip> chips)
    {
        MapData d = StageDatabase.MapData;
        int listLength = chips.Count;
        for (int i = 0; i < listLength; i++)
        {
            RoadChip item = chips[i];
            Vector3[] wa = item.Vertices;
            Vector2[] wb = new Vector2[4];
            for (int j = 0; j < 4; j++)
            {
                wb[j] = new Vector2(wa[j].x, wa[j].z);
            }

            MapChip maked = Instantiate(mapChipPrefab);
            Vector2 leftGurd = new Vector2(item.GurdrailLeftVector.x, item.GurdrailLeftVector.z);
            maked.Init(leftGurd, -d.ChipWidth, d.SplitX, noizeManager.noiseStartPos, d.NoiseLoopSize, l, item.GurdrailLeftNomal, true);
            item.SetMap(true, maked);
            l = item.GurdrailLeftNomal;

            noizeManager.Move(wb[2] - wb[0]);

            maked = Instantiate(mapChipPrefab);
            Vector2 rightGurd = new Vector2(item.GurdrailRightVector.x, item.GurdrailRightVector.z);
            maked.Init(rightGurd, d.ChipWidth, d.SplitX, noizeManager.noiseStartPos, d.NoiseLoopSize, r , -item.GurdrailRightNomal, false);
            item.SetMap(false, maked);
            r = -item.GurdrailRightNomal;

            //noizeManager.Move(wb[2] - wb[1]);

        }
    }
}
