using UnityEngine;

/// <summary>
/// Script made by Dai
/// </summary>
public class playerMovement : MonoBehaviour, Idamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    [SerializeField] float dashDist;
    
    [SerializeField] float dashCooldown;

    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;

    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;


    float dashCooldownTimer;
    float shootTimer;
    int currentSpeed;

    public (int x, int y) playerWorldPosition;


    Vector3 moveDir;
    Vector3 playerVel;

    // Update is called once per frame
    void Update()
    {
        
        AimGunAtMouse();
        Movement();
       // Sprint();
        Dash();
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
            Shoot();

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0f, z);

        currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * sprintMod;
        }

        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
    }

    void AimGunAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane aimPlane = new Plane(Vector3.up, gunPivot.position);

        if (aimPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);

            Vector3 lookDir = mouseWorldPos - gunPivot.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                gunPivot.rotation = Quaternion.LookRotation(lookDir);
            }

            Debug.DrawLine(gunPivot.position, mouseWorldPos, Color.green);
        }
    }
    //void Sprint() //Sprinting with left shift key, increases speed by sprintMod, and returns to normal speed when released
    //{
    //    if (Input.GetButtonDown("Sprint"))
    //    {
    //        speed *= sprintMod;
    //    }
    //    else if (Input.GetButtonUp("Sprint"))
    //    {
    //        speed /= sprintMod;
    //    }
    //}
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

        Vector3 shootDir = gunPivot.forward;
        shootDir.y = 0f;
        shootDir.Normalize();

        Vector3 spawnPos = shootPos.position + shootDir * 0.75f;

        GameObject newProjectile = Instantiate( projectile, spawnPos, Quaternion.LookRotation(shootDir));

        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

        damage dmgScript = newProjectile.GetComponent<damage>();

        if (dmgScript != null)
        {
            dmgScript.SetOwner(gameObject);
        }

        if (rb != null)
        {
            rb.linearVelocity = shootDir * projectileSpeed;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
    }
}
