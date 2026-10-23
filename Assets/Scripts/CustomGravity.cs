using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGravity : MonoBehaviour
{
    InputAction jumpAction;
    private CharacterController characterController;
    PlayerControls playerControls;

    public float jumpSpeed = 15;
    public bool canJump = true;
    public float gravity = -9f;
    public float terminalVelocity = -10;
    public float minFallSpeed = -1.5f;

    private float verticalSpeed;
    private Animator animator;

    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        characterController = GetComponent<CharacterController>();
        playerControls = GetComponent<PlayerControls>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        bool hitGround = false;
        RaycastHit groundRaycastHit;
        if (verticalSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit))
        {
            hitGround = groundRaycastHit.distance <= .12f;
        }

        if (hitGround)
        {
            if (animator != null) animator.SetBool("Jumping", false);

            if (jumpAction.triggered && canJump)
            {
                if (animator != null) animator.SetBool("Jumping", true);
                verticalSpeed = jumpSpeed;
            }
            else
            {
                verticalSpeed = minFallSpeed;
            }
        }
        else
        {
            verticalSpeed += gravity * 5 * Time.deltaTime;
            // terminalVelocity is negative
            if (verticalSpeed < terminalVelocity)
            {
                verticalSpeed = terminalVelocity;
            }

            movement.y = verticalSpeed * Time.deltaTime;
            characterController.Move(movement);
        }
    }
}
