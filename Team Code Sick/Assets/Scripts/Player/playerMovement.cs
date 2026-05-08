using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] float dashDist;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCooldown;
    float dashCooldownTimer;
    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprint();
        Dash();
    }

    void Movement() //Basic movement using the CharacterController component, with WASD
    {
        moveDir = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
        controller.Move(playerVel * Time.deltaTime);
    }
    
    void Sprint() //Sprinting with left shift key, increases speed by sprintMod, and returns to normal speed when released
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    void Dash()
    {
        if (dashCooldown > 0)
            dashCooldown -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashCooldown <= 0)
        {
            Vector3 dashDir = moveDir.normalized;

            if (dashDir == Vector3.zero)
            {
                dashDir = transform.forward;
            }
            controller.Move(dashDir * dashDist);
            dashCooldownTimer = dashCooldown;
        }
    }
}
