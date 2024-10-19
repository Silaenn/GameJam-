using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    public int maxHP = 50;
    public Image[] healthBars;
    public GameObject gameOverUI;
    public float knockbackForce = 10f;
    public float immunityTime = 3f;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;
    private int currentHP;
    private bool isImmune = false;
    private bool isGameOver = false;
    private bool isKnockedBack = false;

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    void Update()
    {
        if (!isKnockedBack && !isGameOver)
        {
            Movement();
        }
    }

    void FixedUpdate()
    {
        if (!isGameOver && !isKnockedBack)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float speed = movement.sqrMagnitude;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", speed);

        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
            animator.SetFloat("LastHorizontal", lastDirection.x);
            animator.SetFloat("LastVertical", lastDirection.y);
        }
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        if (!isImmune && currentHP > 0)
        {
            currentHP -= damage;
            UpdateHealthUI();

            if (currentHP > 0)
            {
                StartCoroutine(HandleKnockbackAndImmunity(attackDirection));
            }
            else
            {
                GameOver();
            }
        }
    }

    IEnumerator HandleKnockbackAndImmunity(Vector2 attackDirection)
    {
        isImmune = true;
        isKnockedBack = true;

        Vector2 knockbackDirection = attackDirection.normalized;
        rb.velocity = Vector2.zero; // Reset velocity before applying knockback
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        Debug.Log("Applying knockback: " + (knockbackDirection * knockbackForce));

        yield return new WaitForSeconds(0.2f); // Short delay to allow knockback to be visible
        isKnockedBack = false;

        // Blink the player
        for (int i = 0; i < 6; i++)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(immunityTime - 1.5f); // Adjust for blinking time
        isImmune = false;
    }

    void UpdateHealthUI()
    {
        int barsToShow = currentHP / (maxHP / 5);
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].enabled = i < barsToShow;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
    }
}