using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    [Header("Bullet Object")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;

    public float spreadAngle = 90;
    public int projectileCount = 10;
    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 1f, 0f);

        if (shootTimer > fireRate)
        {
            Shoot();
            shootTimer = 0;
        }
        else
        {
            scatterShot();
            shootTimer = 0;
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, transform.position, Quaternion.identity);
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
}
