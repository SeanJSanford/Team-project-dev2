using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour//, Idamage
{
    //[SerializeField] Renderer rend;
    //[SerializeField] NavMeshAgent agent;
    //[SerializeField] LayerMask ignoreLayer;

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
    //float stopDist;
    bool playerInTrigger;
    Vector3 playerDir;
    EnemyBase enemyBase;

    //EnemyStats enemyStats = gamemanager.instance.GetComponent<EnemyStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        colorOrig = enemyBase.rend.material.color;
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
    void shoot()
    {
        shootTimer = 0;
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
