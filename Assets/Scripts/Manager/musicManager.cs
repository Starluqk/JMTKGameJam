using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : MonoBehaviour
{
    [SerializeField] private audioclass menuMusic;
    [SerializeField] private audioclass gameMusic;
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.GetSceneByName("House1").buildIndex)
        {
            menuMusic.playClipOnLoop("MainMenuMusic");
        }
        else
        {
            gameMusic.playClipOnLoop("gameMusic");
        }
    }
}
