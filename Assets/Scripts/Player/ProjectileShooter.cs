using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootStartPoint;
    [SerializeField] private Vector3 spawnAngle;

    public void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("You have not set prefab yet.");
            return;
        }

        Instantiate(projectilePrefab, shootStartPoint.position, transform.rotation * Quaternion.Euler(spawnAngle));
    }
}
