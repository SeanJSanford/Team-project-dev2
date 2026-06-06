using UnityEngine;
using System.Collections;

/// <summary>
/// Script made by Dai
/// </summary>
public class playerMovement : MonoBehaviour, Idamage
{
    [Header("Sources")]
    [SerializeField] Renderer rend;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
   

    [Header("Audio")]

    [SerializeField] AudioSource audPlayer;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 0.3f)] [SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 0.3f)] [SerializeField] float audHurtVol;

    [SerializeField] AudioClip audDash;
    [Range(0, 0.3f)] [SerializeField] float audDashVol;

    [SerializeField] AudioClip audShoot;
    [Range(0, 0.3f)][SerializeField] float audShootVol;

    bool isPlayingStep;
    bool isSprinting;

    [Header("Stats")]
    public float HP;
    public float speed;
    public float sprintMod;

    [Header("Dashing Stats")]

    [SerializeField] float dashDist;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration = 0.15f;
    [SerializeField] GameObject dashGhost;
    [SerializeField] float ghostSpawnRate = 0.03f;

    [Header("Gun Components")]
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;

    [Header("Gun Stats")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;


    [Header("Misc")]
    [SerializeField] float iFrameDuration = 0.5f;
    bool isInvincible;
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
        isSprinting = moving && Input.GetKey(KeyCode.LeftShift);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveDir.normalized;
        }

        currentSpeed = speed;

        if (isSprinting)
        {
            currentSpeed = speed * sprintMod;
        }

        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

        if (moveDir.magnitude > 0.3f && !isPlayingStep)
        {
            StartCoroutine(playStep());
        }
    }

    IEnumerator playStep()
    {
        isPlayingStep = true;

        if (audSteps != null && audSteps.Length > 0)
            audPlayer.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (isSprinting)
        {
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isPlayingStep = false;
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

    IEnumerator DashRoutine(Vector3 dashDir)
    {
        isDashing = true;

        if (audDash != null)
            audPlayer.PlayOneShot(audDash, audDashVol);

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

    void SpawnDashGhost()
    {
        if (dashGhost == null)
            return;

        Instantiate(dashGhost, transform.position, gunPivot.rotation);
    }

    void Shoot()
    {
        shootTimer = 0;
        audPlayer.PlayOneShot(audShoot, audShootVol);

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
        if(IsInvincible())
        return;

        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamageScreen());
        audPlayer.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
        else
        {
            StartCoroutine(flashRed());
            StartCoroutine(IFrameRoutine());
        }
    }
    IEnumerator flashDamageScreen()
    {
        gamemanager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageScreen.SetActive(false);
    }
    IEnumerator flashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
    }
    IEnumerator IFrameRoutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(iFrameDuration);

        isInvincible = false;
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / OriginalHP;
    }

    public bool IsInvincible()
    {
        return isDashing || isInvincible;
    }
}
