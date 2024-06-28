using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public GameObject fill;

    void Update()
    {
        timeUpdate(AudioPlayer.instance.currentProgress);
    }

    public void timeUpdate(float perc)
    {
        fill.transform.localScale = new Vector3(perc, 1f, 1f);
    }
}
