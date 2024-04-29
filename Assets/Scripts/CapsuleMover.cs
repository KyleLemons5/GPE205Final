using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMover : Mover
{
    private Rigidbody rb;
    private Transform tf;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    public override void Move(Vector3 direction, float speed)
    {
        direction.y = 0;
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    public override void Jump(float jumpForce)
    {
        if(rb.velocity.y < 0.01)
        {
            Vector3 jump = Vector3.zero;
            jump.y = jumpForce;
            rb.AddForce(jump, ForceMode.VelocityChange);
        }
    }

    public override void Rotate(float turnSpeed)
    {
        tf.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}
