using UnityEngine;

public class PlayerTopDownShooter : MonoBehaviour
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int shootDamage = 10;
    [SerializeField] int shootDist = 20;
    [SerializeField] float shootRate = 0.25f;

    float shootTimer;

    void Update()
    {
        shootTimer += Time.deltaTime;

        Debug.DrawRay(transform.position + Vector3.up, transform.forward * shootDist, Color.red);

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootTimer = 0;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }
    }
}