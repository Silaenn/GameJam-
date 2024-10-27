using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] healthBars;
    public int maxHP;
    private int currentHP;

    public static PlayerHealth singleton;
    private void Awake() {
        singleton = this;
    }
    public void UpdateHealthUI(int currentHP){
        this.currentHP = currentHP;

         int barsToShow = currentHP / (maxHP / healthBars.Length); 
        float partialHealth = (float)(currentHP % (maxHP / healthBars.Length)) / (maxHP / healthBars.Length);

    float originalBarWidth = healthBars[0].rectTransform.sizeDelta.x; // Ambil ukuran asli health bar pertama

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