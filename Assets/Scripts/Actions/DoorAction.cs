using UnityEngine;
using System.Collections;

public class DoorAction : MonoBehaviour, IActionable
{
    [Header("Settings")]
    public GameObject door;
    public Vector3 openOffset = new Vector3(0, 0, 2f);
    public float openSpeed = 2f;

    [HideInInspector]
    public bool CanAct { get; set; } = false;

    private bool isOpen = false;

    public void ExecuteAction()
    {
        if (door == null)
        {
            Debug.LogError("DoorAction: No door assigned!", this);
            return;
        }

        if (!isOpen)
        {
            StartCoroutine(OpenDoorRoutine());
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        isOpen = true;
        Vector3 startPos = door.transform.position;
        Vector3 endPos = startPos + openOffset;
        float elapsed = 0;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            door.transform.position = Vector3.Lerp(startPos, endPos, elapsed);
            yield return null;
        }
        door.transform.position = endPos;
    }
}
