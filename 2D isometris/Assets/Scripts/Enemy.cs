using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int attackDamage = 7;
    public float maxHealth = 30f;
    public float currentHealth;
    public RectTransform healthBarFill;       
    public Image BackgorundBar;

    public Transform player;
    private float nextAttackTime;

    public float knockbackForce = 5f;   // Kekuatan knockback
    public float immunityDuration = 1f; // Durasi imunitas setelah terkena serangan
    private bool isImmune = false;      // Status imun

    public static Enemy singleton;

    private void Awake() {
        singleton = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }

    void Update()
    {
        if (player != null && !isImmune)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        Vector2 attackDirection = (player.position - transform.position).normalized;
        Player.singleton.TakeDamage(attackDamage, attackDirection);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

     public void TakeDamage(float damage, Vector2 attackDirection)
    {
        currentHealth -= damage;  // Kurangi health
        if (currentHealth <= 0)
        {
            Die();  // Panggil metode Die jika health mencapai 0
        }
        else
        {
            Knockback(attackDirection);
            StartCoroutine(BecomeImmune());
        }
        UpdateHealthBar();  // Update tampilan health bar
    }

    // Metode untuk memperbarui health bar
    void UpdateHealthBar()
    {
        // Hitung persentase health yang tersisa
        float healthPercent = currentHealth / maxHealth;
        healthBarFill.sizeDelta = new Vector2(healthPercent * BackgorundBar.rectTransform.sizeDelta.x, healthBarFill.sizeDelta.y);
    }
    void Knockback(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = direction.normalized * -1; // Arah knockback adalah berlawanan dari serangan
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
    IEnumerator BecomeImmune()
    {
        isImmune = true; // Aktifkan imunitas
        yield return new WaitForSeconds(immunityDuration); // Tunggu selama durasi imunitas
        isImmune = false; // Matikan imunitas
    }
    public void Die()
    {
        Destroy(gameObject);
        CountdownTimer.singleton.YouWin();
    }
}