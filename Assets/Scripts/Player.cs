using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1;
    [SerializeField]
    private float lookSensitivity = 5;
    [SerializeField]
    private float jumpHeight = 10;
    [SerializeField]
    private float gravity = 9.81f;
    private Vector2 moveVector;
    private Vector2 lookVector;
    private Vector3 rotation;
    private float verticalVelocity;
    private CharacterController characterController;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        animator.SetBool("isWalking", moveVector.magnitude > 0);
    }

    private void Move()
    {
        // Handle vertical velocity
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -0.1f; // Small value to keep the character grounded
            }

            // Jump logic
            if (Input.GetButtonDown("Jump")) // Use Input.GetButtonDown for jumping
            {
                Jump();
            }
        }
        else
        {
            verticalVelocity += -gravity * Time.deltaTime;
        }

        // Calculate movement
        Vector3 move = transform.right * moveVector.x + transform.forward * moveVector.y + transform.up * verticalVelocity;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    private void Rotate()
    {
        rotation.y += lookVector.x * lookSensitivity * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }

    private void Jump()
    {
        verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravity); // Calculate the initial vertical velocity for jumping
        animator.Play("Jump"); // Trigger the jump animation
    }
}
