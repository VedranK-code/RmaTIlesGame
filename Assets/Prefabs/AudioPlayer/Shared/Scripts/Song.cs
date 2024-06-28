using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Song
{

    public string poster;
    public string value;
    public string songId;
    public string playlist;

    public Sprite image;
    public AudioClip audio;
    public string title;
    public string category;
    public string artist;
    public bool fetched = false;
    public bool loading = false;

    public bool sentPlayEvent = false;

    public IEnumerator Load()
    {
        if (fetched)
        {
            yield break;
        }

        loading = true;

        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(value, AudioType.MPEG))
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
                    audio = DownloadHandlerAudioClip.GetContent(request);
                    break;
            }
        }
        
        if (poster != "")
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(poster))
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
                        var texture = DownloadHandlerTexture.GetContent(request);
                        image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
                        break;
                }
            }
        }

        loading = false;
        fetched = true;
    }
}