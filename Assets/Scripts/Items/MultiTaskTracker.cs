using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiTaskTracker : MonoBehaviour
{
    [System.Serializable]
    public class LayerObjective
    {
        [Tooltip("Nom affiché dans l'UI (ex: Trash, Poules, Extincteurs)")]
        public string displayName = "Trash";

        [Tooltip("Layer de l'objet à détecter sur la scène")]
        public LayerMask targetLayer;

        [HideInInspector] public int totalCount;
    }

    [Header("Liste des Objectifs")]
    [SerializeField] private List<LayerObjective> objectives = new List<LayerObjective>();

    [Header("UI")]
    [Tooltip("Glisse ton composant TextMeshPro - Text (UI) ici")]
    [SerializeField] private TextMeshProUGUI trackerText;

    [Header("Paramètres")]
    [Tooltip("Intervalle de vérification en secondes")]
    [SerializeField] private float updateInterval = 0.25f;

    private void Start()
    {
        foreach (var obj in objectives)
        {
            obj.totalCount = CountObjectsInLayer(obj.targetLayer);
        }

        UpdateTrackerUI();

        InvokeRepeating(nameof(UpdateTrackerUI), updateInterval, updateInterval);
    }

    public void UpdateTrackerUI()
    {
        if (trackerText == null || objectives.Count == 0) return;

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        for (int i = 0; i < objectives.Count; i++)
        {
            var obj = objectives[i];

            if (obj.totalCount == 0) continue;

            int remaining = CountObjectsInLayer(obj.targetLayer);
            int cleared = obj.totalCount - remaining;

            if (cleared < 0)
            {
                obj.totalCount = remaining;
                cleared = 0;
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.Append($"{obj.displayName} - {cleared}/{obj.totalCount}");
        }

        trackerText.text = stringBuilder.ToString();
    }

    private int CountObjectsInLayer(LayerMask layerMask)
    {
        int count = 0;
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if ((layerMask.value & (1 << obj.layer)) != 0)
            {
                count++;
            }
        }

        return count;
    }
}