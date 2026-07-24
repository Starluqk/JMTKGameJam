using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireScript : MonoBehaviour
{
    public static int _numberFire = 0;
    [SerializeField] private int _life = 100;
    [SerializeField] private float _minTimeToExtend = 1.5f;
    [SerializeField] private float _maxTimeToExtend = 5;
    [SerializeField] private GameObject _fire;
    private float _timeToExtend;
    static public NavMeshPlus.Components.NavMeshSurface navMeshSurface;
    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomTime();
        navMeshSurface = FindAnyObjectByType<NavMeshPlus.Components.NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > _timeToExtend)
        {
            if (_numberFire < 25)
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                Vector3 target = transform.position + (Vector3)direction;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(target, out hit, 1f, NavMesh.AllAreas))
                {
                    // Cherche le point navigable le plus proche du feu actuel
                    NavMeshHit startHit;
                    if (!NavMesh.SamplePosition(transform.position, out startHit, 2f, NavMesh.AllAreas))
                    {
                        return;
                    }

                    NavMeshPath path = new NavMeshPath();

                    bool pathFound = NavMesh.CalculatePath(
                        startHit.position,
                        hit.position,
                        NavMesh.AllAreas,
                        path
                    );

                   

                    if (pathFound && path.status == NavMeshPathStatus.PathComplete)
                    {
                        Instantiate(_fire, hit.position, Quaternion.identity);
                        navMeshSurface.BuildNavMesh();
                        _numberFire++;
                    }
                }
            }
            
            time = 0f;
            SetRandomTime();
        }

        if (_life <= 0)
        {
            ScoreManager.Instance.AddScore(25);
            Destroy(gameObject);
            _numberFire--;
        }
    }

    private void SetRandomTime()
    {
        _timeToExtend = Random.Range(_minTimeToExtend, _maxTimeToExtend);
        
    }

    public void killFire()
    {
        _life--;
    }
}
