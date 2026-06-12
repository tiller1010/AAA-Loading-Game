using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGravity : MonoBehaviour
{
    InputAction jumpAction;
    private CharacterController characterController;
    PlayerControls playerControls;

    public float jumpSpeed = 15;
    public float gravity = -9f;
    public float terminalVelocity = -10;
    public float minFallSpeed = -1.5f;

    //private float currentSpeed = 0f;
    private float verticalSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        characterController = GetComponent<CharacterController>();
        playerControls = GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        bool hitGround = false;
        RaycastHit groundRaycastHit;
        if (verticalSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit))
        {
            Debug.Log(groundRaycastHit.transform.gameObject.name);
            float hitGroundCheck = (characterController.height + characterController.radius) / 1.9f;
            hitGround = groundRaycastHit.distance * 15 <= hitGroundCheck;

            if (jumpAction.triggered)
            {
                Debug.Log("raycast distance" + groundRaycastHit.distance);
                Debug.Log("hitground" + hitGroundCheck);
            }
        }

        if (hitGround)
        {
            if (jumpAction.triggered)
            {
                verticalSpeed = jumpSpeed;
            }
            else
            {
                verticalSpeed = minFallSpeed;
                // stop jump animation?
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

            // stop jump animation?

            //if (characterController.isGrounded)
            //{
            //    if (Vector3.Dot(movement, contact.normal) < 0)
            //    {
            //        //movement = contact.normal * moveSpeed;
            //    }
            //    else
            //    {
            //        //movement += contact.normal * moveSpeed;
            //    }
            //}

            //playerControls.movement.y = verticalSpeed * Time.deltaTime;
            movement.y = verticalSpeed * Time.deltaTime;
            characterController.Move(movement);
        }
    }
}
