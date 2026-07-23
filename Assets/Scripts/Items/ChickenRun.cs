using UnityEngine;

public class ChickenRun : MonoBehaviour
{
    private float speed = 4f;
    private float keepSpeed;
    public float distanceView = 3f;
    Animator animator;
    private string runningBool = "isRunning";

    public GameObject player;
    void Start()
    {
        keepSpeed = speed;
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceView && distance > 1.8f)
        {
            speed = keepSpeed;

            animator.SetBool(runningBool, true);

            Vector2 run = (transform.position - player.transform.position).normalized;

            transform.Translate((run *  speed)* Time.deltaTime);
        }

        if (distance < 1.5f || distance > distanceView)
        {
            speed = 0f;
        }


        if(speed < 1)
        {
            animator.SetBool(runningBool, false);
        }
    }
}
