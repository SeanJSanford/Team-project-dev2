using System.Buffers.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScatter : MonoBehaviour, Idamage
{
    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Rigidbody rb;
    [SerializeField] public ParticleSystem destroyEffect;

    [Header("Stats")]
    [Range(1, 15)][SerializeField] int baseHP;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;
    [Range(1, 10)][SerializeField] float speed;
    [Range(1, 10)][SerializeField] float stopDist;

    [Header("Weapons")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [Range(0, 25)][SerializeField] int gunRotateSpeed;
    [Range(.1f, 5)][SerializeField] float shootRate;

    float HP;
    int floorsCleared = 1;
    float growthRate = 1.15f;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    public float spreadAngle = 90;
    public int projectileCount = 10;
    public float bulletSpeed = 10f;
    bool playerInTrigger;
    Vector3 playerDir;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = rend.material.color;
        Rigidbody rb = GetComponent<Rigidbody>();
        HP = baseHP * Mathf.Pow(growthRate, floorsCleared);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
            playerDir = gamemanager.instance.player.transform.position - transform.position;

            rotateGun();
            rotateToTarget();
            moveToTarget();

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                scatterShot();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
    void scatterShot()
    {
        shootTimer = 0;
        float angleStep = spreadAngle / (projectileCount - 1);
        float startAngle = -spreadAngle / 2;
        for (int i = 0; i < projectileCount; i++)
        {
            // Calculate spread rotation
            float angle = startAngle + i * angleStep;
            Quaternion rotation = shootPos.rotation * Quaternion.Euler(0, angle, 0);
            // Spawn and shoot projectile
            GameObject proj = Instantiate(bullet, shootPos.position, rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = proj.transform.forward * bulletSpeed;
        }
        Instantiate(bullet, shootPos.position, gunPivot.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

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

    void rotateGun()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, rot, Time.deltaTime * gunRotateSpeed);
    }

    void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void moveToTarget()
    {
        float distance = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);
        
        if (playerInTrigger)
        {
            
            Vector3 direction = (transform.position - gamemanager.instance.player.transform.position).normalized;
            
            if (distance >= stopDist)
                transform.position -= direction * speed * Time.deltaTime;
        }
    }
}
