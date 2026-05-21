using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something is in this");
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            Debug.Log("ouch!");

            //Animator animator = gameObject.GetComponent<Animator>();
            //if (!animator.GetBool("TakingDamage"))
            //{
            //}

            //animator.SetBool("TakingDamage", true);
        }
    }
}
