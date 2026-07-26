using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : MonoBehaviour
{
    [SerializeField] private audioclass menuMusic;
    [SerializeField] private audioclass gameMusic;
    void Update()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex + " < " + SceneManager.GetSceneByName("House1").buildIndex);
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            menuMusic.playClipOnLoop("MainMenuMusic");
        }
        else
        {
            gameMusic.playClipOnLoop("gameMusic");
        }
    }
}
