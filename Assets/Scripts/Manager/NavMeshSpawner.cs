using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [System.Serializable]
    public class DifficultySpawnData
    {
        [Min(0)] public int minSpawnCount = 1;
        [Min(0)] public int maxSpawnCount = 5;
    }
    [System.Serializable]
   public class SpawnGroup
   {
       public string groupName = "Nouveau Groupe";
   
       public GameObject[] prefabsToSpawn;
      
   
       public DifficultySpawnData easy;
       public DifficultySpawnData normal;
       public DifficultySpawnData hard;
       public DifficultySpawnData chicken;
   }
   
    [System.Serializable]
    public class DifficultyTrashData
    {
        public int easy = 3;
        public int normal = 2;
        public int hard = 2;
        public int chicken = 4;
    }
    [SerializeField] private DifficultyTrashData trashData;
    
    [System.Serializable]
    public class DifficultyTime
    {
        public float easy = 75f;
        public float normal = 90f;
        public float hard = 120f;
        public float chicken = 100f;
    }
    
    [System.Serializable]
    public class DifficultyEventTime
    {
        public EventTimeData easy;
        public EventTimeData normal;
        public EventTimeData hard;
        public EventTimeData chicken;
    }
   
    [System.Serializable]
    public class EventTimeData
    {
        [Min(0)] public float[] minSpawnCount;
        [Min(0)] public float[] maxSpawnCount;
    }
    [SerializeField] private DifficultyEventTime eventTime;
    [SerializeField] private DifficultyTime difficultyData;

    [Header("Groupes de Spawns")]
    [SerializeField] private List<SpawnGroup> spawnGroups = new List<SpawnGroup>();

    [Header("Param�tres de Zone")]
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;
    [SerializeField] private Transform parentContainer;

    [Header("Automatique")]
    [SerializeField] private bool spawnOnStart = true;

    private NavMeshTriangulation navMeshTriangulation;
    private int difficultyLevel;

    private void Start()
    {
        difficultyLevel = gameObject.GetComponent<Difficulty>().GetDifficulty();
        difficultyLevel--;
        SetRandomTrash trashes = FindAnyObjectByType<SetRandomTrash>();
        trashes.SetNumberTrash(GetTrashNumber());
        trashes.SetTrashSelected();
        ScoreManager sM = FindAnyObjectByType<ScoreManager>();
        sM.SetGameDuration(GetTimeNumber());
        sM.StartTimer();
        RandomEvent rE = FindAnyObjectByType<RandomEvent>();
        rE.SetTimeEvent(GetTimeData(eventTime).minSpawnCount, GetTimeData(eventTime).maxSpawnCount);
        rE.EventTime();
        if (spawnOnStart)
        {
            SpawnAll();
        }

        
    }

    public void SpawnAll()
    {
        if (spawnGroups == null || spawnGroups.Count == 0)
        {
            Debug.LogWarning("[NavMeshSpawner] Aucun groupe de spawn configur� !");
            return;
        }

        navMeshTriangulation = NavMesh.CalculateTriangulation();

        if (navMeshTriangulation.vertices.Length == 0)
        {
            Debug.LogError("[NavMeshSpawner] Aucun NavMesh trouv� dans la sc�ne ! Avez-vous 'Bake' votre NavMesh ?");
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
            Debug.LogWarning($"[NavMeshSpawner] Aucun pr�fab assign� dans le groupe '{group.groupName}' !");
            return;
        }

        DifficultySpawnData data = GetSpawnData(group);

        int targetSpawnCount = Random.Range(
            data.minSpawnCount,
            data.maxSpawnCount + 1
        );

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

        Debug.Log($"[NavMeshSpawner] Groupe '{group.groupName}' : Cible = {targetSpawnCount} | Spawns r�ussis = {successCount}");
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
    
    private DifficultySpawnData GetSpawnData(SpawnGroup group)
    {
        switch (difficultyLevel)
        {
            case 0:
                return group.easy;

            case 1:
                return group.normal;

            case 2:
                return group.hard;
            case 3:
                return group.chicken;

            default:
                return group.normal;
        }
    }
    
    private int GetTrashNumber()
    {
        switch (difficultyLevel)
        {
            case 0: return trashData.easy;
            case 1: return trashData.normal;
            case 2: return trashData.hard;
            case 3: return trashData.chicken;
            default: return trashData.normal;
        }
    }
    
    private float GetTimeNumber()
    {
        switch (difficultyLevel)
        {
            case 0: return difficultyData.easy;
            case 1: return difficultyData.normal;
            case 2: return difficultyData.hard;
            case 3: return difficultyData.chicken;
            default: return difficultyData.normal;
        }
    }
    
    
    private EventTimeData GetTimeData(DifficultyEventTime group)
    {
        switch (difficultyLevel)
        {
            case 0:
                return group.easy;

            case 1:
                return group.normal;

            case 2:
                return group.hard;
            case 3:
                return group.chicken;
            default:
                return group.normal;
        }
    }
}