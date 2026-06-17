using System.Buffers.Text;
using System.Collections;
using UnityEngine;

public class damage : MonoBehaviour
{

    enum damageType { bullet, stationary, DOT }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] float baseDamageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int bulletSpeed;
    [SerializeField] int bulletDestroyTime;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Renderer rend;

    public float damageAmount;
    bool isDamaging;
    GameObject owner;
    Element elementType;

    void Awake()
    {
        elementType = new Lightning();// Element.RandomElement(Random.Range(0, 4));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageAmount = DifficultyRampUp.instance.EnemyDamageRampUp(baseDamageAmount);
        rend.material = gamemanager.instance.elementMaterials[(int)elementType.type];
        if (type == damageType.bullet)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
            Destroy(gameObject, bulletDestroyTime);
        }
    }
    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
        //elementType
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return;

        if (owner != null && other.transform.root.gameObject == owner)
            return;

        playerMovement player = other.GetComponentInParent<playerMovement>();

        if (player != null && player.IsInvincible() && type == damageType.bullet)
            return;

        Idamage dmg = other.GetComponent<Idamage>();
        ICharacter character = other.GetComponent<ICharacter>();

        if (dmg != null && type != damageType.DOT)
        {
            if (character != null && !character.timerLock)
                gamemanager.instance.StartRoutine(elementType.ModifyTargetDebuff(character));
            dmg.takeDamage((int)damageAmount);
        }

        if (type == damageType.bullet)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        Idamage dmg = other.GetComponent<Idamage>();
        if (dmg != null && type == damageType.DOT && !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(Idamage d)
    {
        isDamaging = true;
        d.takeDamage((int)damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
