using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; 

    public static GameOverManager singleton;

    private void Awake() {
        singleton = this;
    }
    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

}
