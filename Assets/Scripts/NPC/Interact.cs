using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Interact : MonoBehaviour
{
  public float interactDistance = 3.5f;
  public string npcDialogue = "Hello there!";
  private bool canInteract = false;
  public GameObject InteractTooltip;

  InputAction interactAction;
  [SerializeField] TMP_Text hudText;
  public TextUpdateHelper textUpdateHelper;

  void Start()
  {
    interactAction = InputSystem.actions.FindAction("Interact");
    textUpdateHelper = new TextUpdateHelper();
    textUpdateHelper.hudText = hudText;
  }

  void Update()
  {
    canInteract = CheckCanInteract();
    if (InteractTooltip != null)
    {
      InteractTooltip.SetActive(canInteract);
    }

    if (interactAction.triggered && canInteract)
    {
      InteractAction();
    }
  }

  async Awaitable FixedUpdate()
  {
    await textUpdateHelper.FixedUpdate();
  }

  public bool CheckCanInteract()
  {
    Transform player = GameObject.FindWithTag("Player").transform;
    if (Vector3.Distance(player.position, transform.position) <= interactDistance)
    {
      Vector3 direction = transform.position - player.position;
      return Vector3.Dot(player.forward, direction) > .5f;
    }

    return false;
  }

  public void InteractAction()
  {
    Talk();
  }

  void Talk()
  {
    textUpdateHelper.SetText(npcDialogue);
  }

}

