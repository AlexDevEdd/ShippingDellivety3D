using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _leftLimit;
    [SerializeField] private float _rightLimit;
    [SerializeField] private float _bottomLimit;
    [SerializeField] private float _upperLimit;
    [SerializeField] private float _zoomMin = 1;
    [SerializeField] private float _zoomMax = 5;

    private Vector3 _touch;
  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {          
            _touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {         
            Vector3 direction = _touch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;     
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _leftLimit, _rightLimit),
           14f,
            Mathf.Clamp(transform.position.z, _bottomLimit, _upperLimit));
        }
       if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroLastPoint = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOneLastPoint = touchOne.position - touchOne.deltaPosition;

            float distTouch = (touchZeroLastPoint - touchOneLastPoint).magnitude;
            float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

            float difference = currentDistTouch - distTouch;
            Zoom(difference * 0.01f);          
        }  
    }
    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomMin, _zoomMax);
    }
    
}
