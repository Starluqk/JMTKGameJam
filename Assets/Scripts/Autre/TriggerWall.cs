using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWall : MonoBehaviour
{
    [SerializeField] private GameObject _boxCollider;
    private BoxCollider2D _selfBoxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!GetComponent<BoxCollider2D>().IsUnityNull())
        {
            _selfBoxCollider = GetComponent<BoxCollider2D>();
        }
        _boxCollider = Instantiate(_boxCollider, transform);
        _boxCollider.GetComponent<OnTriggerWall>().SetWall(gameObject);
        BoxCollider2D triggerZone = _boxCollider.GetComponent<BoxCollider2D>();
        SpriteRenderer wallSprite = GetComponent<SpriteRenderer>();
        
        if (!triggerZone.IsUnityNull() && !wallSprite.IsUnityNull() && !_selfBoxCollider.IsUnityNull())
        {
            float spriteWidth = wallSprite.bounds.size.x / transform.lossyScale.x;
            float spriteHeight = wallSprite.bounds.size.y / transform.lossyScale.y;

            float ySize = spriteHeight - _selfBoxCollider.size.y;

            triggerZone.size = new Vector2(spriteWidth, ySize);

            float offset =
                _selfBoxCollider.offset.y + (_selfBoxCollider.size.y * 0.5f) + (ySize * 0.5f);

            triggerZone.offset = new Vector2(0f, offset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
