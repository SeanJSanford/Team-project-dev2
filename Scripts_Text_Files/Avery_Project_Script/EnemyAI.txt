using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("Enemy Health")]
    [SerializeField] private int maxHP = 40;
    [SerializeField] private int currentHP;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Transform healthBarFill;

    private Color originalColor;
    private Vector3 originalHealthBarScale;

    private void Start()
    {
        currentHP = maxHP;

        if (rend == null)
            rend = GetComponentInChildren<Renderer>();

        originalColor = rend.material.color;

        if (healthBarFill != null)
            originalHealthBarScale = healthBarFill.localScale;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHealthBar();

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            GetComponent<LootDrop>()?.DropLoot();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent = (float)currentHP / maxHP;

        healthBarFill.localScale = new Vector3(
            originalHealthBarScale.x * healthPercent,
            originalHealthBarScale.y,
            originalHealthBarScale.z
        );
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}