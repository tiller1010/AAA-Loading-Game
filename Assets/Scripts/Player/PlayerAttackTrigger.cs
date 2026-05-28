using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.tag == "Enemy")
        {
            Debug.Log("gotcha!");

            Enemy enemy = otherGameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.SetHealth(enemy.GetHealth() - 50);
            }

            //Animator animator = gameObject.GetComponent<Animator>();
            //if (!animator.GetBool("TakingDamage"))
            //{
            //}

            //animator.SetBool("TakingDamage", true);
        }
    }
}
