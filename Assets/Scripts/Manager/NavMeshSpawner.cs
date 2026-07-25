using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [Header("Préfabs")]
    [SerializeField] private GameObject[] prefabsToSpawn;

    [Header("Nombre de Spawn (Aléatoire)")]
    [SerializeField] private int minSpawnCount = 10;
    [SerializeField] private int maxSpawnCount = 20
        ;

    [Header("Paramètres de Zone")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private float maxNavMeshDistance = 2f;
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;
    [SerializeField] private Transform parentContainer;

    [Header("Automatique")]
    [SerializeField] private bool spawnOnStart = true;

    private void Start()
    {
        if (spawnCenter == null)
            spawnCenter = transform;

        if (spawnOnStart)
        {
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("[NavMeshSpawner] Aucun préfab assigné dans le tableau !");
            return;
        }

        int targetSpawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        int successCount = 0;
        int maxAttempts = targetSpawnCount * 5;
        int attempts = 0;

        while (successCount < targetSpawnCount && attempts < maxAttempts)
        {
            attempts++;

            if (TryGetRandomNavMeshPoint(out Vector3 spawnPoint))
            {
                GameObject randomPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

                Instantiate(randomPrefab, spawnPoint, Quaternion.identity, parentContainer);
                successCount++;
            }
        }

        if (successCount < targetSpawnCount)
        {
            Debug.LogWarning($"[NavMeshSpawner] Seulement {successCount}/{targetSpawnCount} objets ont pu être placés sur le NavMesh.");
        }
    }

    private bool TryGetRandomNavMeshPoint(out Vector3 result)
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += spawnCenter.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxNavMeshDistance, navMeshAreaMask))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Transform center = spawnCenter != null ? spawnCenter : transform;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center.position, spawnRadius);
    }
}