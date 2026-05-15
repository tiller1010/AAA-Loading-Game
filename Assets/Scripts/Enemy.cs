using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private float speed = 3;
    private bool alive;
    private bool moving;

    void Start()
    {
        alive = true;
        StartCoroutine("WaitAndChangeDirection");
    }

    void Update()
    {
        if (alive && moving)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    IEnumerator WaitAndChangeDirection()
    {
        moving = false;
        yield return new WaitForSeconds(5);
        transform.Rotate(0, Random.Range(-110, 110), 0);
        moving = true;
        yield return new WaitForSeconds(5);
        StartCoroutine("WaitAndChangeDirection");
    }
}
