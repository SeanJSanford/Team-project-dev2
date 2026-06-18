using UnityEngine;
using System.Collections;

public class laser : MonoBehaviour, IBulletSpawner
{
    [SerializeField] LineRenderer laserLine;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] GameObject hiteffect;
    [SerializeField] Transform laserStartPos;

    [SerializeField] int laserMaxDist;
    [SerializeField] int laserDamage;
    [SerializeField] float damageRate;
    [SerializeField] Renderer rend;

    bool isDamaging;

    public Element elementType { get; set; }

    // Update is called once per frame
    void Start()
    {
        rend.material = gamemanager.instance.elementMaterials[(int)elementType.type];
    }
    void Update()
    {
        createLaser();
    }

    void createLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(laserStartPos.position, laserStartPos.forward, out hit, laserMaxDist))
        {
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, hit.point);
            hiteffect.SetActive(true);
            hiteffect.transform.position = hit.point;

            Idamage dmg = hit.collider.GetComponent<Idamage>();
            ICharacter character = hit.collider.GetComponent<ICharacter>();

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log(hit.collider.name);
                if (dmg != null && !isDamaging)
                {
                    if (character != null && !character.timerLock)
                    {
                        gamemanager.instance.StartRoutine(elementType.ModifyTargetDebuff(character));
                    }
                    StartCoroutine(damageTime(dmg));
                }
            }
        }
        else
        {
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, laserStartPos.position + laserStartPos.forward * laserMaxDist);
            hiteffect.SetActive(false);
        }
    }

    IEnumerator damageTime(Idamage d)
    {
        isDamaging = true;
        d.takeDamage((int)DifficultyRampUp.instance.EnemyDamageRampUp(laserDamage));
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

    public void SetElement(Element element)
    {
        elementType = element;
    }
}
