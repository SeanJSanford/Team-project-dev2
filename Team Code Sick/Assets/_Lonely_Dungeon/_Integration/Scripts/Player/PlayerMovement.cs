using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Aiming")]
    [SerializeField] private float mouseDeadZone = 1f;

    private PlayerStats playerStats;

    private Vector3 moveDirection;
    private Vector3 playerVelocity;

    private float dashCooldownTimer;
    private bool isSprinting;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        LookAtMouse();
        Move();
        HandleSprint();
        HandleDash();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection =
            transform.right * horizontal +
            transform.forward * vertical;

        float activeSpeed = GetMoveSpeed();

        controller.Move(
            moveDirection.normalized * activeSpeed * Time.deltaTime
        );

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleSprint()
    {
        isSprinting = Input.GetButton("Sprint");
    }

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            return;

        Vector3 lookDirection = hit.point - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude < mouseDeadZone * mouseDeadZone)
            return;

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void HandleDash()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (!Input.GetButtonDown("Dash") || dashCooldownTimer > 0f)
            return;

        Vector3 dashDirection = moveDirection.normalized;

        if (dashDirection == Vector3.zero)
            dashDirection = transform.forward;

        controller.Move(dashDirection * dashDistance);

        dashCooldownTimer = dashCooldown;
    }

    private float GetMoveSpeed()
    {
        float activeSpeed = playerStats != null
            ? playerStats.MoveSpeed
            : moveSpeed;

        if (isSprinting)
            activeSpeed *= sprintMultiplier;

        return activeSpeed;
    }
}

/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: PlayerMovement.cs

Primary Developer:
- Dai

Integration Support:
- Avery Wilson

System Category:
- Player Movement
- Camera-relative Movement
- Gameplay Feel

Purpose:
- Handles player movement, sprinting, mouse-facing,
  and dash behavior.
- Keeps player movement separate from combat,
  health, and stat systems.

Connected Systems:
- PlayerStats
- PlayerCombat
- PlayerHealth
- _PlayerCamera
- EnemyCombat

Design Notes:
This script should not handle shooting, HP, or damage.
Those responsibilities belong to PlayerCombat and PlayerHealth.

Future Expansion Ideas:
- Dash invincibility frames
- Stamina cost
- Animation support
- Controller support
- Movement stat scaling
========================================================
*/