using UnityEngine;
using UnityEngine.AI;

public class ChickenRun : MonoBehaviour
{
    private float destinationTimer = 0f;
    [SerializeField] private float updateDestinationEvery = 0.2f;
    private float speed = 5f;
    private float keepSpeed;
    public float distanceView = 3.5f;
    Animator animator;
    private string runningBool = "isRunning";
    private Vector3 position;
    public SpriteRenderer chickenSprite;

    private ItemGrabber grabber;

    public GameObject player;
    
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent.isOnNavMesh);
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        keepSpeed = speed;
        agent.speed = speed;

        animator = GetComponent<Animator>();
        grabber = FindFirstObjectByType<ItemGrabber>();

        FindPlayerByLayer();
    }

    void Update()
    {
        destinationTimer += Time.deltaTime;
        float distance = Vector2.Distance(agent.nextPosition, player.transform.position);

        if (distance < distanceView && distance > 1.8f || grabber.chickenIsGrabbed == false && distance < 1.8f)
        {
            agent.isStopped = false;
            agent.speed = keepSpeed;

            animator.SetBool(runningBool, true);

            if (destinationTimer >= updateDestinationEvery)
            {
                destinationTimer = 0f;

                Vector3 direction = (agent.nextPosition - player.transform.position).normalized;
                Vector3 destination = agent.nextPosition + direction * 2f;

                agent.SetDestination(destination);
            }

            if (player.transform.position.x > transform.position.x)
            {
                chickenSprite.flipX = false;
            }
            else
            {
                chickenSprite.flipX = true;
            }
        }

        if (grabber.chickenIsGrabbed == true || distance > distanceView)
        {
            agent.isStopped = true;
            animator.SetBool(runningBool, false);
        }
        Debug.DrawLine(agent.nextPosition, agent.destination, Color.red);
    }
    private void FindPlayerByLayer()
    {
        int playerLayer = LayerMask.NameToLayer("Player");

#pragma warning disable 0618
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
#pragma warning restore 0618

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == playerLayer)
            {
                if (obj.transform.parent != null)
                {
                    player = obj.transform.parent.gameObject;
                }
                else
                {
                    player = obj;
                }
                break;
            }
        }

        if (player == null)
        {
            Debug.LogWarning("ChickenRun : Aucun GameObject avec le layer 'Player' n'a �t� trouv� dans la sc�ne !");
        }
    }
}