using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoginScreen()
    {
        SceneManager.LoadScene("Login"); 
    }
    public void RegisterScreen()
    {
        SceneManager.LoadScene("Register"); 
    }
}
