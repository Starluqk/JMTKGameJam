using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMenuDebut : MonoBehaviour
{
    public Animator transition;
    public Animator textFade;
    private string Returned = "Return";
    public void OnStart()
    {
        LoadNextScene();

    }
    private void Awake()
    {
        Time.timeScale = 1.0f;
        transition.SetBool(Returned, true);
        textFade.SetBool(Returned, true);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 3));
    }

    public void tuto()
    {
        SceneManager.LoadScene("tuto");
    }
    public void credit()
    {
        SceneManager.LoadScene("Credit");
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        textFade.SetTrigger("Start");
        transition.SetBool(Returned, false);
        textFade.SetBool(Returned, false);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadChooseDifficulty()
    {
        SceneManager.LoadScene("ChooseDifficulty");
    }
}
