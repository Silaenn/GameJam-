using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2 : MonoBehaviour
{
    public float stunDuration = 3f;  // Durasi stun

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.GetStunned(stunDuration);
            DestroyTrap();
        }
    }

    void DestroyTrap()
    {
        TrapManager.singleton.RemoveTrap(gameObject);  // Hapus jebakan dari TrapManager
    }
}
