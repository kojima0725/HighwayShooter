using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarBody : MonoBehaviour
{
    [SerializeField]
    PlayerCar car;

    public PlayerCar Car => car;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "NPC")
        {
            Transform npcTransform = KMath.GetRoot(collision.transform, "NPC");
            NomalCar npc = npcTransform.GetComponent<NomalCar>();
            if (npc)
            {
                Vector3 move = transform.forward * car.SpeedMS * 1.3f;
                move.y += Random.Range(1.0f, 4.0f);
                npc.OnHit(move);
            }
        }
    }
}
