using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour, Idamage
{
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int speed;
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
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
            agent.SetDestination(gamemanager.instance.player.transform.position);
            float stopDist = agent.stoppingDistance;
            playerDir = gamemanager.instance.player.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, new Vector3(playerDir.x, transform.position.y, playerDir.z));


            rotateToTarget();
            if (distance <= stopDist && canAttack)
            {
                StartCoroutine(AttackPlayer());
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
}
