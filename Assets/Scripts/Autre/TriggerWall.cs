using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWall : MonoBehaviour
{
    [SerializeField] private GameObject _boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boxCollider = Instantiate(_boxCollider, transform);
        _boxCollider.GetComponent<OnTriggerWall>().SetWall(gameObject);
        BoxCollider2D triggerZone = _boxCollider.GetComponent<BoxCollider2D>();
        SpriteRenderer wallSprite = GetComponent<SpriteRenderer>();
        
        if (!triggerZone.IsUnityNull() && !wallSprite.IsUnityNull())
        {
            
            float size = wallSprite.bounds.size.x / transform.lossyScale.x;
            Debug.Log(gameObject + " " + size);
            Vector2 vectorSize = new Vector2(size, 0.35f);
            triggerZone.size = vectorSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
