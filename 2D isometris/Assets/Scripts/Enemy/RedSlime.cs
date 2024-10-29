using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackArea;
using UnityEngine.UI;

public class RedSlime : MonoBehaviour, IDamageable
{
    public float moveSpeed = 3f;       
    private Transform player;          
    public int damageAmount = 10;      
    public float attackCooldown = 1f;  
    private bool canAttack = true;     
    public int maxHealth = 30;         
    private int currentHealth;
    public RectTransform healthBarFill;       
    public Image BackgorundBar;
    public GameObject dropItemPrefab;  
    public float dropChance = 0.2f;    
    public float minimumDistanceToOtherEnemies = 1.5f;
    public float knockbackForce = 10f;   // Kekuatan knockback
    public float immunityDuration = 1f; // Durasi imunitas setelah terkena serangan
    private bool isImmune = false;      // Status imun
    private bool isKnockedBack = false;
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }  else
        {
            Knockback(attackDirection);
            StartCoroutine(BecomeImmune());
        }
        UpdateHealthBar();
    }

     void Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = direction.normalized;
            rb.velocity = Vector2.zero; 
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        StartCoroutine(EndKnockback());
    }

    IEnumerator EndKnockback(){
        yield return new WaitForSeconds(0.5f);
        isKnockedBack = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = Vector2.zero;
    }
    }
    IEnumerator BecomeImmune()
    {
        isImmune = true; // Aktifkan imunitas
        yield return new WaitForSeconds(immunityDuration); // Tunggu selama durasi imunitas
        isImmune = false; // Matikan imunitas
    }

    void Die()
    {
        // Cek apakah item akan di-drop berdasarkan persentase
        float randomValue = Random.Range(0f, 1f);
        // if (randomValue <= dropChance)
        // {
            DropItem();
        // }

        Destroy(gameObject);
    }

    void DropItem()
    {
        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (player != null && canAttack && !isImmune && !isKnockedBack)
        {
            if(IsNearOtherEnemy()){
                transform.position = transform.position;
            }
            transform.position = Vector2.SmoothDamp(transform.position, player.position, ref velocity, 0.3f, moveSpeed);
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

     void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBarFill.sizeDelta = new Vector2(healthPercent * BackgorundBar.rectTransform.sizeDelta.x, healthBarFill.sizeDelta.y);
    }
}
