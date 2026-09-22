using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Interact : MonoBehaviour
{
  public float interactDistance = 3.5f;
  public string npcDialogue = "Hello there!";

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
    if (interactAction.triggered)
    {
      CheckForInteraction();
    }
  }

  async Awaitable FixedUpdate()
  {
    await textUpdateHelper.FixedUpdate();
  }

  public void CheckForInteraction()
  {
    Transform player = GameObject.FindWithTag("Player").transform;
    if (Vector3.Distance(player.position, transform.position) <= interactDistance)
    {
      Vector3 direction = transform.position - player.position;
      if (Vector3.Dot(player.forward, direction) > .5f)
      {
        Talk();
      }
    }
  }

  void Talk()
  {
    textUpdateHelper.SetText(npcDialogue);
  }

}

