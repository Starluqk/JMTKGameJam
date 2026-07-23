using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] private int _life = 100;
    [SerializeField] private float _minTimeToExtend = 1.5f;
    [SerializeField] private float _maxTimeToExtend = 5;
    [SerializeField] private GameObject _fire;
    private float _timeToExtend;

    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomTime();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > _timeToExtend)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector2 newPosition = (Vector2)transform.position + direction;
            Instantiate(_fire, newPosition, Quaternion.identity);
            time = 0f;
            SetRandomTime();
        }

        if (_life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetRandomTime()
    {
        _timeToExtend = Random.Range(_minTimeToExtend, _maxTimeToExtend);
    }
}
