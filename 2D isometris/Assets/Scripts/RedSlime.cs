using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : MonoBehaviour
{
    public float moveSpeed = 3f;       // Kecepatan musuh bergerak
    private Transform player;          // Referensi ke posisi pemain
    public int damageAmount = 10;      // Jumlah damage yang diberikan musuh kepada player
    public float attackCooldown = 1f;  // Waktu jeda setelah menyerang (dalam detik)
    private bool canAttack = true;     // Status apakah musuh bisa menyerang
    public int maxHealth = 30;         // Nyawa musuh
    private int currentHealth;
    public GameObject dropItemPrefab;  // Prefab item yang akan di-drop
    public float dropChance = 0.2f;    // Persentase kemungkinan drop (20%)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage: " + damageAmount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Cek apakah item akan di-drop berdasarkan persentase
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= dropChance)
        {
            DropItem();
        }

        Destroy(gameObject);
    }

    void DropItem()
    {
        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        Debug.Log("Item dropped!");
    }

    void Update()
    {
        if (player != null && canAttack)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Kurangi nyawa player
                playerHealth.TakeDamage(damageAmount);

                // Jalankan cooldown untuk serangan
                StartCoroutine(AttackCooldown());
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            transform.position += (Vector3)pushDirection * 1f;
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        moveSpeed = 0f;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        moveSpeed = 3f;
    }
}
