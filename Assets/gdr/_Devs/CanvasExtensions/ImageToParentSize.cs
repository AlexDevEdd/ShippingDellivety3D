using UnityEngine;
using UnityEngine.UI;

public class ImageToParentSize : MonoBehaviour
{
    [Range(0, 1)]
    public float padding = 0f;

    private void Awake()
    {
        SizeToParent();
    }

    [NaughtyAttributes.Button]
    public void SizeToParent() // Auto prio height/width (what is clamped)
    {
        GetComponent<Image>().SizeToParent(padding);
    }
}
