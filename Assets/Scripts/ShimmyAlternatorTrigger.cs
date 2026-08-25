using UnityEngine;
using TMPro;

public class ShimmyAlternatorTrigger : MonoBehaviour
{
    [SerializeField] TMP_Text hudText;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            PlayerControls playerControls = otherGameObject.GetComponent<PlayerControls>();
            playerControls.canAlternateShimmyDirection = true;
            hudText.SetText("<-- Go left or right -->");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            PlayerControls playerControls = otherGameObject.GetComponent<PlayerControls>();
            playerControls.canAlternateShimmyDirection = false;
            hudText.SetText("");
        }
    }
}
