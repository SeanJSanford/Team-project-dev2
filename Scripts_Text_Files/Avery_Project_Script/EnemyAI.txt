using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("Enemy Health")]
    [SerializeField] private int maxHP = 40;
    [SerializeField] private int currentHP;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Transform healthBarFill;

    [SerializeField] int faceTargetSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [SerializeField] int gunRotateSpeed;

    private Color originalColor;
    private Vector3 originalHealthBarScale;


    float shootTimer;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;

    private void Start()
    {
        currentHP = maxHP;

        if (rend == null)
            rend = GetComponentInChildren<Renderer>();

        originalColor = rend.material.color;

        if (healthBarFill != null)
            originalHealthBarScale = healthBarFill.localScale;
    }

    void Update()
    {
        if (playerInTrigger)
        {
            playerDir = gamemanager.instance.player.transform.position - transform.position;

            rotateGun();
            rotateToTarget();

            shootTimer += Time.deltaTime;

            if (shootTimer > shootRate)
            {
                shoot();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHealthBar();

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            GetComponent<LootDrop>()?.DropLoot();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent = (float)currentHP / maxHP;

        healthBarFill.localScale = new Vector3(
            originalHealthBarScale.x * healthPercent,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
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