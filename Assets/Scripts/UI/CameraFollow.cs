using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The object to follow (player)
    public float smoothSpeed = 0.125f; // How smoothly the camera follows
    public Vector3 offset; // Camera offset from the target

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera's position (keep original z-axis value)
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
