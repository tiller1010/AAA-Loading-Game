using UnityEngine;

public class ShimmyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("Shimmying", true);

            PlayerControls playerControls = gameObject.GetComponent<PlayerControls>();
            playerControls.SetMoveSpeed(1f);
            playerControls.SetRotationSpeed(0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("Shimmying", false);

            PlayerControls playerControls = gameObject.GetComponent<PlayerControls>();
            playerControls.SetMoveSpeed(3f);
            playerControls.SetRotationSpeed(5f);
        }
    }
}
