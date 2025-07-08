using UnityEngine;

public class FreeDragCamera : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100.0f; // ‰ñ“]‘¬“x

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    private float pitch = 0f; // ã‰º‰ñ“]iX²j
    private float yaw = 0f;   // ¶‰E‰ñ“]iY²j

    void Start()
    {
        // ‰Šú‰ñ“]Šp‚ğæ“¾
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotX = -delta.y * rotationSpeed * Time.deltaTime;
            float rotY = delta.x * rotationSpeed * Time.deltaTime;

            pitch += rotX;
            yaw += rotY;

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            // ‰ñ“]‚ğ“K—p
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

}
