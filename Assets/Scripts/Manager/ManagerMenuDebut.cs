using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMenuDebut : MonoBehaviour
{
    public void OnStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
