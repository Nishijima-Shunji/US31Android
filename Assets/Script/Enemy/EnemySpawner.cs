using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnInterval = 2f;  // �ŒZ�҂�����
    [SerializeField] private float maxSpawnInterval = 5f;  // �Œ��҂�����
    [SerializeField] private int spawnCountPerWave = 3;    // 1��̐�����
    [SerializeField] private float minRadius = 3f;
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private int maxEnemies = 10;

    GameObject player;
    List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        // PlayerTag �^�O�����I�u�W�F�N�g��T��
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("PlayerTag �I�u�W�F�N�g��������܂���");
            return;
        }
        // �G�����̃��[�v���J�n
        StartCoroutine(SpawnEnemiesLoop());
    }

    // �G�𐶐����郋�[�v
    IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            activeEnemies.RemoveAll(e => e == null); // ���񂾓G�𐮗�
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

