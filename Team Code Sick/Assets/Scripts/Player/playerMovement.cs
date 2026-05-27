using UnityEngine;
using System.Collections;

/// <summary>
/// Script made by Dai
/// </summary>
public class playerMovement : MonoBehaviour, Idamage
{
    [SerializeField] Renderer rend;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Animator anim;

    public float HP;

    public float speed;
    public float sprintMod;

    [SerializeField] float dashDist;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration = 0.15f;

    [SerializeField] GameObject dashGhost;
    [SerializeField] float ghostSpawnRate = 0.03f;

    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;

    [SerializeField] Transform robotVisual;
    [SerializeField] float robotRotateSpeed = 15f;
    [SerializeField] float modelYRotationOffset = 0f;

    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;


    float dashCooldownTimer;
    bool isDashing;


    float shootTimer;
    float currentSpeed;

    public (int x, int y) playerWorldPosition;

    public float OriginalHP;
    public float OriginalSpeed;
    public float OriginalSprintMod;

    public Color colorOrig;

    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 lastMoveDir;


    void Start()
    {
        OriginalHP = HP;
        OriginalSpeed = speed;
        OriginalSprintMod = sprintMod;
        colorOrig = rend.material.color;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gamemanager.instance.isPaused)
        {
            AimGunAtMouse();
            if (!isDashing)
            {
                Movement();
            }

            Dash();
        }
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
            Shoot();

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0f, z);

        bool moving = moveDir.sqrMagnitude > 0.01f;
        bool sprinting = moving && Input.GetKey(KeyCode.LeftShift);

        anim.SetBool("isMoving", moving);
        anim.SetBool("isSprinting", sprinting);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveDir.normalized;
        }

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
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                gunPivot.rotation = targetRotation;
                robotVisual.rotation = Quaternion.Slerp(robotVisual.rotation, targetRotation * Quaternion.Euler(0f, modelYRotationOffset, 0f), robotRotateSpeed * Time.deltaTime);
            }

            Debug.DrawLine(gunPivot.position, mouseWorldPos, Color.green);
        }
    }

    void Dash()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashCooldownTimer <= 0 && !isDashing)
        {
            Vector3 dashDir = lastMoveDir;

            if (dashDir == Vector3.zero)
            {
                dashDir = gunPivot.forward;
                dashDir.y = 0f;
                dashDir.Normalize();
            }
            StartCoroutine(DashRoutine(dashDir));
            dashCooldownTimer = dashCooldown;
        }
    }
    void SpawnDashGhost()
    {
        if (dashGhost == null || robotVisual == null)
            return;

        Instantiate(dashGhost, robotVisual.position, robotVisual.rotation);
    }


    IEnumerator DashRoutine(Vector3 dashDir)
    {
        isDashing = true;

        float elapsedTime = 0f;
        float ghostTimer = 0f;
        float dashSpeed = dashDist / dashDuration;

        while (elapsedTime < dashDuration)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 inputDir = new Vector3(x, 0f, z);

            if (inputDir.sqrMagnitude > 0.01f)
            {
                dashDir = inputDir.normalized;
            }

            ghostTimer -= Time.deltaTime;

            if (ghostTimer <= 0f)
            {
                SpawnDashGhost();
                ghostTimer = ghostSpawnRate;
            }

            controller.Move(dashDir * dashSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    void Shoot()
    {
        shootTimer = 0;

        anim.SetTrigger("Shoot");

        Vector3 shootDir = gunPivot.forward;
        shootDir.y = 0f;
        shootDir.Normalize();

        Vector3 spawnPos = shootPos.position + shootDir * 0.75f;

        GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.LookRotation(shootDir));

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
        updatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    IEnumerator flashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / OriginalHP;
    }
}
