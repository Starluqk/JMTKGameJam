using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoint;

    [SerializeField] private GameObject fire;

    [SerializeField] private GameObject alarm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject feu = Instantiate(fire, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
        Instantiate(alarm,feu.transform); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
