using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public float moveSpeed;
    public float runSpeed;

    public float groundDrag;

    public float jumpForce;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    public bool jumped;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        if (grounded)
        {
            rb.drag = groundDrag;
        }

        else
        {
            rb.drag = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(moveDir.normalized * runSpeed * 7f, ForceMode.Force);
            }

            else
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 7f, ForceMode.Force);
            }
        }

        else if (jumped)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 4f, ForceMode.Force);
        }
    }

    void Jump()
    {
        jumped = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}