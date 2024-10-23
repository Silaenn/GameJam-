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
    public Image BackgorundBar;


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
        }
    }

    // Terkena damage
    public void TakeDamage(int damage, Vector2 attackDirection)
{
    if (!isImmune && currentHP > 0)
    {
        // Simpan HP sebelum damage
        int previousHP = currentHP;
        
        // Kurangi HP
        currentHP -= damage;
        if(currentHP < 0) currentHP = 0;

        // Hitung batas HP untuk setiap bar penuh
        int hpPerBar = maxHP / healthBars.Length;
        
        // Cek apakah damage menyebabkan bar penuh habis
        bool isBarDepleted = false;
        for(int i = hpPerBar; i <= previousHP; i += hpPerBar)
        {
            // Jika HP sebelumnya di atas batas bar, tapi HP sekarang di bawahnya
            if(previousHP > i && currentHP <= i)
            {
                isBarDepleted = true;
                break;
            }
        }

        // Update UI Health Bar
        UpdateHealthUI();

        if (currentHP > 0)
        {
            // Hanya lakukan knockback jika bar penuh benar-benar habis
            if (isBarDepleted)
            {
                StartCoroutine(HandleKnockbackAndImmunity(attackDirection));
            }
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
        rb.velocity = Vector2.zero; 
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        isKnockedBack = false;

        
        for (int i = 0; i < 6; i++)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(immunityTime - 1.5f); 
        isImmune = false;
    }

   void UpdateHealthUI()
{
    // Hitung proporsi health bar penuh dan sebagian
    int barsToShow = currentHP / (maxHP / healthBars.Length); 
    float partialHealth = (float)(currentHP % (maxHP / healthBars.Length)) / (maxHP / healthBars.Length);

    float originalBarWidth = healthBars[0].rectTransform.sizeDelta.x; // Ambil ukuran asli health bar pertama
    float backgroundWidth = BackgorundBar.rectTransform.sizeDelta.x;   // Ambil ukuran background bar

    for (int i = 0; i < healthBars.Length; i++)
    {
        if (i < barsToShow) // Tampilkan bar penuh
        {
            // Set kembali ukuran health bar sesuai dengan ukuran awalnya
            healthBars[i].rectTransform.sizeDelta = new Vector2(originalBarWidth, healthBars[i].rectTransform.sizeDelta.y);
            healthBars[i].enabled = true;
        }
        else if (i == barsToShow && partialHealth > 0) // Jika bar terakhir, kurangi proporsinya
        {
            // Kurangi lebar hanya pada bar terakhir sesuai dengan sisa HP
            float newWidth = partialHealth * originalBarWidth; // Tetap gunakan ukuran asli sebagai dasar
            healthBars[i].rectTransform.sizeDelta = new Vector2(newWidth, healthBars[i].rectTransform.sizeDelta.y);
            healthBars[i].enabled = true;
        }
        else // Sembunyikan bar yang tidak diperlukan
        {
            healthBars[i].enabled = false;
        }

    }
}




}