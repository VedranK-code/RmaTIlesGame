using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool isFirstTime;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void HighScore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Login");
    }
}

