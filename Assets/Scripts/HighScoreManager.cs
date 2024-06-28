using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions; 
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class HighScoreManager : MonoBehaviour
{
    private FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        var userHighscore = PlayerPrefs.GetInt("Score");
      UpdateHighScore(userHighscore);
    }

    public void UpdateHighScore(int localHighScore)
    {
    
        string username = PlayerPrefs.GetString("Username", string.Empty);
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username not set in PlayerPrefs.");
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

                    if (localHighScore > firestoreHighScore)
                    {
                        Dictionary<string, object> updatedData = new Dictionary<string, object>
                        {
                            { "highscore", localHighScore }
                        };

                        docRef.UpdateAsync(updatedData).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log("High score updated successfully.");
                            }
                            else
                            {
                                Debug.LogError("Failed to update high score.");
                            }
                        });
                    }
                }
                else
                {
                    Debug.LogError("User does not exist in Firestore.");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve user data.");
            }
        });
    }
}
