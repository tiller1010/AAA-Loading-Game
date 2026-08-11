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
    private int iterations = 0;
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
            transform.position = newPosition;
            transform.LookAt(target);

            CheckForObstructions();
            if (obstructingObject != null)
            {
                float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
                float distanceFromObstructingObject = Vector3.Distance(transform.position, obstructingObject.transform.position);
                //float averageObstructionSize = (obstructingObject.transform.localScale.x + obstructingObject.transform.localScale.y + obstructingObject.transform.localScale.z) / 3f;

                Collider[] colliders = Physics.OverlapSphere(
                    transform.position,
                    0.01f
                );
                bool isInObstruction = colliders.Length > 0;

                Debug.Log("Distance from player: " + distanceFromPlayer);
                Debug.Log("Distance from obstruction: " + distanceFromObstructingObject);
                Debug.Log("camera is in a collider: " + isInObstruction);
                // if (distanceFromObstructingObject - averageObstructionSize < distanceFromPlayer)
                if ((distanceFromObstructingObject < distanceFromPlayer || isInObstruction) && distanceFromPlayer > .5f)
                {
                    //offset.x -= Time.deltaTime;
                    //offset.z -= Time.deltaTime;
                    //offset.y -= Time.deltaTime;

                    //transform.position = Vector3.MoveTowards(transform.position, target.position, 5 * Time.deltaTime);
                    //while (IsObstructedByGameWorld() && distanceFromPlayer > 1f && iterations < 100)
                    //{
                    //    Debug.Log("is obstructed");
                    //    // Move camera towards player
                    Vector3 playerCenter = target.position;
                    playerCenter.y += 1;
                    transform.position = Vector3.MoveTowards(transform.position, playerCenter, Time.deltaTime);
                    offset = target.position - transform.position;
                    offset.x = Mathf.Abs(offset.x);
                    offset.z = Mathf.Abs(offset.z);
                    //  iterations++;
                    //}
                    //iterations = 0;
                }
                else if (Mathf.Abs(offset.x) < originalOffset.x || Mathf.Abs(offset.z) < originalOffset.z)
                {
                    // move away from player until the original offset is reached
                    //Vector3 originalPositionByOffset = target.position - (Quaternion.Euler(rotationX, rotationY, 0) * originalOffset);
                    //transform.position = Vector3.MoveTowards(transform.position, originalPositionByOffset, Time.deltaTime);
                    //offset = target.position - transform.position;
                    //offset.x = Mathf.Abs(offset.x);
                    //offset.z = Mathf.Abs(offset.z);

                    //if (offset.x < originalOffset.x)
                    //{
                    //    offset.x += Time.deltaTime;
                    //}
                    //if (offset.z < originalOffset.z)
                    //{
                    //    offset.z += Time.deltaTime;
                    //}
                    //if (offset.y < originalOffset.y)
                    //{
                    //    offset.y += Time.deltaTime;
                    //}
                    //offset.x = originalOffset.x;
                    //offset.z = originalOffset.z;
                }
                //else
                //{
                //    while (!IsObstructedByGameWorld() && distanceFromPlayer < originalDistanceFromPlayer && iterations < 100)
                //    {
                //        // Move camera away from player
                //      transform.position = Vector3.MoveTowards(transform.position, target.position - (Quaternion.Euler(rotationX, rotationY, 0) * offset), 5 * Time.deltaTime);
                //      iterations++;
                //    }
                //    iterations = 0;
                //}
            }

        }
    }

    public void CheckForObstructions()
    {
        RaycastHit obstacleCheck;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out obstacleCheck);
        if (!obstacleCheck.collider) return;
        obstructingObject = obstacleCheck.collider.gameObject;
    }

    public bool IsObstructedByGameWorld()
    {
        RaycastHit obstacleCheck;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out obstacleCheck);
        if (!obstacleCheck.collider) return false;
        Debug.Log("camera obstruction: " + obstacleCheck.collider.gameObject.name);
        return obstacleCheck.collider.gameObject.CompareTag("GameWorld");
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
