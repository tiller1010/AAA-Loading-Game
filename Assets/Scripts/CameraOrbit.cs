using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float rotationSpeed = .2f;

    private float rotationY;
    private float rotationX;
    private Vector3 offset;
    private Vector3 originalOffset;
    private float originalDistanceFromPlayer;
    private GameObject obstructingObject;

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
        originalDistanceFromPlayer = Vector3.Distance(transform.position, target.position);

        rotationX = transform.rotation.eulerAngles.x;
        rotationY = transform.rotation.eulerAngles.y;

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

        if (!IsShimmyLocked && lookValue.y != 0)
        {
            rotationX += lookValue.y * rotationSpeed * -3;
            if (rotationX < -40)
            {
                rotationX = -40;
            }
            else if (rotationX > 40)
            {
                rotationX = 40;
            }
        }

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 newPosition = target.position - (rotation * offset);

        if (IsShimmyLocked)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5 * Time.deltaTime);
        }
        else
        {
            CheckForObstructions(newPosition);
            if (obstructingObject != null)
            {
                float distanceFromPlayer = Vector3.Distance(newPosition, target.position);
                float distanceFromObstructingObject = Vector3.Distance(newPosition, obstructingObject.transform.position);

                Collider[] colliders = Physics.OverlapSphere(
                    newPosition,
                    0.01f
                );
                bool isInObstruction = colliders.Length > 0;

                Debug.Log("Distance from player: " + distanceFromPlayer);
                Debug.Log("Distance from obstruction: " + distanceFromObstructingObject);
                Debug.Log("camera is in a collider: " + isInObstruction);
                if ((distanceFromObstructingObject < distanceFromPlayer || isInObstruction) && distanceFromPlayer > .5f)
                {
                    Vector3 playerCenter = target.position;
                    playerCenter.y += 1;
                    newPosition = Vector3.MoveTowards(newPosition, playerCenter, Time.deltaTime);
                    //offset = target.position - transform.position;
                    //offset.x = Mathf.Abs(offset.x);
                    //offset.z = Mathf.Abs(offset.z);
                }
                else if (Mathf.Abs(offset.x) < originalOffset.x || Mathf.Abs(offset.z) < originalOffset.z)
                {
                    // move away from player until the original offset is reached
                    //Vector3 originalPositionByOffset = newPosition - (Quaternion.Euler(rotationX, rotationY, 0) * originalOffset);
                    //newPosition = Vector3.MoveTowards(newPosition, originalPositionByOffset, Time.deltaTime);
                    //offset = target.position - transform.position;
                    //offset.x = Mathf.Abs(offset.x);
                    //offset.z = Mathf.Abs(offset.z);
                }
            }

        }

        transform.position = newPosition;
        transform.LookAt(target);
    }

    public void CheckForObstructions(Vector3 newPosition)
    {
        RaycastHit obstacleCheck;
        Physics.Raycast(newPosition, transform.TransformDirection(Vector3.forward), out obstacleCheck);
        if (!obstacleCheck.collider) return;
        obstructingObject = obstacleCheck.collider.gameObject;
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
