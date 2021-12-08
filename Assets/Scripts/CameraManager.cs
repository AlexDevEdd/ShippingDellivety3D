using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float _smoothFactor;
    [SerializeField] Vector3 _targetTransform;  
    [SerializeField] Button _buyButton;
    [SerializeField] private Camera _cam;
    [SerializeField] private ManagerWindow managerWindow;

    private Vector3 _cameraStartPosition;

    private void Awake()
    {
        _cameraStartPosition = _cam.transform.position;      
    }

    private void Update()
    {
        if (managerWindow.IsClickBuyButton)
            StartCoroutine(MoveTowards(transform, _targetTransform, _smoothFactor));
    }
  
    IEnumerator MoveTowards(Transform objectToMove, Vector3 toPosition, float duration)
    {
        float counter = 0;
        float temp = 4.5f;
        while (counter < temp)
        {
            counter += Time.deltaTime;
            Vector3 currentPos = objectToMove.position;

            float time = Vector3.Distance(currentPos, toPosition) / (duration - counter) * Time.deltaTime;

            objectToMove.position = Vector3.MoveTowards(currentPos, toPosition, time);

            //Debug.Log(counter + " / " + temp);
            yield return null;
        }
        managerWindow.IsClickBuyButton = false;
        StopCoroutine(MoveTowards( objectToMove,  toPosition,  duration));
    }
}
