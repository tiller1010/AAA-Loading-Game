using UnityEngine;

public class ShimmyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            PlayerControls playerControls = gameObject.GetComponent<PlayerControls>();
            playerControls.shimmyTriggers++;
            if (!playerControls.GetIsShimmying())
            {
                playerControls.SetMoveSpeed(1f);
                playerControls.SetRotationSpeed(0f);
                playerControls.SetRotation(transform.rotation);

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

            playerControls.SetIsShimmying(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            PlayerControls playerControls = gameObject.GetComponent<PlayerControls>();
            playerControls.shimmyTriggers--;
            if (playerControls.shimmyTriggers < 0)
            {
                playerControls.shimmyTriggers = 0;
            }
            if (playerControls.shimmyTriggers > 0)
            {
                return;
            }
            playerControls.SetIsShimmying(false);
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
}
