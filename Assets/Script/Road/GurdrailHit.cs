using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GurdrailHit
{
    public Vector2Line hitLine;
    public RoadChip hitChip;
    public Vector2 hitPos;

    public GurdrailHit( Vector2Line hitLine, RoadChip hitChip, Vector2 hitPos) 
    {
        this.hitLine = hitLine;
        this.hitChip = hitChip;
        this.hitPos = hitPos;
    }
}
