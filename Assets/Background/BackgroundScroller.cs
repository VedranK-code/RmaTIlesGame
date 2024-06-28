using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float scrollSpeed = 0.5f;

    private float offset;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
