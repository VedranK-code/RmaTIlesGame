using UnityEngine;
using UnityEngine.UI;

public class SpriteCycle : MonoBehaviour
{
    public Button targetButton;
    public Sprite[] sprites;
    private int currentIndex = 0;

    private void Start()
    {
        targetButton.image.sprite = sprites[currentIndex];
    }

    public void OnButtonClick()
    {
        currentIndex++;
        if (currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }
        targetButton.image.sprite = sprites[currentIndex];
    }
}