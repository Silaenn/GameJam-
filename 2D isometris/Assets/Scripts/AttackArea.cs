using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 7;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah objek memiliki komponen 'Enemy' atau apapun yang menerima damage
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            // Hitung arah serangan berdasarkan posisi player dan enemy
            Vector2 attackDirection = (enemy.transform.position - transform.position).normalized;

            // Berikan damage dan arah serangan kepada objek tersebut
            enemy.TakeDamage(damage, attackDirection);
        }
    }
}
