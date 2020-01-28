using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody rb;
    public float speed;
    public float maxSpeed;

    public float distanceToGround;
    public float jumpForce;

    void FixedUpdate() {
        Move();
        Jump();
    }

    void Move() {
        Vector2 direction = WallCheck();
        rb.AddForce(new Vector3(direction.x * speed, 0, direction.y * speed), ForceMode.VelocityChange);

        Vector2 clampedVelocity = Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), maxSpeed);
        rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.y);
    }

    Vector2 WallCheck() {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (VerticalRaycast(Vector3.left) && direction.x == -1) direction.x = 0;
        if (VerticalRaycast(Vector3.right) && direction.x == 1) direction.x = 0;
        if (VerticalRaycast(Vector3.forward) && direction.y == 1) direction.y = 0;
        if (VerticalRaycast(Vector3.back) && direction.y == -1) direction.y = 0;

        return direction;
    }

    void Jump() {
        if (Input.GetAxisRaw("Jump") == 0) return;
        if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y/2, transform.position.z), Vector3.down, distanceToGround)) return;
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool VerticalRaycast(Vector3 direction) {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y/2, transform.position.z), direction, distanceToGround) 
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z), direction, distanceToGround)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.localScale.y/2, transform.position.z), direction, distanceToGround)) return true;
        return false;
    }
}
