using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyLaser : MonoBehaviour, Idamage, ICharacter
{
    [Header("Audio")]
    [SerializeField] AudioSource audEnemy;

    [SerializeField] AudioClip[] audHurt;
    [Range(0f, 1f)][SerializeField] float audHurtVol = 0.4f;

    [SerializeField] AudioClip audDeath;
    [Range(0f, 1f)][SerializeField] float audDeathVol = 0.5f;

    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] public ParticleSystem destroyEffect;

    [Header("Stats")]
    [Range(1, 15)][SerializeField] int baseHP;
    [Range(10, 100)][SerializeField] int rotateSpeed;
    [Range(1, 10)][SerializeField] float stopDist;
    [SerializeField] float _Speed;
    [SerializeField] float _Damage;
    [SerializeField] float _Resistance;
    [SerializeField] float _shootRate;

    Color colorOrig;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;

    public GameObject[] lasers;

    public float HP { get; set; }
    public float speed { get; set; }
    public float Damage { get; set; }
    public float Resistance { get; set; }
    public bool timerLock { get; set; }
    public float shootRate { get; set; }
    public Element elementType { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audEnemy == null)
            audEnemy = GetComponent<AudioSource>();

        colorOrig = rend.material.color;
        HP = DifficultyRampUp.instance.EnemyHPRampUp(baseHP);
        speed = _Speed;
        Damage = _Damage;
        Resistance = _Resistance;
        shootRate = _shootRate;
        elementType = Element.ElementObject((int)LevelCreation.instance.roomElements[gamemanager.instance.currentRoom]);
        for (int i = 0; i < lasers.Length; i++)
        {
            IBulletSpawner bulletSpawner = lasers[i].GetComponent<IBulletSpawner>();
            if (bulletSpawner != null)
            {
                bulletSpawner.SetElement(elementType);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gamemanager.instance.playerInRoom)
        {
            playerDir = gamemanager.instance.player.transform.position - transform.position;
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
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
        HP -= amount / Resistance;

        if (HP <= 0)
        {
            if (audDeath != null)
            {
                AudioSource.PlayClipAtPoint(audDeath, transform.position, audDeathVol);
            }

            GetComponent<EnemyLoot>().DropLoot();

            if (EnemySpawnsCenter.instance.currentEnemies.Count == 1)
                FindObjectOfType<PlayerSkillPoints>().AddEnemyKill();

            EnemySpawnsCenter.instance.RemoveEnemy(gameObject);

            destroyEffect.transform.position = gameObject.transform.position;

            Destroy(gameObject);
            Instantiate(destroyEffect);
        }
        else
        {
            PlayHurtSound();
            StartCoroutine(flashRed());
            //StartCoroutine(flashRed());
            destroyEffect.transform.position = gameObject.transform.position;
            Instantiate(destroyEffect);
        }
    }

    void PlayHurtSound()
    {
        if (audEnemy == null)
            return;

        if (audHurt == null || audHurt.Length == 0)
            return;

        AudioClip clip = audHurt[Random.Range(0, audHurt.Length)];

        if (clip != null)
        {
            audEnemy.PlayOneShot(clip, audHurtVol);
        }
    }

    IEnumerator flashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
    }

    public void ModifyStat(NxStatType stat, float amount)
    {
        switch (stat)
        {
            case NxStatType.HP:
                HP += amount;
                break;

            case NxStatType.Speed:
                speed += amount;
                break;

            case NxStatType.Damage:
                Damage += amount;
                break;

            case NxStatType.Resistance:
                Resistance += amount;
                break;

            case NxStatType.FireRate:
                shootRate += amount;
                break;
        }
    }
    public void SetStat(NxStatType stat, float amount)
    {
        switch (stat)
        {
            case NxStatType.HP:
                HP = amount;
                break;

            case NxStatType.Speed:
                speed = amount;
                break;

            case NxStatType.Damage:
                Damage = amount;
                break;

            case NxStatType.Resistance:
                Resistance = amount;
                break;

            case NxStatType.FireRate:
                shootRate = amount;
                break;
        }
    }

    public float GetStat(NxStatType stat)
    {
        switch (stat)
        {
            case NxStatType.HP:
                return HP;

            case NxStatType.Speed:
                return speed;

            case NxStatType.Damage:
                return Damage;

            case NxStatType.Resistance:
                return Resistance;

            case NxStatType.FireRate:
                return shootRate;

            default:
                return 0f;
        }
    }
}
