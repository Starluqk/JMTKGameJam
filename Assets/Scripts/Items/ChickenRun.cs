using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class ChickenRun : MonoBehaviour
{
    private float destinationTimer = 0f;
    [SerializeField] private float updateDestinationEvery = 0.2f;
    private float speed = 10f;
    private float keepSpeed;
    public float distanceView = 3.5f;
    Animator animator;
    private string runningBool = "isRunning";
    private Vector3 position;
    public SpriteRenderer chickenSprite;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] listSound;

    private ItemGrabber grabber;

    public GameObject player;
    
    private NavMeshAgent agent;
    
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float walkRadius = 5f;
    [SerializeField] private float waitBeforeNextWalk = 2f;

    private float walkTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        keepSpeed = speed;
        agent.speed = speed;

        animator = GetComponent<Animator>();
        grabber = FindFirstObjectByType<ItemGrabber>();

        FindPlayerByLayer();
        keepSpeed = runSpeed;
        agent.speed = walkSpeed;
        walkTimer = waitBeforeNextWalk;
        animator.SetBool(runningBool, true);
    }

    void Update()
    {
        destinationTimer += Time.deltaTime;
        float distance = Vector2.Distance(agent.nextPosition, player.transform.position);

        if (distance < distanceView && distance > 1.8f || grabber.chickenIsGrabbed == false && distance < 1.8f)
        {
            agent.isStopped = false;
            playSound();
            agent.speed = runSpeed;

            animator.SetBool(runningBool, true);

            if (destinationTimer >= updateDestinationEvery)
            {
                destinationTimer = 0f;

                Vector3 direction = (transform.position - player.transform.position).normalized;
                Vector3 desiredPosition = transform.position + direction * 5f;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(desiredPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();

                    if (agent.CalculatePath(hit.position, path) &&
                        path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetDestination(hit.position);
                    }
                }
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

        if ( distance > distanceView)
        {
            agent.speed = walkSpeed;

            //animator.SetBool(runningBool, false);

            walkTimer -= Time.deltaTime;

            if (!agent.pathPending && agent.remainingDistance < 0.3f)
            {
                if (walkTimer <= 0)
                {
                    walkTimer = Random.Range(1f, 4f);
                    SetRandomDestination();
                }
            }
            stopplaying();
            
        }

        if (!grabber.GetGrabObject().IsUnityNull() && grabber.GetGrabObject().GetEntityId() == gameObject.GetEntityId())
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
            Debug.LogWarning("ChickenRun : Aucun GameObject avec le layer 'Player' n'a été trouvé dans la scène !");
        }
    }

    private void playSound()
    {
        if (source.isPlaying != true)
        {
            int randomNumber = Random.Range(0, 4);
            source.PlayOneShot(listSound[randomNumber]);
        }
    }

    private void stopplaying()
    {
        source.Stop();
    }
    
    private void SetRandomDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle * walkRadius;
        Vector3 wantedPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(wantedPosition, out hit, walkRadius, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();

            if (agent.CalculatePath(hit.position, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(hit.position);
            }
        }
    }
}