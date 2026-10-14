using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Interact : MonoBehaviour
{
  public float interactDistance = 3.5f;
  public string npcDialogue = "Hello there!";
  private bool canInteract = false;
  private bool isTalking = false;
  public GameObject InteractTooltipPrefab;
  private GameObject InteractTooltipInstance;
  private Animator animator;

  InputAction interactAction;
  [SerializeField] TMP_Text hudText;
  public TextUpdateHelper textUpdateHelper;

  void Start()
  {
    interactAction = InputSystem.actions.FindAction("Interact");
    textUpdateHelper = new TextUpdateHelper();
    textUpdateHelper.hudText = hudText;
    animator = GetComponent<Animator>();
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
    if (isTalking) return false;

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
    if (isTalking) return;
    isTalking = true;
    canInteract = false;
    transform.LookAt(GameObject.FindWithTag("Player").transform);

    // Keep upright
    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    textUpdateHelper.SetText(npcDialogue);
    animator.SetBool("IsTalking", true);
    StartCoroutine(EndTalkAfterDelay(3f));
    if (InteractTooltipInstance != null)
    {
        InteractTooltipInstance.SetActive(false);
    }
  }
  
  IEnumerator EndTalkAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    isTalking = false;
    animator.SetBool("IsTalking", false);
    textUpdateHelper.ClearText();
    if (InteractTooltipInstance != null)
    {
        InteractTooltipInstance.SetActive(canInteract);
    }
  }

}

