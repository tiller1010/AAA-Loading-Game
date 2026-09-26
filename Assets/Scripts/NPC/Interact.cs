using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Interact : MonoBehaviour
{
  public float interactDistance = 3.5f;
  public string npcDialogue = "Hello there!";
  private bool canInteract = false;
  public GameObject InteractTooltipPrefab;
  private GameObject InteractTooltipInstance;

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
    if (InteractTooltipInstance != null)
    {
      InteractTooltipInstance.SetActive(canInteract);
    }

    if (canInteract)
    {
      if (InteractTooltipInstance == null)
      {
        InteractTooltipInstance = Instantiate(InteractTooltipPrefab, transform.position + Vector3.up * .1f, Quaternion.identity);
      }
      else
      {
        InteractTooltipInstance.transform.position = transform.position + Vector3.up * .1f;
      }

      InteractTooltipInstance.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

      if (interactAction.triggered)
      {
        // Destroy(InteractTooltipInstance);
        InteractAction();
      }
    }

    else
    {
      if (InteractTooltipInstance != null)
      {
        Destroy(InteractTooltipInstance);
      }
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

