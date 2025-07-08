using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnInterval = 2f;  // 最短待ち時間
    [SerializeField] private float maxSpawnInterval = 5f;  // 最長待ち時間
    [SerializeField] private int spawnCountPerWave = 3;    // 1回の生成数
    [SerializeField] private float minRadius = 3f;
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private int maxEnemies = 10;

    GameObject player;
    List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        // PlayerTag タグを持つオブジェクトを探す
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("PlayerTag オブジェクトが見つかりません");
            return;
        }
        // 敵生成のループを開始
        StartCoroutine(SpawnEnemiesLoop());
    }

    // 敵を生成するループ
    IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            activeEnemies.RemoveAll(e => e == null); // 死んだ敵を整理
            if (activeEnemies.Count >= maxEnemies)
                continue;

            int spawnCount = Mathf.Min(spawnCountPerWave, maxEnemies - activeEnemies.Count);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 offset = Random.onUnitSphere;
                offset.y = 0;
                offset = offset.normalized * Random.Range(minRadius, maxRadius);
                Vector3 spawnPos = player.transform.position + offset;

                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

