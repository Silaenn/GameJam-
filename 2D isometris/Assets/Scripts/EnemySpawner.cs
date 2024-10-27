using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab musuh yang akan di-spawn
    public float spawnInterval = 3f; // Interval waktu antar spawn
    public float spawnDistance = 10f; // Jarak spawn dari kamera

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;  // Mengambil referensi kamera utama
        StartCoroutine(SpawnEnemy()); // Memulai proses spawn secara berkala
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // Menunggu sebelum spawn berikutnya
            Spawn();
        }
    }

    void Spawn()
    {
        // Mendapatkan posisi acak di luar kamera
        Vector2 spawnPosition = GetRandomPositionOutsideCamera();

        // Spawn musuh di posisi acak
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector2 GetRandomPositionOutsideCamera()
    {
        // Mendapatkan batas kamera
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Tentukan sisi mana yang akan digunakan untuk spawn (atas, bawah, kiri, kanan)
        int randomSide = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (randomSide)
        {
            case 0: // Kiri
                spawnPosition = new Vector2(-screenBounds.x - spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 1: // Kanan
                spawnPosition = new Vector2(screenBounds.x + spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 2: // Atas
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spawnDistance);
                break;
            case 3: // Bawah
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spawnDistance);
                break;
        }

        return spawnPosition;
    }
}
