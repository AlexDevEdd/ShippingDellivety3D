using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCorExecOrder : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FixedCoroutine());
        StartCoroutine(UpdateCoroutine());

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }

    private void Update()
    {
        Debug.Log("Update");
    }

    IEnumerator FixedCoroutine()
    {
        while(true)
        {
            Debug.Log("Pre fix");
            yield return new WaitForFixedUpdate();
            Debug.Log("After fix");
        }
    }

    IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            Debug.Log("Pre UpdateCoroutine");
            int frame = Time.frameCount;
            while(frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            Debug.Log("After UpdateCoroutine");
        }
    }
}
