using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    public Transform player;
    private float nextAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
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
        Debug.Log("Enemy attacks!");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        
        if (playerMovement != null)
        {
            Vector2 attackDirection = (player.position - transform.position).normalized;
            playerMovement.TakeDamage(attackDamage, attackDirection);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}