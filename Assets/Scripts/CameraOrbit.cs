using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float rotationSpeed = .2f;

    private float rotationY;
    private Vector3 offset;
    private Vector3 originalOffset;

    InputAction moveAction;
    InputAction lookAction;

    public bool IsShimmyLocked = false;
    private float ShimmyLockRotation = 1f;
    private float originalYRotation = 0f;

    void Start()
    {
        offset = target.position - transform.position;
        offset.x = Mathf.Abs(offset.x);
        offset.z = Mathf.Abs(offset.z);
        originalOffset = new Vector3(offset.x, offset.y, offset.z);

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");

        if (IsShimmyLocked)
        {
            SetShimmyLockRotation(transform.rotation.eulerAngles.y);
        }
    }

    void LateUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveValue.x;

        Vector2 lookValue = lookAction.ReadValue<Vector2>();

        if (IsShimmyLocked)
        {
            // Need to set to forward of shimmy trigger
            rotationY = ShimmyLockRotation;

            // Fixes camera centering with cinemachine
            offset.x *= .5f;
        }
        else if (lookValue.x != 0)
        {
            rotationY += lookValue.x * rotationSpeed * 3;
        }
        else
        {
            rotationY += horizontalInput * rotationSpeed * 3;
        }

        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
        Vector3 newPosition = target.position - (rotation * offset);

        if (IsShimmyLocked)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5 * Time.deltaTime);
        }
        else
        {
            transform.position = newPosition;
            transform.LookAt(target);
        }
    }

    public void SetShimmyLocked(bool shimmyLock)
    {
        IsShimmyLocked = shimmyLock;
        if (!shimmyLock)
        {
            offset.x = originalOffset.x;
            offset.z = originalOffset.z;
            rotationY = originalYRotation;
        }
        else
        {
            originalYRotation = rotationY;
        }
    }

    public void SetShimmyLockRotation(float rotation)
    {
        ShimmyLockRotation = rotation;
    }
}
