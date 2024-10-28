using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackArea;

public class RedSlime : MonoBehaviour, IDamageable
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
    public float minimumDistanceToOtherEnemies = 1.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
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

        PlayerHealth.singleton.currentHP+= 5;
        PlayerHealth.singleton.UpdateHealthUI(PlayerHealth.singleton.currentHP);   
    }

    void Update()
    {
        if (player != null && canAttack)
        {
            if(IsNearOtherEnemy()){
                transform.position = transform.position;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    bool IsNearOtherEnemy(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies){
            if(enemy != gameObject){
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if(distance < minimumDistanceToOtherEnemies){
                    return true;
                }
            }
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                Vector2 attackDirection = (collision.transform.position - transform.position).normalized;
                Player.singleton.TakeDamage(damageAmount, attackDirection); // Serang player
                StartCoroutine(AttackCooldown());
            }
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
