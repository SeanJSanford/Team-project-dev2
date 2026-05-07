using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer rend;
    [SerializeField] int HP = 30;

    Color colorOrig;

    void Start()
    {
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();

        colorOrig = rend.material.color;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorOrig;
    }
}