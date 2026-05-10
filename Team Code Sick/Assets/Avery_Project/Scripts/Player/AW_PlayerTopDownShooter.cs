using UnityEngine;

public class AW_PlayerTopDownShooter : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private WeaponStats weaponStats;

    private float shootTimer;

    private void Start()
    {
        if (weaponStats == null)
            weaponStats = GetComponent<WeaponStats>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (weaponStats == null)
        {
            Debug.LogWarning("No WeaponStats found on player.");
            return;
        }

        float fireRate = weaponStats.GetFireRate();

        if (shootTimer < fireRate)
            return;

        Shoot();
        shootTimer = 0f;
    }

    private void Shoot()
    {
        int damage = weaponStats.GetTotalDamage();
        float range = weaponStats.GetRange();

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * range, Color.red, 1f);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, ~ignoreLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);

            AW_IDamage damageable = hit.collider.GetComponent<AW_IDamage>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log("Damage dealt: " + damage);
            }
        }
    }
}