using UnityEngine;
using UnityEngine.UI;  // Jika ingin menggunakan UI untuk menampilkan waktu di layar
using UnityEngine.SceneManagement; // Untuk memuat ulang scene atau memindah ke Game Over scene

public class CountdownTimer : MonoBehaviour
{
    private float timeRemaining = 0f; // 2 menit (120 detik)
    public Text timerText; // Text UI untuk menampilkan sisa waktu, jika ingin ditampilkan
    private bool isEnemyDead = false;
    public float finalTime;
    public Text finalTimeText;
    public static CountdownTimer singleton;
    private void Awake() {
        singleton = this;
    }

    void Update()
    {
            if (!isEnemyDead)
            {
                timeRemaining += Time.deltaTime;
                UpdateTimerDisplay(timeRemaining); // Update tampilan waktu (opsional)
            } else {
                YouWin();
            }
    }

    // Method untuk update tampilan waktu pada UI
    void UpdateTimerDisplay(float currentTime)
    {
        // Tampilkan dalam format MM:SS
        timerText.text = FormatTime(currentTime);
    }

    public void YouWin(){
        if(Enemy.singleton.currentHealth <= 0 && !isEnemyDead){
            isEnemyDead = true;
            finalTime = timeRemaining;
            finalTimeText.text = "Final Time: " + FormatTime(finalTime);   
            YouWinManager.singleton.TriggerYouWin();
        }
    }

    string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
