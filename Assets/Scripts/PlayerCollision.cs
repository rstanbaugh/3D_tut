using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            // debug log the name of the collided object
            Debug.Log("Collided with: " + collisionInfo.collider.name);
            movement.enabled = false;
        }

    }
}
