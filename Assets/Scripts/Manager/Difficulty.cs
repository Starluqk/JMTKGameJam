using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public static int difficulty = 2;
    [SerializeField] private int buttonDifficulty = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty()
    {
        Debug.Log(buttonDifficulty);
        difficulty = buttonDifficulty;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
