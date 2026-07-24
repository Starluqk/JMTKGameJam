using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Extinctor : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private ItemGrabber grabber;
    [SerializeField] private GameObject _spray;
    private Vector3 direction;
    private VisualEffect _vfxSpray;
    [SerializeField] private float distanceMax = 2.5f;
    [SerializeField] private float angleMax = 15f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _vfxSpray = _spray.GetComponent<VisualEffect>();
        grabber = FindFirstObjectByType<ItemGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && grabber.extinctorIsGrabbed)
        {
            _spray.SetActive(true);
            //Activer son extincteur ici
            direction =transform.position -  _playerTransform.position;
            direction.z = 0;
            _vfxSpray.SetVector3("Spray", direction);
            Vector3 directionSpray = direction.normalized;

            Collider2D[] objets = Physics2D.OverlapCircleAll(transform.position, distanceMax);
            foreach (Collider2D obj in objets)
            {
                Vector3 versObjet = obj.transform.position - transform.position;

                if (versObjet.magnitude <= distanceMax)
                {
                    float angle = Vector3.Angle(directionSpray, versObjet);

                    if (angle <= angleMax)
                    {
                        GameObject objGameObject = obj.gameObject;
                        if (!objGameObject.GetComponent<FireScript>().IsUnityNull())
                        {
                            objGameObject.GetComponent<FireScript>().killFire();
                        }
                    }
                }
            }
            Debug.DrawRay(transform.position,
                Quaternion.Euler(0, 0, 15) * direction.normalized * distanceMax,
                Color.green);

            Debug.DrawRay(transform.position,
                Quaternion.Euler(0, 0, -15) * direction.normalized * distanceMax,
                Color.green);
        }
        else
        {
            _spray.SetActive(false);
            //Désctiver son extincteur ici
        }
    }
}
