using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemySplit : MonoBehaviour//, Idamage
{
    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] public ParticleSystem destroyEffect;

    [Header("Stats")]
    [Range(1, 15)][SerializeField] int HP;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;
    [Range(1, 10)][SerializeField] float speed;
    [Range(1, 10)][SerializeField] float stopDist;

    [Header("Weapons")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [Range(1, 25)][SerializeField] int gunRotateSpeed;
    [Range(.1f, 2)][SerializeField] float shootRate;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;
    

    //EnemyStats enemyStats = gamemanager.instance.GetComponent<EnemyStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = rend.material.color;
        //gamemanager.instance.updateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
        }
            playerDir = gamemanager.instance.player.transform.position - transform.position;

            moveToTarget();
            rotateGun();
            rotateToTarget();

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                shoot();
            }
        //agent.SetDestination(gamemanager.instance.player.transform.position);
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
    void shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            shootTimer = 0;
            Instantiate(bullet, shootPos.position, gunPivot.rotation);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            //gamemanager.instance.updateGameGoal(-1);
            GetComponent<EnemyLoot>().DropLoot();
            FindObjectOfType<PlayerSkillPoints>().AddEnemyKill();
            Instantiate(destroyEffect);
            Destroy(gameObject);
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
        // Move only if farther than the stop distance
        if (playerInTrigger)
        {
            // Find the direction toward the player
            Vector3 direction = (transform.position - gamemanager.instance.player.transform.position).normalized;
            // Move toward the player
            transform.position -= direction * speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y / transform.position.y, transform.position.z);
            transform.LookAt(gamemanager.instance.player.transform);
        }
    }
}
