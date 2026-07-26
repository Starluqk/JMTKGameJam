using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToiletFlood : MonoBehaviour
{
    [Header("Réglages du Nettoyage")]
    [SerializeField] private float cleanSpeed = 0.5f;

    [Header("Identification de l'outil")]
    [SerializeField] private LayerMask broomLayer;
    [SerializeField] private ItemGrabber itemGrabber;
    [SerializeField] private audioclass audioclass;

    private List<SpriteRenderer> allRenderers = new List<SpriteRenderer>();
    private float currentOpacity = 1f;

    private void Awake()
    {
        if (itemGrabber == null)
        {
            itemGrabber = FindFirstObjectByType<ItemGrabber>();
        }

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            if (!allRenderers.Contains(sr))
                allRenderers.Add(sr);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((broomLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (IsBroomGrabbed(other.gameObject))
            {
                float mouseMovement = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));

                if (mouseMovement > 0.05f)
                {
                    CleanStain(mouseMovement * cleanSpeed * Time.deltaTime);

                    if (audioclass != null)
                    {
                        audioclass.playClipOnLoop("balais");
                    }
                }
            }
        }
    }

    private bool IsBroomGrabbed(GameObject broomObject)
    {
        if (itemGrabber == null) return false;
        return itemGrabber.GrabbedGameObject == broomObject;
    }

    private void CleanStain(float amount)
    {
        currentOpacity -= amount;
        currentOpacity = Mathf.Clamp01(currentOpacity);

        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr != null)
            {
                Color color = sr.color;
                color.a = currentOpacity;
                sr.color = color;
            }
        }

        if (currentOpacity <= 0.05f)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(50);
            }

            Destroy(gameObject);
        }
    }
}