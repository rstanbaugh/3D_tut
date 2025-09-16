using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating; // new input system

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float accel = 50f;     // m/s^2
    public float maxSpeed = 200;   // m/s
    public float jumpImpulse = 10f;     // one-shot upward velocity change
    public float groundedCheckDist = 0.1f;

    bool jumpRequested = false; // input request, consumed in FixedUpdate
    Vector3 move; // x/z each frame


    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f, z = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  z -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) z += 1f;

        // normalize so diagonals aren't faster
        Vector3 dir = new Vector3(x, 0f, z);
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        //
        move = dir;

        // jump request
        if (kb.spaceKey.wasPressedThisFrame)
            jumpRequested = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 1) accelerate in desired horizontal direction (mass- & framerate-independent)
        Vector3 desiredDir = move.sqrMagnitude > 0f ? move : Vector3.zero; // move is unit or zero
        rb.AddForce(desiredDir * accel, ForceMode.Acceleration);


        // OPTIONAL auto-brake when no input (comment out if you prefer glide)
        // if (desiredDir == Vector3.zero)
        // {
        //     Vector3 v = rb.velocity;
        //     Vector3 horiz = new Vector3(v.x, 0f, v.z);
        //     rb.AddForce(-horiz * accel, ForceMode.Acceleration); // symmetric brake
        // }


        // 2) clamp horizontal speed (don’t touch Y)
        // get horizontal velocity
        Vector3 vel = rb.linearVelocity;
        Vector3 horiz = new Vector3(vel.x, 0f, vel.z);      // horizontal part of the velocity

        // only clamp if over maxSpeed
        if (horiz.sqrMagnitude > maxSpeed * maxSpeed)
            horiz = horiz.normalized * maxSpeed;
        rb.linearVelocity = horiz + Vector3.up * vel.y;     // reassemble full velocity vector

        // 3) jump via impulse
        if (jumpRequested && IsGrounded())
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        jumpRequested = false;        // reset jump request     
    }

    bool IsGrounded()
    {
        // Simple raycast ground check from the collider bottom
        var col = GetComponent<Collider>();
        Vector3 origin = col.bounds.center;
        float dist = (col.bounds.extents.y + groundedCheckDist);
        return Physics.Raycast(origin, Vector3.down, dist);
    }

}
