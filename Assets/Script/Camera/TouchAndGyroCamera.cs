using UnityEngine;

public class TouchAndGyroCamera : MonoBehaviour
{
    public float swipeSensitivity = 0.2f;
    public float gyroSensitivity = 1.0f;
    public bool useGyro = true;

    private float pitch = 0f; // 上下（X軸）
    private float yaw = 0f;   // 左右（Y軸）

    private Vector2 lastTouchPos;
    private bool isDragging = false;

    private bool gyroSupported;
    private Quaternion initialGyroRotation;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;

        gyroSupported = SystemInfo.supportsGyroscope;

        if (useGyro && gyroSupported)
        {
            Input.gyro.enabled = true;
            initialGyroRotation = Input.gyro.attitude;
        }
        else
        {
            useGyro = false;
        }
    }

    void Update()
    {
        HandleTouch();

        if (useGyro && gyroSupported)
        {
            ApplyGyroRotation();
        }

        // カメラの回転反映（上下±90度制限）
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPos;
                lastTouchPos = touch.position;

                float rotX = -delta.y * swipeSensitivity;
                float rotY = delta.x * swipeSensitivity;

                pitch += rotX;
                yaw += rotY;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    void ApplyGyroRotation()
    {
        Quaternion deviceRotation = Input.gyro.attitude;
        Quaternion gyro = new Quaternion(-deviceRotation.x, -deviceRotation.y, deviceRotation.z, deviceRotation.w);
        Quaternion delta = gyro * Quaternion.Inverse(initialGyroRotation);
        Vector3 euler = delta.eulerAngles;

        // ジャイロ角度の補正
        float gyroPitch = NormalizeAngle(euler.x) * gyroSensitivity;
        float gyroYaw = NormalizeAngle(euler.y) * gyroSensitivity;

        pitch += gyroPitch * Time.deltaTime;
        yaw += gyroYaw * Time.deltaTime;
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
