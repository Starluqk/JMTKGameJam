using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoint;

    [SerializeField] private GameObject chicken;

    private Transform point;

    private Vector2 offset;

    private Vector3 spawnPos;

    [SerializeField] private int numberOfSpawn = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            point = spawnPoint[Random.Range(0, spawnPoint.Length)].transform;

            offset = Random.insideUnitCircle * 2;
            spawnPos = point.position + new Vector3(offset.x, offset.y, 0);
            
            Instantiate(chicken, spawnPos, Quaternion.identity);        
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Gizmos.DrawSphere(spawnPos,0.5f);
    }
}
