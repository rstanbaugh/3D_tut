using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Offset from the player

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
