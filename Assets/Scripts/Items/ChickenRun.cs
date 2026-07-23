using UnityEngine;

public class ChickenRun : MonoBehaviour
{
    private float speed = 5f;
    private float keepSpeed;
    public float distanceView = 3f;

    public GameObject player;
    void Start()
    {
        keepSpeed = speed;
    }

    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceView && distance > 1.8f)
        {
            speed = keepSpeed;

            Vector2 run = (transform.position - player.transform.position).normalized;

            transform.Translate((run *  speed)* Time.deltaTime);
        }

        if (distance < 1.8f)
        {
            speed = 0f;
        }
    }
}
