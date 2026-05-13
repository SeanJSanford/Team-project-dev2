using UnityEngine;

public class SplitProjectile : MonoBehaviour
{
    public GameObject splitProjectile;  // Prefab for spawned projectiles
    public int splitCount = 8;                // How many to spawn when splitting
    public float splitSpreadAngle = 45f;      // Total spread between the projectiles
    public float splitSpeed = 10f;            // Speed of new projectiles
    private bool hasSplit = false;
    
    void OnDestroy()
    {
        if (!hasSplit && gameObject.scene.isLoaded) // Ensure not called on scene unload
            Split();
    }
    void Split()
    {
        if (hasSplit) return;
        hasSplit = true;
        float angleStep = splitSpreadAngle / (splitCount - 1);
        for (int i = 0; i < splitCount; i++)
        {
            float angle = -splitSpreadAngle / 2 + i * angleStep;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);
            GameObject newProjectile = Instantiate(
                splitProjectile,
                transform.position,
                rotation
            );
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = rotation * Vector3.forward * splitSpeed;
            }
        }
        Destroy(gameObject);
    }
}
