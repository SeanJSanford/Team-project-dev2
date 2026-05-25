using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyScatter : MonoBehaviour//, Idamage
{
    //[SerializeField] Renderer rend;
    //[SerializeField] NavMeshAgent agent;
    //[SerializeField] LayerMask ignoreLayer;
    //[SerializeField] Rigidbody rb;

    //[SerializeField] int HP;
    //[SerializeField] float faceTargetSpeed;
    //[SerializeField] float speed;
    //[SerializeField] float stopDist;

    //[SerializeField] GameObject bullet;
    //[SerializeField] float shootRate;
    //[SerializeField] Transform gunPivot;
    //[SerializeField] Transform shootPos;
    //[SerializeField] int gunRotateSpeed;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    public float spreadAngle = 90;
    public int projectileCount = 10;
    public float bulletSpeed = 10f;
    bool playerInTrigger;
    Vector3 playerDir;
    EnemyBase enemyBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        colorOrig = enemyBase.rend.material.color;
        Rigidbody rb = GetComponent<Rigidbody>();
        gamemanager.instance.updateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
        }
        //agent.SetDestination(gamemanager.instance.player.transform.position);
        playerDir = gamemanager.instance.player.transform.position - transform.position;

        enemyBase.rotateGun();
        enemyBase.rotateToTarget();
        enemyBase.moveToTarget();

        shootTimer += Time.deltaTime;

        if (shootTimer > enemyBase.shootRate)
        {
            scatterShot();
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
            Quaternion rotation = enemyBase.shootPos.rotation * Quaternion.Euler(0, angle, 0);
            // Spawn and shoot projectile
            GameObject proj = Instantiate(enemyBase.bullet, enemyBase.shootPos.position, rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = proj.transform.forward * bulletSpeed;
        }
        Instantiate(enemyBase.bullet, enemyBase.shootPos.position, enemyBase.gunPivot.rotation);
    }

    //public void takeDamage(int amount)
    //{
    //    enemyBase.HP -= amount;

    //    if (enemyBase.HP <= 0)
    //    {
    //        gamemanager.instance.updateEnemyCount(-1);
    //        GetComponent<EnemyLoot>().DropLoot();
    //        FindObjectOfType<PlayerSkillPoints>().AddEnemyKill();
    //        Instantiate(enemyBase.destroyEffect);
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        StartCoroutine(flashRed());
    //    }
    //}

    //IEnumerator flashRed()
    //{
    //    enemyBase.rend.material.color = Color.red;
    //    yield return new WaitForSeconds(0.1f);
    //    enemyBase.rend.material.color = colorOrig;
    //}

    //void rotateGun()
    //{
    //    Quaternion rot = Quaternion.LookRotation(playerDir);
    //    enemyBase.gunPivot.rotation = Quaternion.Lerp(enemyBase.gunPivot.rotation, rot, Time.deltaTime * enemyBase.gunRotateSpeed);
    //}

    //void rotateToTarget()
    //{
    //    Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
    //    transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * enemyBase.faceTargetSpeed);
    //}

    //void moveToTarget()
    //{
    //    float distance = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);
    //    // Move only if farther than the stop distance
    //    if (playerInTrigger)
    //    {
    //        // Find the direction toward the player
    //        Vector3 direction = (transform.position - gamemanager.instance.player.transform.position).normalized;
    //        // Move toward the player
    //        transform.position -= direction * enemyBase.speed * Time.deltaTime;
    //        transform.position = new Vector3(transform.position.x, transform.position.y / transform.position.y, transform.position.z);
    //        transform.LookAt(gamemanager.instance.player.transform);
    //    }
    //}
}
