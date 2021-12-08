using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
