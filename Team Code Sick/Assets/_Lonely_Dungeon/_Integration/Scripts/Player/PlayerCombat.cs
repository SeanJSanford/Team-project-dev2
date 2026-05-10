using UnityEngine;

/*
 * PlayerTopDownShooter
 * 
 * Purpose:
 * Handles mouse-based aiming and raycast shooting.
 * 
 * Camera Setup:
 * This works with an angled/isometric camera by raycasting
 * from the mouse position down onto a flat ground plane.
 * 
 * Movement and aiming are separate:
 * - WASD controls movement
 * - Mouse controls facing/shooting direction
 */

public class PlayerTopDownShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private WeaponStats weaponStats;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private float aimHeight = 0.5f;

    [Header("Aim Visual")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float laserHeight = 0.5f;

    private float shootTimer;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (weaponStats == null)
            weaponStats = GetComponent<WeaponStats>();
    }

    private void Update()
    {
        AimAtMouse();
        UpdateAimLine();

        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            TryShoot();
        }
    }

    private void AimAtMouse()
    {
        if (mainCamera == null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);

            Vector3 aimDirection = point - transform.position;
            aimDirection.y = 0f;

            if (aimDirection.sqrMagnitude > 0.01f)
            {
                transform.forward = aimDirection.normalized;
            }
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

        Vector3 origin =
            transform.position + Vector3.up * aimHeight;

        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * range, Color.red, 1f);

        if (Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                range,
                ~ignoreLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);

            IDamageable damageable =
                hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log("Damage dealt: " + damage);
            }
        }
    }

    private void UpdateAimLine()
    {
        if (aimLine == null || weaponStats == null)
            return;

        float range = weaponStats.GetRange();

        Vector3 start = transform.position + Vector3.up * laserHeight;
        Vector3 end = start + transform.forward * range;

        if (Physics.Raycast(start, transform.forward, out RaycastHit hit, range, ~ignoreLayer))
        {
            end = hit.point;
        }

        aimLine.positionCount = 2;
        aimLine.SetPosition(0, start);
        aimLine.SetPosition(1, end);
    }
}