using UnityEngine;

public class PlayerCombat : MonoBehaviour
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

    /*
 * Update
 * 
 * Main combat update loop.
 * 
 * Handles:
 * - Mouse aiming
 * - Aim line visualization
 * - Fire rate timing
 * - Shooting input
 */

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

    /*
 * AimAtMouse
 * 
 * Converts the mouse position into a world-space point
 * using a raycast against a flat gameplay plane.
 * 
 * This allows mouse aiming to function correctly
 * with the angled/isometric gameplay camera.
 * 
 * The player rotates toward the calculated aim direction.
 */

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

    /*
 * TryShoot
 * 
 * Checks whether the player is allowed to fire.
 * 
 * Uses WeaponStats fire rate values to control
 * attack speed timing.
 * 
 * If enough time has passed since the last shot:
 * - Shoot() is called
 * - The fire timer resets
 */

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

    /*
 * Shoot
 * 
 * Performs a forward raycast using the player's
 * current aim direction.
 * 
 * Combat Flow:
 * - Pull damage/range values from WeaponStats
 * - Fire a raycast forward
 * - Detect hit targets
 * - Check for IDamageable
 * - Apply TakeDamage()
 * 
 * Why Raycasts:
 * Raycasts provide instant-hit combat behavior,
 * making them ideal for:
 * - Fast gameplay testing
 * - Responsive combat
 * - Prototype weapon systems
 * 
 * Future Expansion Ideas:
 * - Projectile weapons
 * - Critical hits
 * - Headshot detection
 * - Weapon spread
 * - Hit VFX/SFX
 */

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

    /*
 * UpdateAimLine
 * 
 * Updates the LineRenderer used for the aiming laser.
 * 
 * The laser:
 * - Extends forward based on weapon range
 * - Stops early if it hits geometry
 * - Provides visual feedback for aiming direction
 * 
 * Primarily used for:
 * - Combat readability
 * - Aim visualization
 * - Debugging weapon range
 */

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

/*
========================================================
Project: Team Code Sick
Script: PlayerCombat.cs

Primary Developers:
- Avery Wilson
- Dai

Combat Framework Support:
- Sean

System Category:
- Player Combat
- Mouse Aiming System
- Player Movement Integration

Purpose:
- Handles player mouse-based aiming and raycast shooting.
- Integrates with Dai's PlayerMovement so movement and aiming
  remain separated during gameplay.

Core Responsibilities:
- Mouse aiming
- Player facing direction
- Raycast shooting
- Runtime fire-rate timing
- Weapon range handling
- Aim line visualization
- IDamageable combat interaction

Integration Notes:
- PlayerMovement handles player position and movement.
- PlayerCombat handles player aiming/facing direction.
- WeaponStats provides runtime combat values:
    - Damage
    - Fire Rate
    - Range
- IDamageable allows combat interactions to remain modular.

Connected Team Systems:
- Dai: PlayerMovement / angled camera setup
- Avery: WeaponStats / combat stat integration
- Sean: Combat framework support / future weapon systems
- Nilo: Gameplay direction oversight

Combat Workflow:
Mouse Position ->
Ground Plane Raycast ->
Player Rotates Toward Aim ->
Shoot Input ->
WeaponStats Retrieves Final Values ->
Raycast Shot ->
IDamageable Receives Damage

Design Philosophy:
This script intentionally focuses ONLY on:
- Player aiming
- Shooting logic
- Combat interaction flow

Responsibilities intentionally excluded:
- Movement handling
- Inventory systems
- Stat calculations
- Enemy AI
- Loot systems
- UI management

These systems remain separated into
their own modular gameplay systems.

Development Notes:
- Adapted from Avery's earlier prototype combat testing code.
- Updated to support Dai's movement and camera systems.
- Uses a ground-plane raycast so mouse aiming functions
  correctly with the angled/isometric gameplay camera.
- Built to support scalable modular combat architecture.

Current Features:
- Mouse aiming
- Raycast shooting
- Runtime weapon stat scaling
- Aim laser visualization
- Fire rate timing
- Shared IDamageable combat interaction

Future Expansion Ideas:
- Projectile weapons
- Critical hits
- Headshot detection
- Weapon recoil
- Alternate fire modes
- Weapon spread
- Hit effects
- Audio feedback
- Weapon swapping
- Multiplayer combat synchronization
========================================================
*/