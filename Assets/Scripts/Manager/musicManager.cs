using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : MonoBehaviour
{
    [SerializeField] private audioclass menuMusic;
    [SerializeField] private audioclass gameMusic;
    public ScoreManager scoreManager;
    void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex + " < " + SceneManager.GetSceneByName("House1").buildIndex);
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            menuMusic.playClipOnLoop("MainMenuMusic");
        }
        else
        {
            if(scoreManager.isPlaying == true)
            { 
            gameMusic.playClipOnLoop("gameMusic");
            }
        }
    }
}
