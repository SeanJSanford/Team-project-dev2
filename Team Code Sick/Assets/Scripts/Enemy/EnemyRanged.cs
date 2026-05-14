using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour, Idamage
{
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] float faceTargetSpeed;
    [SerializeField] float speed;
    [SerializeField] float stopDist;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [SerializeField] int gunRotateSpeed;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    //float stopDist;
    bool playerInTrigger;
    Vector3 playerDir;

    //EnemyStats enemyStats = gamemanager.instance.GetComponent<EnemyStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = rend.material.color;
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

            rotateGun();
            rotateToTarget();
            moveToTarget();

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                shoot();
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

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
            GetComponent<EnemyLoot>().DropLoot();
            FindObjectOfType<PlayerSkillPoints>().AddEnemyKill();
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
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, gunPivot.rotation);
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
