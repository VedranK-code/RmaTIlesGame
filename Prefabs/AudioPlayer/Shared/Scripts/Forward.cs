using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forward : MonoBehaviour
{

    public void Do()
    {
        AudioPlayer.instance.Forward();
    }
}
