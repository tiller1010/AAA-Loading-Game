using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float targetRange = 15f;
    [SerializeField] private float targetAngle = 120f;
    
    private Transform currentTarget;
    private bool isLocked = false;

    public Transform GetCurrentTarget() => currentTarget;
    public bool IsLocked() => isLocked;

    public void SetLock(bool lockTarget)
    {
        isLocked = lockTarget;
        if (!lockTarget)
        {
            currentTarget = null;
        }
    }

    public void UpdateTarget()
    {
        if (isLocked)
        {
            // Verify target is still valid and in range and alive
            if (currentTarget == null || !IsTargetValid(currentTarget))
            {
                FindBestTarget();
            }
        }
        else
        {
            FindBestTarget();
        }
    }

    private bool IsTargetValid(Transform target)
    {
        if (target == null) return false;
        
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null || !enemy.IsAlive()) return false;

        if (Vector3.Distance(transform.position, target.position) > targetRange) return false;

        return true;
    }

    private void FindBestTarget()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Transform bestTarget = null;
        float closestDistanceSqr = targetRange * targetRange;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy.IsAlive()) continue;

            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);

            if (angle < targetAngle)
            {
                float distSqr = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distSqr;
                    bestTarget = enemy.transform;
                }
            }
        }

        currentTarget = bestTarget;
    }
}
