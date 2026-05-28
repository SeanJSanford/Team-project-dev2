using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyMelee : MonoBehaviour, Idamage
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
    [Range(1, 5)][SerializeField] int damage;
    [Range(1, 3)][SerializeField] float pauseDuration;
    [Range(.5f, 2)][SerializeField] float charge;
    [Range(1, 3)][SerializeField] float attackCooldown;

    Color colorOrig;
    float angleToPlayer;
    bool playerInTrigger;
    bool canAttack = true;
    bool canMove = true;
    Vector3 playerDir;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
            playerDir = gamemanager.instance.player.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, new Vector3(playerDir.x, transform.position.y, playerDir.z));


            rotateToTarget();
            if (canMove)
                moveToTarget();
            if (distance <= stopDist)
            {
                StartCoroutine(AttackPlayer());
                wait();
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

    IEnumerator AttackPlayer()
    {
        canMove = false;
        canAttack = false;
        // Damage
        Idamage playerHealth = gamemanager.instance.player.GetComponent<Idamage>();
        yield return new WaitForSeconds(charge);
        float distance = Vector3.Distance(transform.position, new Vector3(playerDir.x, transform.position.y, playerDir.z));
        if (distance <= stopDist + 2)
        {
            playerHealth.takeDamage(damage);
        }
        // Pause enemy briefly after attack
        yield return new WaitForSeconds(pauseDuration);
        canMove = true;
        // Wait before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator wait()
    {
        canMove = false;
        canAttack = false;
        yield return new WaitForSeconds(pauseDuration);
        canMove = true;
        // Wait before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
            GetComponent<EnemyLoot>().DropLoot();
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
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }


    void moveToTarget()
    {
        float distance = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);
        Vector3 direction = (transform.position - gamemanager.instance.player.transform.position).normalized;

        if (distance >= stopDist)
            transform.position -= direction * speed * Time.deltaTime;
    }
}
