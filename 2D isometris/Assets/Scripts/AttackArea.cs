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
            // Berikan damage kepada objek tersebut
            enemy.TakeDamage(damage);
        }
    }
}
