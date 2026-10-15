using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    [Header("Target Action")]
    public GameObject actionTarget;
    public bool activateOnEnter = true;
    public bool activateOnExit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetActionState(activateOnEnter);
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         SetActionState(activateOnExit);
    //     }
    // }

    private void SetActionState(bool state)
    {
        if (actionTarget == null) return;

        IActionable actionable = actionTarget.GetComponent<IActionable>();
        if (actionable != null)
        {
            actionable.CanAct = state;
            Debug.Log($"ActionTrigger: Set {actionTarget.name} CanAct to {state}");
        }
        else
        {
            Debug.LogWarning($"ActionTrigger: No IActionable component found on {actionTarget.name}");
        }
    }
}
