using UnityEngine;

public class ChickenRun : MonoBehaviour
{
    private float speed = 4f;
    private float keepSpeed;
    public float distanceView = 3f;
    Animator animator;
    private string runningBool = "isRunning";

    private ItemGrabber grabber;

    public GameObject player;
    void Start()
    {
        keepSpeed = speed;
        animator = GetComponent<Animator>();
        grabber = FindAnyObjectByType<ItemGrabber>();
    }

    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceView && distance > 1.8f && grabber.chickenIsGrabbed == false)
        {
            speed = keepSpeed;

            animator.SetBool(runningBool, true);

            Vector2 run = (transform.position - player.transform.position).normalized;

            transform.Translate((run *  speed)* Time.deltaTime);
        }

        if (grabber.chickenIsGrabbed == true|| distance > distanceView)
        {
            speed = 0f;
        }


        if(speed < 1)
        {
            animator.SetBool(runningBool, false);
        }
    }
}
