using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float rotationSpeed = 40f;
    public float scrollSpeed = 20f;

    private bool _isRotating;
    private Vector3 _initialMousePosition;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            pos += transform.forward * (panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos -= transform.forward * (panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += transform.right * (panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos -= transform.right * (panSpeed * Time.deltaTime);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        transform.position = pos;

        // Rotation
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            _isRotating = true;
            _initialMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isRotating = false;
        }

        if (_isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _initialMousePosition;

            float rotationX = mouseDelta.x * rotationSpeed * Time.deltaTime;
            float rotationY = mouseDelta.y * rotationSpeed * Time.deltaTime;

            Transform transform1;
            (transform1 = transform).Rotate(Vector3.up, rotationX, Space.World); // Rotate around the world up vector
            transform.Rotate(transform1.right, -rotationY, Space.World); // Rotate around the camera's right vector

            _initialMousePosition = currentMousePosition;
        }
    }
}