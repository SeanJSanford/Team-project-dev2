using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss1 : MonoBehaviour, Idamage
{

    [Header("Components")]
    [SerializeField] Renderer rend;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] public ParticleSystem destroyEffect;

    [Header("Stats")]
    [Range(20, 100)][SerializeField] int baseHP;
    [Range(1, 15)][SerializeField] float faceTargetSpeed;
    [Range(1, 10)][SerializeField] float speed;
    [Range(1, 10)][SerializeField] float stopDist;

    [Header("Weapons")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [Range(0, 25)][SerializeField] int gunRotateSpeed;
    [Range(.1f, 2)][SerializeField] float shootRate;

    public static Boss1 instance;
    public bool phase1;
    public bool phase2;
    float HP;
    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    bool playerInTrigger;
    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        colorOrig = rend.material.color;
        HP = DifficultyRampUp.instance.EnemyHPRampUp(baseHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP >= (HP * 0.5))
        {
            phase1 = true;
            phase2 = false;
        }
        else
        {
            phase1 = false;
            phase2 = true;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            gamemanager.instance.updateEnemyCount(-1);
            //GetComponent<EnemyLoot>().DropLoot();
            if (EnemySpawnsCenter.instance.currentEnemies.Count == 1)
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
}
