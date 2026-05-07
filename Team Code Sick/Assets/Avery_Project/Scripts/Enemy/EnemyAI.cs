using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] private Renderer rend;
    [SerializeField] private int HP = 40;

    private Color originalColor;

    private void Start()
    {
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();

        originalColor = rend.material.color;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP: {HP}");

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}