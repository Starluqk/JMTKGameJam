using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{
    public void Quit()
    {
       Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuDebut-Erwan");
    }
    
}
