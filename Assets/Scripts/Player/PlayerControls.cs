using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    //[SerializeField] Transform target;

    private float moveSpeed = 6.0f;

    InputAction moveAction;

    private CharacterController characterController;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Debug.Log(horizontalInput);

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        //Debug.Log(moveValue);

        movement = movement.normalized;
        movement.x = moveValue.x;
        movement.z = moveValue.y;

        movement *= moveSpeed;
        movement *= Time.deltaTime;

        characterController.Move(movement);
    }
}
