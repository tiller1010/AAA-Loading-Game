using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    private int health = 100;

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }
}
