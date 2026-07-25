using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : MonoBehaviour
{
    [SerializeField] private audioclass menuMusic;
    [SerializeField] private audioclass gameMusic;
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            menuMusic.playClipOnLoop("MainMenuMusic");
        }
        else
        {
            gameMusic.playClipOnLoop("gameMusic");
        }
    }
}
