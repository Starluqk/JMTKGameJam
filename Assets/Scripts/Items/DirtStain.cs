using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DirtStain : MonoBehaviour
{
    [Header("Réglages du Nettoyage")]
    [Tooltip("Vitesse d'effacement de la tache")]
    [SerializeField] private float cleanSpeed = 2f;

    [Header("Identification de l'outil")]
    [Tooltip("Layer du balai (ex: Broom)")]
    [SerializeField] private LayerMask broomLayer;

    private List<SpriteRenderer> allRenderers = new List<SpriteRenderer>();
    private float currentOpacity = 1f;

    private void Awake()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            if (!allRenderers.Contains(sr))
                allRenderers.Add(sr);
        }

        Transform currentParent = transform.parent;
        while (currentParent != null)
        {
            if (currentParent.TryGetComponent<SpriteRenderer>(out SpriteRenderer parentSr))
            {
                if (!allRenderers.Contains(parentSr))
                    allRenderers.Add(parentSr);
            }

            if (!currentParent.TryGetComponent<DirtStain>(out _) && !currentParent.TryGetComponent<SpriteRenderer>(out _))
            {
                break;
            }

            currentParent = currentParent.parent;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((broomLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            float mouseMovement = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));

            if (mouseMovement > 0.05f)
            {
                CleanStain(mouseMovement * cleanSpeed * Time.deltaTime);
            }
        }
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

            DestroyStainGroup();
        }
    }

    private void DestroyStainGroup()
    {
        Transform rootStain = transform;
        while (rootStain.parent != null && (rootStain.parent.GetComponent<DirtStain>() != null || rootStain.parent.GetComponent<SpriteRenderer>() != null))
        {
            if (rootStain.parent.GetComponent<DirtStain>() == null && rootStain.parent.parent == null)
                break;

            rootStain = rootStain.parent;
        }

        Destroy(rootStain.gameObject);
    }
}