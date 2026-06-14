using UnityEngine;
using System.Collections;

/// <summary>
/// Script made by Dai
/// </summary>
public class playerMovement : MonoBehaviour, Idamage, ICharacter
{


    [Header("Sources")]
    [SerializeField] Renderer rend;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Transform visualHolder;
    [SerializeField] float visualTurnSpeed = 15f;
    [SerializeField] float visualYawOffset = 0f;

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

    [SerializeField] float _HP;
    [SerializeField] float _Speed;
    [SerializeField] float _Damage;
    [SerializeField] float _Resistance;
    public float sprintMod;

    public float HP { get; set; }
    public float speed { get; set; }
    public float Damage { get; set; }
    public float Resistance { get; set; }
    public bool timerLock { get; set; }


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
    [SerializeField] int shootDist;
    [SerializeField] float _shootRate;

    public float shootRate { get; set; }


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
        // Setting Stats from Inspector

        HP = _HP;
        speed = _Speed;
        Damage = _Damage;
        Resistance = _Resistance;
        shootRate = _shootRate;

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
        shootTimer -= Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer < 0)
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
                Quaternion aimRotation = Quaternion.LookRotation(lookDir);

              
                gunPivot.rotation = aimRotation;

              
                if (visualHolder != null)
                {
                    Quaternion visualRotation = aimRotation * Quaternion.Euler(0f, visualYawOffset, 0f);

                    visualHolder.rotation = Quaternion.Slerp(
                        visualHolder.rotation,
                        visualRotation,
                        visualTurnSpeed * Time.deltaTime
                    );
                }
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
            dmgScript.damageAmount = Damage;
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

        HP -= amount / Resistance;
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
    public void ModifyStat(NxStatType stat, float amount)
    {
        switch (stat)
        {
            case NxStatType.HP:
                HP += amount;
                break;

            case NxStatType.Speed:
                speed += amount;
                break;

            case NxStatType.Damage:
                Damage += amount;
                break;

            case NxStatType.Resistance:
                Resistance += amount;
                break;

            case NxStatType.FireRate:
                shootRate += amount;
                break;
        }
    }
    public void SetStat(NxStatType stat, float amount)
    {
        switch (stat)
        {
            case NxStatType.HP:
                HP = amount;
                break;

            case NxStatType.Speed:
                speed = amount;
                break;

            case NxStatType.Damage:
                Damage = amount;
                break;

            case NxStatType.Resistance:
                Resistance = amount;
                break;

            case NxStatType.FireRate:
                shootRate = amount;
                break;
        }
    }

    public float GetStat(NxStatType stat)
    {
        switch (stat)
        {
            case NxStatType.HP:
                return HP;

            case NxStatType.Speed:
                return speed;

            case NxStatType.Damage:
                return Damage;

            case NxStatType.Resistance:
                return Resistance;

            case NxStatType.FireRate:
                return shootRate;

            default: 
                return 0f;
        }
    }
}
