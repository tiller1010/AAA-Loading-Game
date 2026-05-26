using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Enemy")
        {
            Debug.Log("gotcha!");

            //Animator animator = gameObject.GetComponent<Animator>();
            //if (!animator.GetBool("TakingDamage"))
            //{
            //}

            //animator.SetBool("TakingDamage", true);
        }
    }
}
