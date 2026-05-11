using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CharacterController controller;

    [Header("Sprint")]
    [SerializeField] private float sprintMultiplier = 1.5f;
    private bool isSprinting;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 1f;

    private float dashCooldownTimer;

    private Vector3 moveDir;

    private void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    private void Update()
    {
        Sprint();
        Movement();
        Dash();
    }

    /*
     * Movement
     * 
     * Reads directional input and moves the player.
     */
    private void Movement()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        moveDir =
            Input.GetAxisRaw("Horizontal") * camRight +
            Input.GetAxisRaw("Vertical") * camForward;

        float moveSpeed = playerStats != null
            ? playerStats.GetStatValue(StatType.MoveSpeed)
            : 6f;

        if (isSprinting)
        {
            moveSpeed *= sprintMultiplier;
        }

        controller.Move(
            moveDir.normalized * moveSpeed * Time.deltaTime
        );
    }

    /*
     * Sprint
     * 
     * Temporarily increases movement speed while Sprint is held.
     */
    private void Sprint()
    {
        isSprinting = Input.GetButton("Sprint");
    }

    /*
     * Dash
     * 
     * Moves the player quickly in the current movement direction.
     * If no movement input is held, dashes forward.
     */
    private void Dash()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Dash") && dashCooldownTimer <= 0f)
        {
            Vector3 dashDir = moveDir.normalized;

            if (dashDir == Vector3.zero)
            {
                dashDir = transform.forward;
            }

            controller.Move(dashDir * dashDistance);

            dashCooldownTimer = dashCooldown;
        }
    }
}

/*
========================================================
Project: Team Code Sick
Script: PlayerMovement.cs

Primary Developer:
- Dai

Integration Support:
- Avery Wilson

System Category:
- Player Movement
- CharacterController Movement
- Camera-Relative Movement

Purpose:
- Handles player movement, sprinting, and dashing.
- Uses Dai's movement setup as the primary player
  locomotion framework.
- Reads movement values from PlayerStats to support
  Avery's runtime stat architecture.

Current Features:
- WASD movement
- Camera-relative movement
- CharacterController movement
- Sprint multiplier
- Dash movement
- Dash cooldown
- Runtime MoveSpeed stat integration

Connected Team Systems:
- Dai: Player movement implementation
- Avery: PlayerStats / MoveSpeed integration
- Sean: Combat gameplay interaction support
- Nilo: Gameplay direction oversight

How The System Works:
Movement Input ->
Camera Relative Direction ->
PlayerStats Retrieves MoveSpeed ->
CharacterController Applies Movement ->
Sprint/Dash Modifiers Applied

Design Philosophy:
This script intentionally handles ONLY:
- Player movement
- Sprint behavior
- Dash behavior
- Runtime locomotion

Responsibilities intentionally excluded:
- Shooting systems
- Weapon stats
- Enemy damage
- Loot interactions
- Inventory logic
- Camera follow logic

These systems remain separated into
their own gameplay systems.

Important Integration Notes:
- PlayerMovement controls player position and locomotion.
- PlayerCombat controls player aiming/facing direction.
- Camera-relative movement depends on PlayerCamera.
- Runtime movement scaling comes from PlayerStats.

If multiple scripts attempt to rotate the player,
movement and aiming conflicts may occur.

Why This Separation Exists:
Separating movement from combat and gameplay systems:
- Improves scalability
- Simplifies debugging
- Reduces Git conflicts
- Supports modular gameplay architecture

Development Notes:
- Uses CharacterController for movement handling.
- Built to support fast gameplay iteration.
- Designed around top-down/isometric gameplay readability.
- Intended to integrate cleanly with:
    - PlayerCombat
    - PlayerCamera
    - PlayerStats

Current Limitations:
- No animation integration
- No slope handling
- No stamina system
- No knockback handling
- No movement state machine
- No root motion support

Future Expansion Ideas:
- Stamina systems
- Dodge invulnerability frames
- Animation state integration
- Slow/freeze effects
- Movement abilities
- Knockback reactions
- Multiplayer movement syncing
- Advanced movement states
========================================================
*/