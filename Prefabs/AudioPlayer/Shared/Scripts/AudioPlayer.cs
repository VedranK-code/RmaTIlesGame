using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class AudioPlayer : MonoBehaviour
{
    public Canvas canvas;

    public Sprite play;
    public Sprite pause;
    public TextMeshProUGUI heading;
    public TextMeshProUGUI category;
    public Image poster;
    public AudioSource audioSource;
    public float currentProgress;
    public string apiKey;

    public static AudioPlayer instance;
    private int index = 0;

    private bool playing = true;
    private bool songReady = false;
    private string url = "https://f6sidjh1yi.execute-api.eu-central-1.amazonaws.com/dev/";
    private List<Song> songs = new List<Song>();

    [System.Serializable]
    private class PlaylistSong
    {
        public string songId;
        public string playlist;
        public string poster;
        public string value;
        public string title;
        public string category;
        public string author;
    }

    [System.Serializable]
    private class PlaylistSongs
    {
        public List<PlaylistSong> songs;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

        if (AudioPlayerState.instance.songs.Count == 0)
        {
            canvas.gameObject.SetActive(false);
            StartCoroutine(GetPlaylist());
            return;
        }

        songs = AudioPlayerState.instance.songs;
        index = AudioPlayerState.instance.index;
        currentProgress = AudioPlayerState.instance.currentProgress;

        SetSong();

        if (currentProgress != 0)
        {
            audioSource.time = currentProgress;
        }
    }

    private void Update()
    {

        if (!playing || songs.Count == 0)
        {
            return;
        }

        var song = songs[index];

        if (!song.fetched && !song.loading)
        {
            StartCoroutine(song.Load());
        }

        if (!songReady && song.fetched)
        {
            audioSource.clip = song.audio;
            audioSource.time = 0;
            heading.text = $"{song.artist} - {song.title}";

            song.sentPlayEvent = false;

            if (poster != null)
            {
                poster.sprite = song.image;
            }

            if (category != null)
            {
                category.text = song.category;
            }

            songReady = true;

            if (canvas.gameObject.activeSelf)
            {
                audioSource.Play();
            }
            else
            {
                canvas.gameObject.SetActive(true);
            }
        }

        if (!songReady)
        {
            return;
        }

        var duration = song.audio.length;

        currentProgress = audioSource.time / duration;
        AudioPlayerState.instance.currentProgress = currentProgress;

        if (currentProgress >= .3 && !song.sentPlayEvent)
        {
            StartCoroutine(LogEvent(song, "played"));
            song.sentPlayEvent = true;
        }

        if (currentProgress >= .7)
        {
            var nextSong = songs[GetNextIndex()];

            if (currentProgress >= .7 && !nextSong.loading && !nextSong.fetched)
            {
                StartCoroutine(nextSong.Load());
            }
        }

        if (audioSource.time >= duration)
        {
            index = GetNextIndex();
            SetSong();
        }
    }

    public void TogglePlay()
    {

        var sourceImage = GameObject.Find("TogglePlay").GetComponent<Image>();

        if (playing)
        {
            sourceImage.sprite = play;
            audioSource.Pause();
        }
        else
        {
            sourceImage.sprite = pause;
            audioSource.Play();
        }

        playing = !playing;
    }

    private int GetNextIndex()
    {
        if (index < songs.Count - 1)
        {
            return index + 1;
        }

        return 0;
    }

    public void Forward()
    {
        index = GetNextIndex();
        SetSong();
    }

    public void Back()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = songs.Count - 1;
        }

        SetSong();
    }

    public void SetSong()
    {
        AudioPlayerState.instance.index = index;
        songReady = false;
    }

    IEnumerator GetPlaylist()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url + "playlist?k=" + apiKey))
        {
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + request.error);
                    break;
                case UnityWebRequest.Result.Success:
                    var text = request.downloadHandler.text;
                    var playlist = JsonUtility.FromJson<PlaylistSongs>(text);

                    foreach (PlaylistSong song in playlist.songs)
                    {
                        var s = new Song();

                        s.poster = song.poster;
                        s.songId = song.songId;
                        s.playlist = song.playlist;
                        s.value = song.value;
                        s.artist = song.author;
                        s.title = song.title;
                        s.category = song.category;

                        songs.Add(s);
                    }

                    SetSong();
                    break;
            }
        }
    }

    IEnumerator LogEvent(Song song, string eventType) 
    {
        var path = "playlist/metrics/" + eventType + "?k=" + apiKey + "&s=" + song.songId + "&p=" + song.playlist;

        using (UnityWebRequest request = UnityWebRequest.Get(url + path))
        {
            yield return request.SendWebRequest();
        }
    }
}