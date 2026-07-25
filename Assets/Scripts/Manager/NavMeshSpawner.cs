using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnGroup
    {
        public string groupName = "Nouveau Groupe";

        public GameObject[] prefabsToSpawn;

        [Header("Quantité")]
        [Min(0)] public int minSpawnCount = 1;
        [Min(0)] public int maxSpawnCount = 5;
    }

    [Header("Groupes de Spawns")]
    [SerializeField] private List<SpawnGroup> spawnGroups = new List<SpawnGroup>();

    [Header("Paramètres de Zone")]
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;
    [SerializeField] private Transform parentContainer;

    [Header("Automatique")]
    [SerializeField] private bool spawnOnStart = true;

    private NavMeshTriangulation navMeshTriangulation;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        if (spawnGroups == null || spawnGroups.Count == 0)
        {
            Debug.LogWarning("[NavMeshSpawner] Aucun groupe de spawn configuré !");
            return;
        }

        navMeshTriangulation = NavMesh.CalculateTriangulation();

        if (navMeshTriangulation.vertices.Length == 0)
        {
            Debug.LogError("[NavMeshSpawner] Aucun NavMesh trouvé dans la scène ! Avez-vous 'Bake' votre NavMesh ?");
            return;
        }

        foreach (var group in spawnGroups)
        {
            SpawnGroupObjects(group);
        }
    }

    private void SpawnGroupObjects(SpawnGroup group)
    {
        if (group.prefabsToSpawn == null || group.prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning($"[NavMeshSpawner] Aucun préfab assigné dans le groupe '{group.groupName}' !");
            return;
        }

        int targetSpawnCount = Random.Range(group.minSpawnCount, group.maxSpawnCount + 1);

        if (targetSpawnCount <= 0) return;

        int successCount = 0;

        for (int i = 0; i < targetSpawnCount; i++)
        {
            if (TryGetPointOnEntireNavMesh(out Vector3 spawnPoint))
            {
                int randomIndex = Random.Range(0, group.prefabsToSpawn.Length);
                GameObject prefabToInstantiate = group.prefabsToSpawn[randomIndex];

                Instantiate(prefabToInstantiate, spawnPoint, Quaternion.identity, parentContainer);
                successCount++;
            }
        }

        Debug.Log($"[NavMeshSpawner] Groupe '{group.groupName}' : Cible = {targetSpawnCount} | Spawns réussis = {successCount}");
    }

    private bool TryGetPointOnEntireNavMesh(out Vector3 result)
    {
        int totalTriangles = navMeshTriangulation.indices.Length / 3;

        if (totalTriangles == 0)
        {
            result = Vector3.zero;
            return false;
        }

        int randomTriangleIndex = Random.Range(0, totalTriangles) * 3;

        Vector3 vertexA = navMeshTriangulation.vertices[navMeshTriangulation.indices[randomTriangleIndex]];
        Vector3 vertexB = navMeshTriangulation.vertices[navMeshTriangulation.indices[randomTriangleIndex + 1]];
        Vector3 vertexC = navMeshTriangulation.vertices[navMeshTriangulation.indices[randomTriangleIndex + 2]];

        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;

        result = (1 - r1) * vertexA + (r1 * (1 - r2)) * vertexB + (r1 * r2) * vertexC;
        return true;
    }
}