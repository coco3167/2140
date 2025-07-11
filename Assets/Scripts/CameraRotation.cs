using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private InputActionReference look;

    [SerializeField, Range(0.1f, 1)] private float rotationSpeed;

    private float m_pitch, m_yaw;

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        Vector2 lookingVector = look.ToInputAction().ReadValue<Vector2>();
        
        m_pitch += rotationSpeed * -lookingVector.y;
        m_yaw += rotationSpeed * lookingVector.x;

        // Clamp pitch:
        m_pitch = Mathf.Clamp(m_pitch, -90f, 90f);

        // Wrap yaw:
        while (m_yaw < 0f)
        {
            m_yaw += 360f;
        }

        while (m_yaw >= 360f)
        {
            m_yaw -= 360f;
        }

        mainCamera.eulerAngles = new Vector3(m_pitch, m_yaw, 0);
    }
}
