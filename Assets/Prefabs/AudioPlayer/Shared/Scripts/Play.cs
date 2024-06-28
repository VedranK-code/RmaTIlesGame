using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{

    public void Toggle()
    {
        AudioPlayer.instance.TogglePlay();
    }
}
