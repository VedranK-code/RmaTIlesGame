using UnityEngine;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        DisplayHighScore();
    }

    private void DisplayHighScore()
    {
        string username = PlayerPrefs.GetString("Username", "Unknown");
        int localHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (username == "Unknown")
        {
            scoreText.text = "No username found";
            return;
        }
        DocumentReference docRef = db.Collection("users").Document(username);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> user = snapshot.ToDictionary();
                    int firestoreHighScore = user.ContainsKey("highscore") ? Convert.ToInt32(user["highscore"]) : 0;

                    int displayHighScore = Mathf.Max(localHighScore, firestoreHighScore);
                    string scoreDisplay = $"{username}: {displayHighScore}";

                    scoreText.text = scoreDisplay;
                }
                else
                {
                    scoreText.text = $"No Firestore record found for {username}";
                }
            }
            else
            {
                scoreText.text = "Failed to retrieve data from Firestore.";
                Debug.LogError("Failed to retrieve user data.");
            }
        });
    }
}