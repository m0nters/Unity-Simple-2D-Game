using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPillSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject healthPillPrefab;  // Assign your Health Pill prefab
    [SerializeField] private Vector2 spawnBoundsMin = new Vector2(-11f, -6f);
    [SerializeField] private Vector2 spawnBoundsMax = new Vector2(14f, 9f);
    [SerializeField] private int maxPills = 3;  // Fewer than enemies for balance
    [SerializeField] private float spawnInterval = 5f;  // Slower than enemies
    [SerializeField] private float minSpawnDistPlayer = 3f;  // Closer OK for pills
    [SerializeField] private float spawnCheckRadius = 0.8f;  // Smaller for pills

    [Header("Layers (Same as Enemy Spawner)")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;

    private Transform playerTransform;
    private List<GameObject> activePills = new List<GameObject>();
    private Coroutine spawnCoroutine;

    private void Start()
    {
        // Find player
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player?.transform;

        if (healthPillPrefab == null)
        {
            Debug.LogError("Assign Health Pill Prefab in Inspector!");
            enabled = false;
            return;
        }

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
            // Cleanup picked up/destroyed pills
            activePills.RemoveAll(pill => pill == null);

            if (activePills.Count < maxPills && playerTransform != null)
            {
                SpawnPill();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPill()
    {
        Vector2 spawnPos = GetValidRandomPosition();
        if (spawnPos != Vector2.zero)
        {
            GameObject newPill = Instantiate(healthPillPrefab, spawnPos, Quaternion.identity);
            activePills.Add(newPill);

            // Safety despawn
            Destroy(newPill, 120f);  // 2 min timeout

            Debug.Log($"Spawned Health Pill at {spawnPos}. Total: {activePills.Count}");
        }
        else
        {
            Debug.LogWarning("No valid spawn position for pill!");
        }
    }

    private Vector2 GetValidRandomPosition()
    {
        int maxAttempts = 30;
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
        // Away from player
        if (playerTransform != null && Vector2.Distance(pos, playerTransform.position) < minSpawnDistPlayer)
            return false;

        // No obstacles
        if (Physics2D.OverlapCircle(pos, spawnCheckRadius * 0.5f, obstacleLayer) != null)
            return false;

        // No enemies
        if (Physics2D.OverlapCircle(pos, spawnCheckRadius, enemyLayer) != null)
            return false;

        return true;
    }
}