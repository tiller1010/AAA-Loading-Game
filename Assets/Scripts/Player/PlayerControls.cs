using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] Transform target;

    public float rotationSpeed;
    public float moveSpeed;

    InputAction moveAction;
    InputAction attackAction;

    private CharacterController characterController;

    private Animator animator;

    private Vector3? shimmyStartPosition;

    private bool isAttacking = false;

    [SerializeField] private GameObject attackTriggerPrefab;
    private GameObject attackTrigger;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        float horizontalInput = 0f;

        if (!animator.GetBool("Shimmying"))
        {
            horizontalInput = moveValue.x;
        }

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
        }
        else
        {
            animator.SetBool("Running", false);
            if (animator.GetBool("Shimmying"))
            {
                animator.speed = 0f;
            }
        }

        if (shimmyStartPosition != null)
        {
            transform.position = (Vector3)shimmyStartPosition;
            SetShimmyStartPosition(null);
        }
        else
        {
            movement *= moveSpeed;
            movement *= Time.deltaTime;

            characterController.Move(movement);
        }

        if (attackAction.triggered && !isAttacking)
        {
            animator.SetBool("Attacking", true);
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(.25f);

        attackTrigger = Instantiate(attackTriggerPrefab);
        Vector3 attackTriggerPosition = transform.position + transform.forward;
        attackTriggerPosition.y = transform.position.y + 1;
        attackTrigger.transform.position = attackTriggerPosition;
        attackTrigger.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(1);

        animator.SetBool("Attacking", false);
        Destroy(attackTrigger);
        isAttacking = false;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void SetShimmyStartPosition(Vector3? position)
    {
        shimmyStartPosition = position;
    }
}
