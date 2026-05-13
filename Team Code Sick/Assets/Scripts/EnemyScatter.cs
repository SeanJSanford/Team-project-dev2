using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyScatter : MonoBehaviour, Idamage
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
    public float spreadAngle = 90;
    public int projectileCount = 10;
    public float bulletSpeed = 10f;
    //float stopDist;
    bool playerInTrigger;
    Vector3 playerDir;

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
}
