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
    private PlayerProperties playerProperties;

    private Vector3? shimmyStartPosition;
    private bool isAttacking = false;
    private int attackIndex = 0;
    private int attackAnimationsCount = 3;
    private bool isShimmying = false;
    private bool canPauseAnimations = false;

    [SerializeField] private GameObject attackTriggerPrefab;
    private GameObject attackTrigger;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerProperties = GetComponent<PlayerProperties>();
    }

    void Update()
    {
        if (!playerProperties.GetIsAlive()) return;

        Vector3 movement = Vector3.zero;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        float horizontalInput = 0f;

        if (!isShimmying)
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
            if (isShimmying && canPauseAnimations)
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

        if (!isShimmying && attackAction.triggered && !isAttacking && !PauseMenu.GameIsPaused)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetBool("Attacking", true);
        attackIndex = attackIndex + 1;
        if (attackIndex > attackAnimationsCount)
        {
            attackIndex = 1;
        }
        animator.SetInteger("AttackIndex", attackIndex);

        // Delay attack trigger until animation finishes
        yield return new WaitForSeconds(.3f);

        attackTrigger = Instantiate(attackTriggerPrefab);
        Vector3 attackTriggerPosition = transform.position + transform.forward;
        attackTriggerPosition.y = transform.position.y + 1;
        attackTrigger.transform.position = attackTriggerPosition;
        attackTrigger.transform.rotation = transform.rotation;

        isAttacking = false;
        animator.SetBool("Attacking", false);

        StartCoroutine("ResetAttackIndex");

        yield return new WaitForSeconds(.2f);

        Destroy(attackTrigger);
    }

    IEnumerator CanPauseAnimationsTimeout()
    {
        yield return new WaitForSeconds(1.25f);
        if (isShimmying) canPauseAnimations = true;
    }

    IEnumerator ResetAttackIndex()
    {
        yield return new WaitForSeconds(.5f);
        if (!isAttacking)
        {
            attackIndex = 0;
            animator.SetInteger("AttackIndex", 0);
        }
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

    public bool GetIsShimmying()
    {
        return isShimmying;
    }

    public void SetIsShimmying(bool newIsShimmying)
    {
        // Allow transition to shimmying to occur before being able to pause the animation
        if (newIsShimmying)
        {
            StartCoroutine("CanPauseAnimationsTimeout");
        }
        else
        {
            canPauseAnimations = false;
        }

        isShimmying = newIsShimmying;
        animator.SetBool("Shimmying", isShimmying);
    }

    public void SetShimmyStartPosition(Vector3? position)
    {
        shimmyStartPosition = position;
    }
}
