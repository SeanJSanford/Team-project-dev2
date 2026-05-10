using UnityEngine;

/*
 * PlayerMovement
 * 
 * Purpose:
 * Handles player movement, sprinting, and dashing.
 * 
 * How It Works:
 * - Reads WASD / movement input
 * - Uses CharacterController for movement
 * - Reads MoveSpeed from PlayerStats
 * - Applies sprint as a temporary movement multiplier
 * - Allows dash movement with cooldown timing
 * 
 * Connected Systems:
 * - PlayerStats
 * - StatType.MoveSpeed
 * - CharacterController
 * - GameManager pause state
 * 
 * Design Notes:
 * PlayerStats owns persistent movement scaling.
 * This script only consumes those stats and applies movement behavior.
 * 
 * Sprint is temporary behavior, not a permanent stat change.
 * 
 * Future Expansion Ideas:
 * - DashAmount stat support
 * - Dash invulnerability frames
 * - Stamina system
 * - Slow/freeze debuffs
 * - Movement animation hooks
 */

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
        moveDir =
            Input.GetAxisRaw("Horizontal") * transform.right +
            Input.GetAxisRaw("Vertical") * transform.forward;

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