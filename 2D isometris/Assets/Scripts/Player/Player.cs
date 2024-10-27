using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    // health Player deklarsi 
    public int maxHP = 50;
    public float knockbackForce = 10f;
    public float immunityTime = 3f;
    private int currentHP;
    public bool isKnockedBack = false;
    public bool isImmune = false;


    // GameOver deklarasi
    public bool isGameOver = false;

    // Dash
    public static Player singleton;
    private void Awake() {
       if(singleton == null) {
        singleton = this;
       } else{
        Destroy(gameObject);
       }
    }

    private void Start()
    {
        currentHP = maxHP;
        PlayerHealth.singleton.maxHP = maxHP;
        PlayerHealth.singleton.UpdateHealthUI(currentHP);
        rb = GetComponent<Rigidbody2D>();
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
        int hpPerBar = maxHP / PlayerHealth.singleton.healthBars.Length;
        
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
        PlayerHealth.singleton.UpdateHealthUI(currentHP);

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

}