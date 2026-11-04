using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGravity : MonoBehaviour
{
    InputAction jumpAction;
    private CharacterController characterController;
    PlayerControls playerControls;

    public float jumpSpeed = 15;
    private bool isOnGround = true;
    public bool canJump = true;
    public float gravity = -9f;
    public float terminalVelocity = -10;
    public float minFallSpeed = -1.5f;

    private float verticalSpeed;
    private Animator animator;

    public AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;

    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        characterController = GetComponent<CharacterController>();
        playerControls = GetComponent<PlayerControls>();
        animator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void SyncAudioSource()
    {
      audioSource = playerControls.audioSource;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        bool hitGround = false;
        RaycastHit groundRaycastHit;
        if (verticalSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit))
        {
            bool newHitGround = groundRaycastHit.distance <= .12f;
            if (newHitGround && !isOnGround)
            {
                SyncAudioSource();
                if (audioSource != null && landingClip != null)
                {
                    audioSource.PlayOneShot(landingClip);
                    Debug.Log("landing sound played");
                }
            }

            hitGround = newHitGround;
            isOnGround = newHitGround;
        }

        if (hitGround)
        {
            if (animator != null) animator.SetBool("Jumping", false);

            if (jumpAction.triggered && canJump)
            {
                if (animator != null) animator.SetBool("Jumping", true);
                verticalSpeed = jumpSpeed;
                SyncAudioSource();
                if (audioSource != null && jumpClip != null)
                {
                    audioSource.PlayOneShot(jumpClip);
                    Debug.Log("Jump sound played");
                }
            }
            else
            {
                verticalSpeed = minFallSpeed;
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

            movement.y = verticalSpeed * Time.deltaTime;
            characterController.Move(movement);
        }
    }
}
