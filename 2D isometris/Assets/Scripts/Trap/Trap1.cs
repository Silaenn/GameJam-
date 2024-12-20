using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap1 : MonoBehaviour
{
    public int damage = 10;  // Jumlah damage yang diberikan jebakan
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah objek yang terkena jebakan adalah player
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Vector2 attackDirection = (player.transform.position - transform.position).normalized;
            // Kurangi nyawa player
            player.TakeDamage(damage, attackDirection);
            DestroyTrap();
        }
    }
    void DestroyTrap()
    {
        TrapManager.singleton.RemoveTrap(gameObject);  // Hapus jebakan dari TrapManager dan hancurkan
    }

}
