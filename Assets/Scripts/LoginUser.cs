using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class LoginUser : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public Button loginButton;
    public TMP_Text feedbackText;

    private FirebaseFirestore firestore;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            firestore = FirebaseFirestore.DefaultInstance;
        });

        loginButton.onClick.AddListener(() => Login());
    }

    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill in both fields.";
            return;
        }

        firestore.Collection("users")
                 .WhereEqualTo("username", username)
                 .WhereEqualTo("password", password) 
                 .GetSnapshotAsync()
                 .ContinueWithOnMainThread(task => {
                     if (task.IsCanceled || task.IsFaulted)
                     {
                         feedbackText.text = "Login failed.";
                         Debug.LogError(task.Exception);
                         return;
                     }

                     QuerySnapshot snapshot = task.Result;
                     if (snapshot.Count > 0)
                     {
                         feedbackText.text = "Login successful!";
                         PlayerPrefs.SetString("Username", username); 
                         Debug.Log("PlayerPrefs Username set to: " + username); 
                         SceneManager.LoadScene("MainMenu");
                     }
                     else
                     {
                         feedbackText.text = "Invalid username or password.";
                     }
                 });
    }
}
