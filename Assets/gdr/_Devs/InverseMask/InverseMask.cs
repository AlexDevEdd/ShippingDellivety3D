using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enable read/write on texture
public class InverseMask : MonoBehaviour
{
    public Image targetImage;

    private Sprite _inverseSprite;
    private Texture2D _newTexture;
    private Image _selfImage;

    public void Awake()
    {
        InverseSprite();
    }

    [NaughtyAttributes.Button]
    void InverseSprite()
    {
        var targetSprite = targetImage.sprite;
        var texture = targetSprite.texture;
        _newTexture = new Texture2D(texture.width * 2, texture.height * 2, texture.format, true);
        var colors = new Color[(texture.width * 2) * (texture.height * 2)];

        for (int i = 0; i < colors.Length; i++)
            colors[i] = new Color(1f, 1f, 1f, 1f);

        _newTexture.SetPixels(0, 0, texture.width * 2, texture.height * 2, colors);

        for (int y = 0 ; y < targetSprite.texture.height; y++)
        {
            for (int x = 0 ; x < targetSprite.texture.width; x++)
            {
                var pixel = targetSprite.texture.GetPixel(x, y);
                if (pixel.a > 0f)
                    _newTexture.SetPixel(x + texture.width / 2, y + texture.height / 2, new Color(0f,0f,0f,0f));
                else
                    _newTexture.SetPixel(x + texture.width / 2, y + texture.height / 2, new Color(1f, 1f, 1f, 1f));
            }
        }
        _newTexture.Apply();
        if (GetComponent<Image>() == null)
        {
            _selfImage = gameObject.AddComponent<Image>();
        }
        if (_selfImage == null)
            _selfImage = GetComponent<Image>();
        _selfImage.sprite = Sprite.Create(_newTexture, new Rect(0, 0, _newTexture.width, _newTexture.height), Vector2.one * .5f);
        if (GetComponent<Mask>() == null)
        {
            var mask= gameObject.AddComponent<Mask>();
        }
        GetComponent<Mask>().showMaskGraphic = false;
    }
}
