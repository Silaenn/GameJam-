using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    
    // Movement deklarasi
    public float moveSpeed = 5f;
    private float movementSpeedMultiplier;
    private Vector2 movement;

    // health Player deklarsi 
    public int maxHP = 50;
    public Image[] healthBars;
    public float knockbackForce = 10f;
    public float immunityTime = 3f;
    private int currentHP;
    private bool isKnockedBack = false;
    private Vector2 lastDirection = Vector2.down;
    private bool isImmune = false;

    // GameOver deklarasi
    private bool isGameOver = false;

    public static Player singleton;
    private void Awake() {
        singleton = this;
    }

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
            rb.MovePosition(rb.position + movement * moveSpeed * movementSpeedMultiplier * Time.fixedDeltaTime);
        }
    }

    // movemennt
    void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float speed = movement.sqrMagnitude;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        movementSpeedMultiplier = isRunning ? 1.5f : 1.0f;

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

    // Terkena damage
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
                GameOverManager.singleton.TriggerGameOver();
                isGameOver = true;
            }
        }
    }

    // Membuat efek dorongan dan menjadi immun selama 3 detik
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
}