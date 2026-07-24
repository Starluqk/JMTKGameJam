using Unity.VisualScripting;
using UnityEngine;

public class TriggerObject : MonoBehaviour
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
        _boxCollider.GetComponent<OnTriggerObject>().SetObj(gameObject);
        BoxCollider2D triggerZone = _boxCollider.GetComponent<BoxCollider2D>();
        SpriteRenderer objectSprite = GetComponent<SpriteRenderer>();
        Debug.Log(_selfBoxCollider.IsUnityNull());
        if (!triggerZone.IsUnityNull() && !objectSprite.IsUnityNull() && !_selfBoxCollider.IsUnityNull())
        {
            float spriteWidth = objectSprite.bounds.size.x / transform.lossyScale.x;
            float spriteHeight = objectSprite.bounds.size.y / transform.lossyScale.y;

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
