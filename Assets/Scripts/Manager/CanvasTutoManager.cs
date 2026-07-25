using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Liste de tous les Panels à gérer")]
    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    [Header("Index du Panel de départ")]
    [SerializeField] private int startingPanelIndex = 0;

    private int currentIndex = 0;

    private void Start()
    {
        ShowPanel(startingPanelIndex);
    }


    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panelList.Count)
        {
            Debug.LogWarning($"[PanelManager] L'index {index} est hors limites !");
            return;
        }

        currentIndex = index;

        for (int i = 0; i < panelList.Count; i++)
        {
            if (panelList[i] != null)
            {
                panelList[i].SetActive(i == currentIndex);
            }
        }
    }

    public void NextPanel()
    {
        int nextIndex = (currentIndex + 1) % panelList.Count;
        ShowPanel(nextIndex);
    }

    public void PreviousPanel()
    {
        int prevIndex = (currentIndex - 1 + panelList.Count) % panelList.Count;
        ShowPanel(prevIndex);
    }
}