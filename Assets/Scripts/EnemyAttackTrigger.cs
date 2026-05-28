using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.tag == "Player")
        {
            PlayerProperties playerProperties = otherGameObject.GetComponent<PlayerProperties>();

            if (playerProperties != null)
            {
                playerProperties.SetHealth(playerProperties.GetHealth() - 20);
                Messenger<int>.Broadcast("PLAYER_HEALTH_UPDATED", playerProperties.GetHealth());
            }

            //Animator animator = gameObject.GetComponent<Animator>();
            //if (!animator.GetBool("TakingDamage"))
            //{
            //}

            //animator.SetBool("TakingDamage", true);
        }
    }
}
