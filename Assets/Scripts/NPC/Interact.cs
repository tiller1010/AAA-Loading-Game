using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Interact : MonoBehaviour
{
  public float interactDistance = 3.5f;
  public string npcDialogue = "Hello there!";
  public string actionDialogue = "I'll open it for you!";
  public string afterActionDialogue = "The door is open now.";
  private bool canInteract = false;
  private bool isTalking = false;
  private bool actionPerformed = false;

  public GameObject InteractTooltipPrefab;
  private GameObject InteractTooltipInstance;
  private Animator animator;

  InputAction interactAction;
  [SerializeField] TMP_Text hudText;
  public TextUpdateHelper textUpdateHelper;

  private AudioSource audioSource;
  public List<AudioClip> talkingAudioClips = new List<AudioClip>();

  void Start()
  {
    interactAction = InputSystem.actions.FindAction("Interact");
    textUpdateHelper = new TextUpdateHelper();
    textUpdateHelper.hudText = hudText;
    animator = GetComponent<Animator>();
    audioSource = this.gameObject.AddComponent<AudioSource>();
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
    IActionable actionable = GetComponent<IActionable>();
    if (actionable != null && actionable.CanAct)
    {
      actionable.ExecuteAction();
      actionPerformed = true;
      Talk(actionDialogue);
      return; 
    }

    if (actionPerformed)
    {
      Talk(afterActionDialogue);
    }
    else
    {
      Talk(npcDialogue);
    }
  }

  void Talk(string dialogue)
  {
    if (isTalking) return;
    isTalking = true;
    canInteract = false;
    transform.LookAt(GameObject.FindWithTag("Player").transform);

    // Keep upright
    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    textUpdateHelper.SetText(dialogue, this.PlayRandomAudioClip);
    animator.SetBool("IsTalking", true);
    StartCoroutine(EndTalkAfterDelay(3f));
    if (InteractTooltipInstance != null)
    {
        InteractTooltipInstance.SetActive(false);
    }
  }

  public void PlayRandomAudioClip()
  {
    if (talkingAudioClips.Count == 0) return;

    audioSource.Stop();

    int randomIndex = Random.Range(0, talkingAudioClips.Count);
    AudioClip clip = talkingAudioClips[randomIndex];
    audioSource.PlayOneShot(clip);
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

