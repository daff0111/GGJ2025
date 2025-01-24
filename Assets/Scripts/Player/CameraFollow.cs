using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        
    public Vector2 offset;          
    public float smoothSpeed = 5f;  
    public Vector2 minBounds;       
    public Vector2 maxBounds;       

    private Vector3 targetPosition; 

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the target position
            targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            // Clamp the position within bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}