using UnityEngine;

/// <summary>
/// Script made by Dai
/// </summary>
public class playerMovement : MonoBehaviour, Idamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int HP;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    [SerializeField] float dashDist;
    
    [SerializeField] float dashCooldown;

    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;


    float dashCooldownTimer;
    float shootTimer;

    public (int x, int y) playerWorldPosition;


    Vector3 moveDir;
    Vector3 playerVel;

    // Update is called once per frame
    void Update()
    {
        
        Movement();
        Sprint();
        Dash();
        AimGunAtMouse();
    }

    void Movement() //Basic movement using the CharacterController component, with WASD
    {
        
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
            Shoot();


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0f, z);

        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }
    void AimGunAtMouse()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 lookDir = hit.point - gunPivot.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                gunPivot.rotation = Quaternion.LookRotation(lookDir);
            }
        }
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
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashCooldownTimer <= 0)
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

    void Shoot()
    {
        shootTimer = 0;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            Idamage dmg = hit.collider.GetComponent<Idamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
        Debug.DrawRay(shootPos.position, gunPivot.forward * shootDist, Color.red, 1f);
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
    }
}
