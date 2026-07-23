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
        }
        else
        {
            audioclass.playClipOnLoop("bip");
        }
    }
}
