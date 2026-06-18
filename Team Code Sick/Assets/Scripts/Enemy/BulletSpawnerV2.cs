using UnityEngine;

public class BulletSpawnerV2 : MonoBehaviour, IBulletSpawner
{

    [Header("Bullet Object")]
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;

    public float spreadAngle = 90;
    public int projectileCount = 10;
    float enragedFirerate = 1.5f;
    int enragedProjectileCount = 36;
    float shootTimer;
    Vector3 playerDir;
    bool isEnraged = Boss1.phase2;

    public Element elementType { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //playerDir = gamemanager.instance.player.transform.position - transform.position;
        shootTimer -= Time.deltaTime;
        //transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 1f, 0f);
        //if (isEnraged == true)
        //{
        //    projectileCount = enragedProjectileCount;
        //    fireRate = enragedFirerate;
        //}

        //rotateToTarget();
        if (shootTimer < 0)
        {
            scatterShot();
        }
    }

    public void SetElement(Element element)
    {
        elementType = element;
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, transform.position, transform.rotation);
    }

    void scatterShot()
    {
        shootTimer = 1 / shootRate;
        float angleStep = spreadAngle / (projectileCount - 1);
        float startAngle = -spreadAngle / 2;
        for (int i = 0; i < projectileCount; i++)
        {
            // Calculate spread rotation
            float angle = startAngle + i * angleStep;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);
            // Spawn and shoot projectile
            GameObject bulletGO = Instantiate(bullet, transform.position, rotation);
            damage dmgScript = bulletGO.GetComponent<damage>();
            if (dmgScript)
                dmgScript.SetElement(elementType);
            Rigidbody rb = bulletGO.GetComponent<Rigidbody>();
            rb.linearVelocity = bulletGO.transform.forward * bulletSpeed;
        }
    }

    void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
