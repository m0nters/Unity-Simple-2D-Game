using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;  // Assign your Enemy prefab
    [SerializeField] private Vector2 spawnBoundsMin = new Vector2(-10f, -6f);
    [SerializeField] private Vector2 spawnBoundsMax = new Vector2(10f, 6f);
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnInterval = 2f;  // Seconds between attempts
    [SerializeField] private float minSpawnDistPlayer = 4f;  // Avoid spawning near player
    [SerializeField] private float spawnCheckRadius = 1.2f;  // Clear radius for spawn

    [Header("Layers (Setup Required)")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;

    private Transform playerTransform;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;

    private void Start()
    {
        // Find player
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player?.transform;

        // Start spawning loop
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Cleanup dead enemies first
            activeEnemies.RemoveAll(enemy => enemy == null);

            if (activeEnemies.Count < maxEnemies && playerTransform != null)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPos = GetValidRandomPosition();
        if (spawnPos != Vector2.zero)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(newEnemy);

            // Safety despawn if stuck/lost
            Destroy(newEnemy, 60f);

            Debug.Log($"Spawned enemy at {spawnPos}. Total: {activeEnemies.Count}");
        }
        else
        {
            Debug.LogWarning("No valid spawn position found!");
        }
    }

    private Vector2 GetValidRandomPosition()
    {
        int maxAttempts = 30;  // Prevent infinite loop
        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(spawnBoundsMin.x, spawnBoundsMax.x);
            float y = Random.Range(spawnBoundsMin.y, spawnBoundsMax.y);
            Vector2 pos = new Vector2(x, y);

            if (IsValidSpawnPosition(pos))
                return pos;
        }
        return Vector2.zero;
    }

    private bool IsValidSpawnPosition(Vector2 pos)
    {
        // 1. Away from player
        if (playerTransform != null && Vector2.Distance(pos, playerTransform.position) < minSpawnDistPlayer)
            return false;

        // 2. No overlap with obstacles
        if (Physics2D.OverlapCircle(pos, spawnCheckRadius * 0.5f, obstacleLayer) != null)
            return false;

        // 3. No overlap with other enemies
        if (Physics2D.OverlapCircle(pos, spawnCheckRadius, enemyLayer) != null)
            return false;

        return true;
    }
}