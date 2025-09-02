using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class MaterialScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.5f, 0f);

    private Renderer rend;
    private Material mat;
    private Vector2 currentOffset;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    private void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = currentOffset;
    }
}
