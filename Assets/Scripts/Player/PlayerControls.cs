using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] Transform target;

    public float rotationSpeed;
    public float moveSpeed;

    InputAction moveAction;

    private CharacterController characterController;

    private Animator animator;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveValue.x;
        float verticalInput = moveValue.y;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            movement.x = horizontalInput * moveSpeed;
            movement.z = verticalInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion rotation = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = rotation;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);

            animator.SetBool("Running", true);
            animator.speed = 1f;
        } else
        {
            animator.SetBool("Running", false);
            if (animator.GetBool("Shimmying"))
            {
                animator.speed = 0f;
            }
        }

        movement *= moveSpeed;
        movement *= Time.deltaTime;

        characterController.Move(movement);
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
