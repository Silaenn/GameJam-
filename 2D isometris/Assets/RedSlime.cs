using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : MonoBehaviour
{
    public Transform player; // Referensi ke posisi pemain
    public float speed = 3.0f; // Kecepatan gerak musuh
    public float attackDistance = 2.0f; // Jarak untuk mulai menyerang
    public int HP = 30; // Health Point musuh
    public int power = 7; // Power yang mempengaruhi damage
    public float attackCooldown = 1.0f; // Jeda antar serangan
    private float lastAttackTime = 0.0f; // Waktu serangan terakhir
    public int damagePerHit = 10; // Kerusakan yang diberikan ke pemain
    public GameObject greenOrbPrefab; // Prefab untuk green orb
    private bool isDead = false;

    private void Update()
    {
        if (isDead) return; // Jika musuh mati, tidak bergerak lagi

        // Hitung jarak ke pemain
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            // Musuh berjalan mendekat ke arah pemain
            MoveTowardsPlayer();
        }
        else
        {
            // Jika jarak cukup dekat, serang pemain
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        // Serangan ke pemain
        Debug.Log("Musuh menyerang pemain!");
        // Tambahkan logika untuk mengenai pemain di sini
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerHit);
        }
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Musuh mati!");
        isDead = true;

        // Drop green orb dengan kemungkinan 20%
        if (Random.Range(0, 100) < 20)
        {
            Instantiate(greenOrbPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Hapus objek musuh
    }
}
