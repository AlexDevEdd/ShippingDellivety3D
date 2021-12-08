using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskPaint : MonoBehaviour
{
    public SpriteMask spriteMask;
    public RenderTexture renderTexture;

    private void Update()
    {
        spriteMask.sprite = ToSprite(ToTexture2D(renderTexture));
    }

    Sprite ToSprite(Texture2D texture2D)
    {
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
