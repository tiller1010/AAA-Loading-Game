using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private float speed = 3;
    private bool alive;
    private bool moving;
    private bool playerIsDetected;
    private Transform playerTransform;

    void Start()
    {
        alive = true;
        StartCoroutine("WaitAndChangeDirection");
    }

    void Update()
    {
        if (alive)
        {
            if (playerIsDetected && playerTransform)
            {
                // Rotate to face the player
                Vector3 direction = playerTransform.position - transform.position;
                direction.y = 0; // Keep the rotation on the horizontal plane
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                }
            }
            if (moving)
            {
                Move();
            }
        }
    }

    void Move()
    {
        if (playerIsDetected && playerTransform)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (distanceToPlayer < 1.5f) return;
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    IEnumerator WaitAndChangeDirection()
    {
        if (playerIsDetected) yield break;
        moving = false;
        yield return new WaitForSeconds(5);
        transform.Rotate(0, Random.Range(-110, 110), 0);
        moving = true;
        yield return new WaitForSeconds(5);
        StartCoroutine("WaitAndChangeDirection");
    }

    public void OnFOVDetect(Transform player)
    {
        Debug.Log("enemy sees player");
        playerIsDetected = true;
        moving = true;
        playerTransform = player;
    }
}
