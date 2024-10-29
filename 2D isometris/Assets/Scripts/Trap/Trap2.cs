using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2 : MonoBehaviour
{
    public float stunDuration = 3f;  // Durasi stun
    public TrapManager trapManager;  // Referensi ke TrapManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            // Berikan damage kepada player
           

            // Tambahkan efek stun ke player
            player.GetStunned(stunDuration);

            Debug.Log("Player terkena jebakan 2 dan terkena stun selama " + stunDuration + " detik!");
            DestroyTrap();
        }
    }

    void DestroyTrap()
    {
        trapManager.RemoveTrap(gameObject);  // Hapus jebakan dari TrapManager
    }
}
