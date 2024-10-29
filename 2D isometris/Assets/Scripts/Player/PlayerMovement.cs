using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    public float moveSpeed = 5f;
    private float movementSpeedMultiplier;

    public float dashDistance = 5f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1f;
    public int staminaCost = 30;
    private bool isDashing = false;
    private float lastDashTime = 0f;
    private Vector2 dashDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movementSpeedMultiplier = 1.0f;
    }

    private void Update() {
        if(!Player.singleton.isKnockedBack && !Player.singleton.isGameOver && !Player.singleton.isStunned){
            HandleMovementInput();
            HandleDashInput();
        }
    }

    private void FixedUpdate() {
        if(!Player.singleton.isGameOver && !Player.singleton.isKnockedBack && !isDashing && !Player.singleton.isStunned){
            rb.MovePosition(rb.position + movement * moveSpeed * movementSpeedMultiplier * Time.fixedDeltaTime);
        }
    }

    void HandleMovementInput(){
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        movement.x = horizontalInput;
        movement.y = verticalInput;

        float speed = movement.sqrMagnitude;

        if(speed > 1){
            movement = movement.normalized; 
        }
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && PlayerStamina.singleton.currentStamina > 0;
        movementSpeedMultiplier = isRunning ? 1.5f : 1.0f;

         if (isRunning){
            PlayerStamina.singleton.UseStamina(PlayerStamina.singleton.staminaRunCost * Time.deltaTime);
        } else {
            PlayerStamina.singleton.RegenerateStamina();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", speed);
        

        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
            animator.SetFloat("LastHorizontal", lastDirection.x);
            animator.SetFloat("LastVertical", lastDirection.y);

            PlayerAttack.singleton.SetLastDirection(lastDirection);
            Player.singleton.SetLastDirection(lastDirection);
        }
    }

     private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown && PlayerStamina.singleton.currentStamina >= staminaCost)
        {
            dashDirection = movement.normalized;
            if (dashDirection != Vector2.zero)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        PlayerStamina.singleton.UseStamina(staminaCost);

        lastDashTime = Time.time;

        isDashing = true;
        Player.singleton.isImmune = true;

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        Vector2 startPos = rb.position;
        Vector2 dashTarget = startPos + dashDirection * dashDistance;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            rb.MovePosition(Vector2.Lerp(startPos, dashTarget, elapsed / dashDuration));
            yield return null;
        }

        collider.enabled = true;
        isDashing = false;
        Player.singleton.isImmune = false;
    }
    // PlayerMovement script
    public void TakeDamage(int damageAmount, Vector2 attackDirection)
    {
        // Logika pengurangan nyawa player
        Player.singleton.currentHP -= damageAmount;

        // Cek apakah player mati
        if (Player.singleton.currentHP <= 0)
        {
            Player.singleton.isGameOver = true;
            Debug.Log("Player died!");
            // Mungkin tambahkan animasi kematian atau logika game over
        }
        else
        {
            // Jika player masih hidup, lakukan knockback atau efek lain
            StartCoroutine(HandleKnockback(attackDirection));
        }
    }

    // Coroutine untuk mengatasi knockback setelah terkena serangan
    private IEnumerator HandleKnockback(Vector2 direction)
    {
        Player.singleton.isKnockedBack = true;
        float knockbackDuration = 0.2f; // Atur durasi knockback
        Vector2 knockbackTarget = rb.position + direction * 2f; // Atur jarak knockback

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            rb.MovePosition(Vector2.Lerp(rb.position, knockbackTarget, elapsed / knockbackDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        Player.singleton.isKnockedBack = false;
    }

}
