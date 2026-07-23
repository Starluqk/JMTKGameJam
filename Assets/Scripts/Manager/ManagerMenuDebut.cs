using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMenuDebut : MonoBehaviour
{
    public Animator transition;
    public void OnStart()
    {
        LoadNextScene();
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }
}
