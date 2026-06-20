using System.Buffers.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour, Idamage, ICharacter
{
    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] public ParticleSystem destroyEffect;
    [SerializeField] Animator anim;

    [Header("Stats")]
    [Range(1, 15)][SerializeField] int baseHP;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;
    [Range(1, 10)][SerializeField] float stopDist;
    [Range(1, 5)][SerializeField] int damage;
    [Range(1, 3)][SerializeField] float pauseDuration;
    [Range(.5f, 2)][SerializeField] float charge;
    [Range(1, 3)][SerializeField] float attackCooldown;

    [SerializeField] float _Speed;
    [SerializeField] float _Damage;
    [SerializeField] float _Resistance;
    [SerializeField] float _shootRate;


    Color colorOrig;
    float angleToPlayer;
    bool playerInTrigger;
    bool canAttack = true;
    bool canMove = true;
    bool isAttacking;

    Vector3 playerDir;

    public float HP { get; set; }
    public float speed { get; set; }
    public float Damage { get; set; }
    public float Resistance { get; set; }
    public bool timerLock { get; set; }
    public float shootRate { get; set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        colorOrig = rend.material.color;
        HP = DifficultyRampUp.instance.EnemyHPRampUp(baseHP);
        speed = _Speed;
        Damage = _Damage;
        Resistance = _Resistance;
        shootRate = _shootRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamemanager.instance.playerInRoom)
            return;

        playerDir = gamemanager.instance.player.transform.position - transform.position;
        playerDir.y = 0f;


        rotateToTarget();

        if (isAttacking)
        {
            if (anim != null)
                anim.SetBool("isRunning", false);

            return;
        }


        if (canMove)
        {
            moveToTarget();

            if (anim != null)
                anim.SetBool("isRunning", true);
        }
        else
        {
            if (anim != null)
                anim.SetBool("isRunning", false);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            StartCoroutine(AttackPlayer(other.gameObject));
        }
    }

    IEnumerator AttackPlayer(GameObject playerObj)
    {
        isAttacking = true;
        canMove = false;
        canAttack = false;

        if (anim != null)
        {
            anim.SetBool("isRunning", false);
            anim.SetTrigger("Attack");
        }
        
        yield return new WaitForSeconds(charge);

        Vector3 toPlayer = playerObj.transform.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance <= stopDist + 0.5f)
        {
            Idamage playerHealth = playerObj.GetComponent<Idamage>();

            if (playerHealth != null)
            {
                playerHealth.takeDamage(damage);
            }
        }

        yield return new WaitForSeconds(pauseDuration);


        yield return new WaitForSeconds(attackCooldown);

        canMove = true;
        canAttack = true;
        isAttacking = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount / Resistance;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
            GetComponent<EnemyLoot>().DropLoot();
            if (EnemySpawnsCenter.instance.currentEnemies.Count == 1)
                FindObjectOfType<PlayerSkillPoints>().AddEnemyKill();
            EnemySpawnsCenter.instance.RemoveEnemy(gameObject);
            destroyEffect.transform.position = gameObject.transform.position;
            Destroy(gameObject);
            Instantiate(destroyEffect);
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

    void rotateToTarget()
    {
        if (playerDir.sqrMagnitude < 0.01f)
            return;

        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void moveToTarget()
    {
        Vector3 targetPos = gamemanager.instance.player.transform.position;
        Vector3 direction = targetPos - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > stopDist)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
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
