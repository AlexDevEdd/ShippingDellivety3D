using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class CanvasExtension
{
    public static void SizeToParent(this Image image, float padding = 0) // Auto prio height/width (what is clamped)
    {
        image.SizeToParentHeight(padding);
        float sizeOne = image.GetComponent<RectTransform>().sizeDelta.x;
        image.SizeToParentWidth(padding);
        float sizeTwo = image.GetComponent<RectTransform>().sizeDelta.x;
        if (sizeOne < sizeTwo)
            image.SizeToParentHeight(padding);
    }

    public static void SizeToParent(this RawImage image, float padding = 0) // Auto prio height/width (what is clamped)
    {
        image.SizeToParentHeight(padding);
        float sizeOne = image.GetComponent<RectTransform>().sizeDelta.x;
        image.SizeToParentWidth(padding);
        float sizeTwo = image.GetComponent<RectTransform>().sizeDelta.x;
        if (sizeOne < sizeTwo)
            image.SizeToParentHeight(padding);
    }

    public static void SizeToParentHeight(this Image image, float padding = 0)
    {
        if (image.transform.parent == null) return;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();
        if (parent == null) return;
        padding = 1 - padding;
        float w = 0, h = 0;
        float ratio = image.mainTexture.width / (float)image.mainTexture.height;
        var bounds = new Rect(0, 0, float.PositiveInfinity, parent.rect.height);
        if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
        {
            //Invert the bounds if the image is rotated
            bounds.size = new Vector2(bounds.height, bounds.width);
        }
        //Size by height first
        h = bounds.height * padding;
        w = h * ratio;
        if (w > bounds.width * padding)
        { //If it doesn't fit, fallback to width;
            w = bounds.width * padding;
            h = w / ratio;
        }
        imageTransform.sizeDelta = new Vector2(w, h);
    }

    public static void SizeToParentWidth(this Image image, float padding = 0)
    {
        if (image.transform.parent == null) return;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();
        if (parent == null) return;
        padding = 1 - padding;
        float w = 0, h = 0;
        float ratio = image.mainTexture.width / (float)image.mainTexture.height;
        var bounds = new Rect(0, 0, parent.rect.width, float.PositiveInfinity);
        if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
        {
            //Invert the bounds if the image is rotated
            bounds.size = new Vector2(bounds.height, bounds.width);
        }
        //Size by height first
        h = bounds.height * padding;
        w = h * ratio;
        if (w > bounds.width * padding)
        { //If it doesn't fit, fallback to width;
            w = bounds.width * padding;
            h = w / ratio;
        }
        imageTransform.sizeDelta = new Vector2(w, h);
    }

    public static void SizeToParentHeight(this RawImage image, float padding = 0)
    {
        if (image.transform.parent == null) return;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();
        if (parent == null) return;
        padding = 1 - padding;
        float w = 0, h = 0;
        float ratio = image.texture.width / (float)image.texture.height;
        var bounds = new Rect(0, 0, float.PositiveInfinity, parent.rect.height);
        if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
        {
            //Invert the bounds if the image is rotated
            bounds.size = new Vector2(bounds.height, bounds.width);
        }
        //Size by height first
        h = bounds.height * padding;
        w = h * ratio;
        if (w > bounds.width * padding)
        { //If it doesn't fit, fallback to width;
            w = bounds.width * padding;
            h = w / ratio;
        }
        imageTransform.sizeDelta = new Vector2(w, h);
    }

    public static void SizeToParentWidth(this RawImage image, float padding = 0)
    {
        if (image.transform.parent == null) return;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();
        if (parent == null) return;
        padding = 1 - padding;
        float w = 0, h = 0;
        float ratio = image.mainTexture.width / (float)image.mainTexture.height;
        var bounds = new Rect(0, 0, parent.rect.width, float.PositiveInfinity);
        if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
        {
            //Invert the bounds if the image is rotated
            bounds.size = new Vector2(bounds.height, bounds.width);
        }
        //Size by height first
        h = bounds.height * padding;
        w = h * ratio;
        if (w > bounds.width * padding)
        { //If it doesn't fit, fallback to width;
            w = bounds.width * padding;
            h = w / ratio;
        }
        imageTransform.sizeDelta = new Vector2(w, h);
    }
}