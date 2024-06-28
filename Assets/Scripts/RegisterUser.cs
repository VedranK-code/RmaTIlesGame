using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using TMPro;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class RegisterUser : MonoBehaviour
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField usernameField;
    public Button registerButton;
    public TMP_Text feedbackText;

    private FirebaseFirestore firestore;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            firestore = FirebaseFirestore.DefaultInstance;
        });

        registerButton.onClick.AddListener(() => Register());
    }

    public void Register()
    {
        string email = emailField.text;
        string password = passwordField.text;
        string username = usernameField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
        {
            feedbackText.text = "Please fill in all fields.";
            return;
        }

        firestore.Collection("users").Document(username).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    feedbackText.text = "Username already exists.";
                }
                else
                {
                    Dictionary<string, object> user = new Dictionary<string, object>
                    {
                        { "username", username },
                        { "email", email },
                        { "password", password }
                    };

                    firestore.Collection("users").Document(username).SetAsync(user).ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCanceled || task.IsFaulted)
                        {
                            feedbackText.text = "Failed to save user data.";
                            Debug.LogError(task.Exception);
                            return;
                        }
                        feedbackText.text = "Registration successful!";
                        PlayerPrefs.SetString("Username", username);
                        SceneManager.LoadScene("MainMenu");
                    });
                }
            }
            else
            {
                feedbackText.text = "Error checking username.";
                Debug.LogError(task.Exception);
            }
        });
    }
}
