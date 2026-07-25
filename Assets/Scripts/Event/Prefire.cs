using Unity.VisualScripting;
using UnityEngine;

public class Prefire : MonoBehaviour
{
    [SerializeField] private GameObject fire;

    private float timer;

    private bool isCreated;

    private GameObject obj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCreated && timer < 0)
        {
            obj = Instantiate(fire, transform);
            isCreated = true;
        }

        timer -= Time.deltaTime;
        if (isCreated)
        {
            if (obj.IsUnityNull())
            {
                Destroy(gameObject);
            }
        }
    }
}
