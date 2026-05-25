using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, Idamage
{
    [Header("Components")]
    [SerializeField] public Renderer rend;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public LayerMask ignoreLayer;

    [Header("Stats")]
    [Range(1, 15)][SerializeField] public int HP;
    [Range(1, 15)][SerializeField] public float faceTargetSpeed;
    [Range(1, 10)][SerializeField] public float speed;
    [Range(1, 10)][SerializeField] public float stopDist;

    [Header("Weapons")]
    [SerializeField] public GameObject bullet;
    [Range(0.1f, 2)][SerializeField] public float shootRate;
    [SerializeField] public Transform gunPivot;
    [SerializeField] public Transform shootPos;
    [Range(1, 25)][SerializeField] public int gunRotateSpeed;
    [SerializeField] public ParticleSystem destroyEffect;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;

    //EnemyStats enemyStats = gamemanager.instance.GetComponent<EnemyStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    colorOrig = rend.material.color;
    //    gamemanager.instance.updateEnemyCount(1);
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (gamemanager.instance.playerInRoom)
    //    {
    //    }
    //    //agent.SetDestination(gamemanager.instance.player.transform.position);
    //    playerDir = gamemanager.instance.player.transform.position - transform.position;

    //    rotateGun();
    //    rotateToTarget();
    //    moveToTarget();

    //    shootTimer += Time.deltaTime;

    //    if (shootTimer > shootRate)
    //    {
    //        shoot();
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInTrigger = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInTrigger = false;
    //    }
    //}

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
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

    public IEnumerator flashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
    }

    public void rotateGun()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, rot, Time.deltaTime * gunRotateSpeed);
    }

    public void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }


    public void moveToTarget()
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

    //public virtual void shoot()
    //{
    //    shootTimer = 0;
    //    Instantiate(bullet, shootPos.position, gunPivot.rotation);
    //}
}
