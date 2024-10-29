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
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
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
        if (player != null && canAttack)
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
