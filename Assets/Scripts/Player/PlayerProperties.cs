using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    private int health = 100;
    private bool alive = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public bool GetIsAlive()
    {
        return alive;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            health = 0;
            alive = false;
            animator.SetBool("Alive", false);
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
}
