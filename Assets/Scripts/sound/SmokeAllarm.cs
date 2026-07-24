using UnityEngine;
using Unity.VisualScripting;

public class SmokeAllarm : MonoBehaviour
{
    public audioclass audioclass;

    [SerializeField ]private GameObject flame;
    void Update()
    {
        if (FindAnyObjectByType<FireScript>().IsUnityNull())
        {
            audioclass.source.Stop();
        }
        else
        {
            audioclass.playClipOnLoop("bip");
        }
    }
}
