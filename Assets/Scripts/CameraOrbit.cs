using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float rotationSpeed = .2f;

    private float rotationY;
    private Vector3 offset;

    InputAction moveAction;
    InputAction lookAction;

    private bool IsShimmyLocked = false;
    private float ShimmyLockRotation = 1f;

    void Start()
    {
        rotationY = transform.eulerAngles.y;
        offset = target.position - transform.position;

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
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
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }

    public void SetShimmyLocked(bool shimmyLock)
    {
        IsShimmyLocked = shimmyLock;
    }

    public void SetShimmyLockRotation(float rotation)
    {
        ShimmyLockRotation = rotation;
    }
}
