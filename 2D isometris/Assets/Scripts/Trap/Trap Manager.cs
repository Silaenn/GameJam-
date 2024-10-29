using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject trapPrefab1;   // Prefab untuk jebakan jenis 1
    public GameObject trapPrefab2;   // Prefab untuk jebakan jenis 2
    public Transform[] spawnPoints;  // Titik-titik spawn jebakan
    public int maxTraps = 5;         // Maksimal jebakan yang bisa aktif sekaligus
    public float respawnDelay = 5f;  // Waktu delay untuk respawn

    private List<GameObject> activeTraps = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnTraps());
    }

    IEnumerator SpawnTraps()
    {
        while (true)
        {
            if (activeTraps.Count < maxTraps) // Cek apakah jumlah jebakan di bawah batas maksimal
            {
                // Tentukan jebakan mana yang akan di-spawn secara acak
                GameObject trapPrefab = (Random.Range(0, 2) == 0) ? trapPrefab1 : trapPrefab2;
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Spawn jebakan di lokasi acak
                GameObject newTrap = Instantiate(trapPrefab, randomSpawnPoint.position, Quaternion.identity);
                activeTraps.Add(newTrap);

                // Tunggu sebelum respawn jebakan lagi
                yield return new WaitForSeconds(respawnDelay);
            }
            else
            {
                // Jika jumlah jebakan sudah maksimal, tunggu sebelum cek lagi
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void RemoveTrap(GameObject trap)
    {
        activeTraps.Remove(trap);  // Hapus jebakan dari daftar jebakan aktif
        Destroy(trap);             // Hancurkan jebakan
    }
}
