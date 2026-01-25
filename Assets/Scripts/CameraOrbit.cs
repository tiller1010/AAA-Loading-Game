using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float rotationSpeed = .5f;

    private float rotationY;
    private Vector3 offset;

    InputAction moveAction;
    InputAction lookAction;

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

        if (horizontalInput != 0)
        {
            rotationY += horizontalInput * rotationSpeed;
        } else
        {
            rotationY += lookValue.x * rotationSpeed * 3;
        }

        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }
}
