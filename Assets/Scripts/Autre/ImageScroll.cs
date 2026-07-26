using UnityEngine;
using UnityEngine.UI;

public class ImageScroll : MonoBehaviour
{
    public RawImage img;

    [Header("Scroll")]
    public float x = 0.1f;
    public float y = 0.0f;

    [Header("Vague")]
    public float waveAmplitude = 0.02f;
    public float waveFrequency = 2f;

    [Header("Rotation")]
    public float rotationAngle = 3f;      // Angle max en degrés
    public float rotationSpeed = 1f;      // Vitesse du balancement

    private Vector2 uvPosition;

    private void Start()
    {
        uvPosition = img.uvRect.position;
    }

    private void Update()
    {
        // Déplacement principal
        Vector2 direction = new Vector2(x, y);

        if (direction.sqrMagnitude > 0.0001f)
        {
            direction.Normalize();

            // Vecteur perpendiculaire
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);

            // Petite vague
            float wave =
                Mathf.Sin(Time.time * waveFrequency) * waveAmplitude +
                Mathf.Sin(Time.time * waveFrequency * 0.53f) * waveAmplitude * 0.5f;

            uvPosition += (direction * new Vector2(x, y).magnitude + perpendicular * wave) * Time.deltaTime;
        }
        else
        {
            uvPosition += new Vector2(x, y) * Time.deltaTime;
        }

        img.uvRect = new Rect(uvPosition, img.uvRect.size);

        // Balancement de gauche à droite
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}