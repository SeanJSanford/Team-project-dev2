using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    [Header("Bullet Object")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;

    public float spreadAngle = 90;
    public int projectileCount = 10;
    float enragedFirerate = 0.4f;
    float shootTimer;
    Vector3 playerDir;
    bool isEnraged = Boss1.phase2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        shootTimer += Time.deltaTime;
        //if (isEnraged == true)
        //{
        //    fireRate = enragedFirerate;
        //}

        rotateToTarget();
        if (shootTimer > fireRate)
        {
            scatterShot();
            shootTimer = 0;
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, transform.position, transform.rotation);
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
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);
            // Spawn and shoot projectile
            GameObject proj = Instantiate(bullet, transform.position, rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = proj.transform.forward * bulletSpeed;
        }
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
