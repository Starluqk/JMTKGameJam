using UnityEngine;

public class ChickenRun : MonoBehaviour
{
    private float speed = 5f;
    private float keepSpeed;
    public float distanceView = 3.5f;
    Animator animator;
    private string runningBool = "isRunning";
    private Vector3 position;
    public SpriteRenderer chickenSprite;

    private ItemGrabber grabber;

    public GameObject player;

    void Start()
    {
        keepSpeed = speed;
        animator = GetComponent<Animator>();

        grabber = FindFirstObjectByType<ItemGrabber>();
            FindPlayerByLayer();
    }

    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceView && distance > 1.8f || grabber.chickenIsGrabbed == false && distance < 1.8f)
        {
            speed = keepSpeed;

            animator.SetBool(runningBool, true);

            Vector2 run = (transform.position - player.transform.position).normalized;

            transform.Translate((run * speed) * Time.deltaTime);

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
            speed = 0f;
            animator.SetBool(runningBool, false);
        }
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
}