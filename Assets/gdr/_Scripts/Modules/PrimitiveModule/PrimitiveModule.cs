using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrimitiveModule
{
    public static GameObject Create(PrimitiveType type, Vector3 position, float size)
    {
        GameObject primitive = GameObject.CreatePrimitive(type);
        primitive.transform.position = position;
        primitive.transform.localScale = Vector3.one * size;
        return primitive;
    }
}
