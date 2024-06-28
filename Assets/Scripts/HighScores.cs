using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;

public class HighScores : MonoBehaviour
{
    public GameObject scorePrefab;
    public Transform scoresWrapper;

    private FirebaseFirestore db;
    private List<GameObject> scoreUiItems = new List<GameObject>();

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        
        var userHighscore = PlayerPrefs.GetInt("Score");
        var userName = PlayerPrefs.GetString("Username");
        var invoked = false;

        db.Collection("users")
            .OrderByDescending("highscore")
            .Limit(10)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                QuerySnapshot snapshot = task.Result;

                var index = 1;
                Debug.Log(userHighscore);

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    Dictionary<string, object> user = documentSnapshot.ToDictionary();

                    Debug.Log(int.Parse(user["highscore"].ToString()));

                    if (userHighscore > int.Parse(user["highscore"].ToString()) && index < 10 && !invoked)
                    {
                        Debug.Log("Called");
                        invoked = true;

                        RenderUser(new Dictionary<string, object>
                        {
                            { "username", userName },
                            { "highscore", userHighscore }
                        }, index);

                        index++;
                    }

                    if (index <= 10)
                    {
                        RenderUser(user, index);
                    }

                    index++;
                }

                if (index <= 10 && !invoked)
                {
                    Debug.Log("Called In Last");
                    invoked = true;

                    RenderUser(new Dictionary<string, object>
                    {
                        { "username", userName },
                        { "highscore", userHighscore }
                    }, index);
                }
            });
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }

    private void RenderUser(Dictionary<string, object> user, int index)
    {
        var inst = Instantiate(scorePrefab, Vector3.zero, Quaternion.identity);
        inst.transform.SetParent(scoresWrapper, false);
        var texts = inst.GetComponentsInChildren<TextMeshProUGUI>();

        texts[0].text = index.ToString();
        texts[1].text = user["username"].ToString();
        texts[2].text = user["highscore"].ToString();
    }
}
