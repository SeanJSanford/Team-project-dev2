using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss1 : MonoBehaviour, Idamage
{

    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] public ParticleSystem destroyEffect;

    [Header("Roaming")]
    [SerializeField] float roamRadius;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;
    [SerializeField] float reachedThreshold;
    private Vector3 spawnPoint;
    private Vector3 targetDestination;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    [Header("Stats")]
    [Range(20, 100)][SerializeField] int baseHP;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;
    [Range(1, 10)][SerializeField] float stopDist;
    [SerializeField] float _Speed;
    [SerializeField] float _Damage;
    [SerializeField] float _Resistance;
    [SerializeField] float _shootRate;

    public static Boss1 instance;
    public static bool phase2 = false;
    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;
    (int x, int y) originalCenter = LevelCreation.instance.allCenters[gamemanager.instance.currentRoom];

    public float HP { get; set; }
    public float speed { get; set; }
    public float Damage { get; set; }
    public float Resistance { get; set; }
    public bool timerLock { get; set; }
    public float shootRate { get; set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        (int x, int y) roomWorldPosition = (originalCenter.x * gamemanager.instance.unitSize, originalCenter.y * gamemanager.instance.unitSize);
        instance = this;
        colorOrig = rend.material.color;
        HP = DifficultyRampUp.instance.EnemyHPRampUp(baseHP);
        speed = _Speed;
        Damage = _Damage;
        Resistance = _Resistance;
        shootRate = _shootRate;
        spawnPoint = new Vector3(roomWorldPosition.x, 1, roomWorldPosition.y);
        //PickNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        //rotateToTarget();
        if (HP <= (HP * 0.5))
        {
            phase2 = true;
        }

        //if (isWaiting)
        //{
        //    waitTimer -= Time.deltaTime;
        //    if (waitTimer <= 0f)
        //    {
        //        isWaiting = false;
        //        PickNewDestination();
        //    }
        //}
        //else
        //{
        //    roam();
        //    if (Vector3.Distance(transform.position, targetDestination) <= reachedThreshold)
        //    {
        //        isWaiting = true;
        //        waitTimer = Random.Range(waitTimeMin, waitTimeMax);
        //    }
        //}

    }

    public void takeDamage(int amount)
    {
        HP -= amount / Resistance;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
            //GetComponent<EnemyLoot>().DropLoot();
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
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);
    }

    void roam()
    {
        Vector3 direction = (targetDestination - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void PickNewDestination()
    {
        Vector2 randomCircle = Random.insideUnitCircle * roamRadius;
        targetDestination = spawnPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);
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
