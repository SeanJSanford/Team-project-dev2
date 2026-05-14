using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyMelee : MonoBehaviour, Idamage
{
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] float faceTargetSpeed;
    [SerializeField] float speed;
    [SerializeField] float stopDist;
    [SerializeField] int damage;
    [SerializeField] float pauseDuration;
    [SerializeField] float attackCooldown;
    [SerializeField] float knockback;

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
        //gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {

        }
            //agent.SetDestination(gamemanager.instance.player.transform.position);
            float stopDist = agent.stoppingDistance;
            playerDir = gamemanager.instance.player.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, new Vector3(playerDir.x, transform.position.y, playerDir.z));


            rotateToTarget();
            moveToTarget();
            if (distance <= stopDist && canAttack)
            {
                StartCoroutine(AttackPlayer());
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
            //gamemanager.instance.updateGameGoal(-1);
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
    void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        canMove = false;
        // Damage
        Idamage playerHealth = gamemanager.instance.player.GetComponent<Idamage>();
        if (playerHealth != null)
            playerHealth.takeDamage(damage);
        // Knockback
        Rigidbody playerRb = gamemanager.instance.player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 knockDir = (playerRb.transform.position - transform.position).normalized;
            playerRb.AddForce(knockDir * knockback, ForceMode.Impulse);
        }
        // Pause enemy briefly after attack
        yield return new WaitForSeconds(pauseDuration);
        canMove = true;
        // Wait before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void moveToTarget()
    {
        float distance = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);
        // Move only if farther than the stop distance
        if (distance > stopDist)
        {
            // Find the direction toward the player
            Vector3 direction = (gamemanager.instance.player.transform.position - transform.position).normalized;
            // Move toward the player
            transform.position += direction * speed * Time.deltaTime;
            //Rigidbody rb = GetComponent<Rigidbody>();
           // rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            transform.LookAt(gamemanager.instance.player.transform);
        }
    }
}
