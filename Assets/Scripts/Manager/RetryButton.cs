using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{

    public void RetryGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}