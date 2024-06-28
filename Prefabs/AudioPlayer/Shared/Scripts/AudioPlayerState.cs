using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerState : MonoBehaviour
{

    public static AudioPlayerState instance;
    
    public List<Song> songs = new List<Song>();

    public int index = 0;
    public float currentProgress = 0;

    private void Awake()
    {

        if (instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}