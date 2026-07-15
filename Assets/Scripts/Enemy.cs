using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private float speed = 3;
    private bool alive = true;
    private bool moving = false;
    private bool wandering = true;
    private bool playerIsDetected;
    private bool isAttacking = false;

    private Transform playerTransform;
    public PlayerProperties playerProperties;

    [SerializeField] private GameObject attackTriggerPrefab;
    private GameObject attackTrigger;
    public float attackTriggerPositionZAdjustment = 0;
    public float attackTriggerPositionYAdjustment = 0;
    private NavMeshAgent navMeshAgent;

    private int health = 100;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("WaitAndChangeDirection");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (alive)
        {
            if (playerIsDetected && playerTransform)
            {
                StopCoroutine("WaitAndChangeDirection");

                // Rotate to face the player
                Vector3 direction = playerTransform.position - transform.position;
                //direction.y = 0; // Keep the rotation on the horizontal plane
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                }
            }

            if (playerProperties.GetIsAlive())
            {
                PursuePlayer();
            }
            else
            {
                playerIsDetected = false;
                if (navMeshAgent.isOnNavMesh) navMeshAgent.isStopped = true;
                animator.SetBool("Attacking", false);
                if (!wandering)
                {
                    wandering = true;
                    StartCoroutine("WaitAndChangeDirection");
                }
            }

            if (moving)
            {
                Move();
            }
        }
    }

    void PursuePlayer()
    {
        if (!playerTransform) return;

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer < 1)
        {
            moving = false;
            if (!isAttacking) StartCoroutine("AttackPlayer");
            animator.SetBool("Running", false);
        }
        else
        {
            moving = true;
        }
    }

    void Move()
    {
        animator.SetBool("Attacking", false);
        animator.SetBool("Running", true);

        if (playerIsDetected && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(.75f);

        attackTrigger = Instantiate(attackTriggerPrefab);
        Vector3 attackTriggerPosition = transform.position + transform.forward;
        attackTriggerPosition.z += attackTriggerPositionZAdjustment;
        attackTriggerPosition.y += attackTriggerPositionYAdjustment;
        attackTrigger.transform.position = attackTriggerPosition;
        attackTrigger.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(.75f);

        Destroy(attackTrigger);
        isAttacking = false;
    }

    IEnumerator WaitAndChangeDirection()
    {
        if (playerIsDetected) yield break;

        animator.SetBool("Running", false);
        moving = false;

        yield return new WaitForSeconds(7);

        transform.Rotate(0, Random.Range(-110, 110), 0);
        moving = true;

        yield return new WaitForSeconds(5);

        StartCoroutine("WaitAndChangeDirection");
    }

    public void OnFOVDetect(Transform player)
    {
        if (!playerProperties.GetIsAlive()) return;

        playerIsDetected = true;
        wandering = false;
        moving = true;
        playerTransform = player;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine("DamageAnimation");
        }
    }

    IEnumerator DamageAnimation()
    {
        animator.SetBool("TakingDamage", true);
        yield return new WaitForSeconds(.25f);
        animator.SetBool("TakingDamage", false);
    }

    public void Die()
    {
        Destroy(attackTrigger);
        health = 0;
        alive = false;
        animator.SetBool("Alive", false);
    }
}
