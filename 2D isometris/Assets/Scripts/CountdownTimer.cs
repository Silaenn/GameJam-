using UnityEngine;
using UnityEngine.UI;  // Jika ingin menggunakan UI untuk menampilkan waktu di layar
using UnityEngine.SceneManagement; // Untuk memuat ulang scene atau memindah ke Game Over scene

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 60f; // 2 menit (120 detik)
    public bool isGameOver = false; // Status apakah game sudah berakhir
    public Text timerText; // Text UI untuk menampilkan sisa waktu, jika ingin ditampilkan

    void Update()
    {
        if (!isGameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining); // Update tampilan waktu (opsional)
            }
            else
            {
                GameOver();
            }
        }
    }

    // Method untuk update tampilan waktu pada UI
    void UpdateTimerDisplay(float currentTime)
    {
        currentTime += 1;  // Untuk menghindari tampilan 0 sebelum waktunya habis

        // Hitung menit dan detik
        float minutes = Mathf.FloorToInt(currentTime / 60); 
        float seconds = Mathf.FloorToInt(currentTime % 60);

        // Tampilkan dalam format MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Method yang dipanggil saat waktu habis
    void GameOver()
    {
        isGameOver = true;
        GameOverManager.singleton.TriggerGameOver();
    }
}
