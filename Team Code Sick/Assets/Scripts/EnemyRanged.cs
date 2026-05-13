using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour, Idamage
{
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

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
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
        }
        agent.SetDestination(gamemanager.instance.player.transform.position);
        playerDir = gamemanager.instance.player.transform.position - transform.position;

            rotateGun();
            rotateToTarget();

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
            gamemanager.instance.updateGameGoal(-1);
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
}
