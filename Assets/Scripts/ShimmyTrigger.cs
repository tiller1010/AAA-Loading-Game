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

            Vector3 shimmyStartPosition = transform.TransformPoint(Vector3.back / 2f);
            shimmyStartPosition.y = .71f; // Set character y position

            playerControls.SetShimmyStartPosition(shimmyStartPosition);

            GameObject camera = GameObject.Find("Main Camera");
            if (camera != null)
            {
                CameraOrbit cameraOrbit = camera.GetComponent<CameraOrbit>();
                cameraOrbit.SetShimmyLocked(true);
                cameraOrbit.SetShimmyLockRotation(transform.rotation.eulerAngles.y);
            }
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

            GameObject camera = GameObject.Find("Main Camera");
            if (camera != null)
            {
                CameraOrbit cameraOrbit = camera.GetComponent<CameraOrbit>();
                cameraOrbit.SetShimmyLocked(false);
            }
        }
    }

    void Update()
    {
        //transform.position = transform.forward * 1.1f * Time.deltaTime;
        //transform.Translate(0, 0, 1.1f * Time.deltaTime);

    }
}
