using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.tag == "Enemy")
        {
            Enemy enemy = otherGameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.SetHealth(enemy.GetHealth() - 50);
            }
        }
    }
}
